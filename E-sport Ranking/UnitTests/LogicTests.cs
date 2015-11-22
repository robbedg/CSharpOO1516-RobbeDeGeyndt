using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DataEntities;
using DataAccessInterfaces;
using DataAccessImplementation;
using LogicInterfaces;
using LogicImplementation;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Reflection;

namespace UnitTests
{
    [TestClass]
    public class LogicTests
    {
        [ClassInitialize()]
        public static void MyClassInitialize(TestContext testContext)
        {
            // load all assaemblmies in the bin-directory for auto detection
            // of interface implementations and class/struct definitions
            // loads alls assemblies in the same directory
            var path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            var di = new DirectoryInfo(path);
            foreach (var file in di.GetFiles("*.dll"))
            {
                try
                {
                    var nextAssembly = Assembly.LoadFile(file.FullName);
                }
                catch (BadImageFormatException)
                {
                    // Not a .net assembly  - ignore
                }
            }
        }
        
        private object getInterfaceImplementation(Type wantedInterface)
        {
            Type unknownClass = null;
            foreach (var asm in AppDomain.CurrentDomain.GetAssemblies())
            {
                unknownClass = new List<Type>(asm.GetTypes()).Where(x => wantedInterface.IsAssignableFrom(x) && !x.IsInterface).FirstOrDefault();
                if (unknownClass != null) break;
            }
            if (unknownClass == null) return null;
            return Activator.CreateInstance(unknownClass);
        }
        private Type getInterfaceImplementatingClass(Type wantedInterface)
        {
            Type unknownClass = null;
            foreach (var asm in AppDomain.CurrentDomain.GetAssemblies())
            {
                unknownClass = new List<Type>(asm.GetTypes()).Where(x => wantedInterface.IsAssignableFrom(x) && !x.IsInterface).FirstOrDefault();
                if (unknownClass != null) break;
            }
            if (unknownClass == null) return null;
            return (Type)unknownClass;
        }

        

        private void ClearAllData()
        {
            var DAL = (IGameRankingDataAccess)getInterfaceImplementation(typeof(IGameRankingDataAccess));
            DAL.ClearAllData();
            DAL.SubmitGameListChanges();
            DAL.SubmitPlayerListChanges();
            DAL.SubmitTeamListChanges();
            DAL.SubmitmatchListChanges();
            DAL.SubmitRankingListChanges();
        }

        #region Game Manipulations

        [TestMethod]
        public void TestGameManipulations()
        {
            ClearAllData();
            var gameLogic = (IGameManipulations)getInterfaceImplementation(typeof(IGameManipulations));
            Assert.IsNotNull(gameLogic.GetGames(), " - GameManipulator.GetGames is null after ClearAllData");
            var game = new GameType() { Name = "testgame", ParticipantType = ParticipantTypes.All };
            gameLogic.AddOrUpdateGame(game);
            gameLogic = null;
            gameLogic = (IGameManipulations)getInterfaceImplementation(typeof(IGameManipulations));
            Assert.IsTrue(gameLogic.GetGames().Contains(game), " - GameManipulator does not persist new game");
            game = gameLogic.GetGames().First();
            game.ParticipantType = ParticipantTypes.Solo;
            gameLogic.AddOrUpdateGame(game);
            gameLogic = null;
            gameLogic = (IGameManipulations)getInterfaceImplementation(typeof(IGameManipulations));
            Assert.IsTrue(gameLogic.GetGames().First().ParticipantType == ParticipantTypes.Solo, " - GameManipulator does not persist game changes");
        }

        #endregion Game Manipulations

        #region Player Manipulations

        [TestMethod]
        public void TestSimplePlayerManipulations()
        {
            ClearAllData();
            var playerLogic = (IPlayerManipulations)getInterfaceImplementation(typeof(IPlayerManipulations));
            Assert.IsNotNull(playerLogic.GetPlayers(), " - PlayerManipulator.GetPlayers is null after ClearAllData");
            PlayerType player = new PlayerType() { Name = "testplayer", Mail = "testplayer@odisee.be", Tag = "Tagname" };
            playerLogic.AddOrUpdatePlayer(player);
            playerLogic = null;
            playerLogic = (IPlayerManipulations)getInterfaceImplementation(typeof(IPlayerManipulations));
            Assert.IsTrue(playerLogic.GetPlayers().Contains(player), " - PlayerManipulator does not persist new player");
            player = playerLogic.GetPlayers().First();
            player.Tag = "newtag";
            playerLogic.AddOrUpdatePlayer(player);
            playerLogic = null;
            playerLogic = (IPlayerManipulations)getInterfaceImplementation(typeof(IPlayerManipulations));
            Assert.IsTrue(playerLogic.GetPlayers().First().Tag == "newtag", " - PlayerManipulator does not persist player changes");
        }

        [TestMethod]
        public void TestGetPlayersForGame()
        {
            ClearAllData();
            var playerLogic = (IPlayerManipulations)getInterfaceImplementation(typeof(IPlayerManipulations));
            var gameLogic = (IGameManipulations)getInterfaceImplementation(typeof(IGameManipulations));
            var matchLogic = (IMatchManipulations)getInterfaceImplementation(typeof(IMatchManipulations));

            Assert.IsNotNull(playerLogic.GetPlayers(), " - PlayerManipulator.GetPlayers is null after ClearAllData");
            PlayerType player1 = new PlayerType() { Name = "test1", Mail = "test1", Tag = "test1" };
            PlayerType player2 = new PlayerType() { Name = "test2", Mail = "test2", Tag = "test2" };
            PlayerType player3 = new PlayerType() { Name = "test3", Mail = "test3", Tag = "test3" };
            PlayerType player4 = new PlayerType() { Name = "test4", Mail = "test4", Tag = "test4" };
            playerLogic.AddOrUpdatePlayer(player1);
            playerLogic.AddOrUpdatePlayer(player2);
            playerLogic.AddOrUpdatePlayer(player3);
            playerLogic.AddOrUpdatePlayer(player4);
            var game1 = new GameType() { Name = "game1", ParticipantType = ParticipantTypes.All };
            var game2 = new GameType() { Name = "game2", ParticipantType = ParticipantTypes.Solo };
            gameLogic.AddOrUpdateGame(game1);
            gameLogic.AddOrUpdateGame(game2);
            var match1 = new SoloMatch()
            {
                GameID = game1,
                Category = MatchCategories.Competition,
                dateTime = DateTime.Now,
                Players = new List<PlayerType>() { player1, player2 },
                Scores = new List<int>() { 1, 2 }
            };
            var match2 = new SoloMatch()
            {
                GameID = game2,
                Category = MatchCategories.Training,
                dateTime = DateTime.Now,
                Players = new List<PlayerType>() { player1, player3 },
                Scores = new List<int>() { 1, 2 }
            };
            var match3 = new SoloMatch()
            {
                GameID = game2,
                Category = MatchCategories.Training,
                dateTime = DateTime.Now,
                Players = new List<PlayerType>() { player2, player3 },
                Scores = new List<int>() { 1, 2 }
            };
            matchLogic.AddOrUpdateSoloMatch(match1);
            matchLogic.AddOrUpdateSoloMatch(match2);
            matchLogic.AddOrUpdateSoloMatch(match3);

            var playersForGame2 = playerLogic.GetPlayersForgame(game2);
            Assert.IsTrue(
                ((playersForGame2.Count == 3) &&
                 (playersForGame2.Contains(player1)) &&
                 (playersForGame2.Contains(player2)) &&
                 (playersForGame2.Contains(player3))),
                " - PlayerManipulator \"GetPlayersForGame\" Not OK");
        }
        
        [TestMethod]
        public void TestGetGamesForPlayer()
        {
            ClearAllData();
            var playerLogic = (IPlayerManipulations)getInterfaceImplementation(typeof(IPlayerManipulations));
            var gameLogic = (IGameManipulations)getInterfaceImplementation(typeof(IGameManipulations));
            var matchLogic = (IMatchManipulations)getInterfaceImplementation(typeof(IMatchManipulations));

            Assert.IsNotNull(playerLogic.GetPlayers(), " - PlayerManipulator.GetPlayers is null after ClearAllData");
            PlayerType player1 = new PlayerType() { Name = "test1", Mail = "test1", Tag = "test1" };
            PlayerType player2 = new PlayerType() { Name = "test2", Mail = "test2", Tag = "test2" };
            PlayerType player3 = new PlayerType() { Name = "test3", Mail = "test3", Tag = "test3" };
            PlayerType player4 = new PlayerType() { Name = "test4", Mail = "test4", Tag = "test4" };
            playerLogic.AddOrUpdatePlayer(player1);
            playerLogic.AddOrUpdatePlayer(player2);
            playerLogic.AddOrUpdatePlayer(player3);
            playerLogic.AddOrUpdatePlayer(player4);
            var game1 = new GameType() { Name = "game1", ParticipantType = ParticipantTypes.All };
            var game2 = new GameType() { Name = "game2", ParticipantType = ParticipantTypes.Solo };
            gameLogic.AddOrUpdateGame(game1);
            gameLogic.AddOrUpdateGame(game2);
            var match1 = new SoloMatch()
            {
                GameID = game1,
                Category = MatchCategories.Competition,
                dateTime = DateTime.Now,
                Players = new List<PlayerType>() { player1, player2 },
                Scores = new List<int>() { 1, 2 }
            };
            var match2 = new SoloMatch()
            {
                GameID = game2,
                Category = MatchCategories.Training,
                dateTime = DateTime.Now,
                Players = new List<PlayerType>() { player1, player3 },
                Scores = new List<int>() { 1, 2 }
            };
            var match3 = new SoloMatch()
            {
                GameID = game2,
                Category = MatchCategories.Training,
                dateTime = DateTime.Now,
                Players = new List<PlayerType>() { player2, player3 },
                Scores = new List<int>() { 1, 2 }
            };
            matchLogic.AddOrUpdateSoloMatch(match1);
            matchLogic.AddOrUpdateSoloMatch(match2);
            matchLogic.AddOrUpdateSoloMatch(match3);

            var gamesForPlayer1 = playerLogic.GetGamesForPlayer(player1);
            Assert.IsTrue(
                ((gamesForPlayer1.Count == 2) &&
                 (gamesForPlayer1.Contains(game1)) &&
                 (gamesForPlayer1.Contains(game2))),
                " - PlayerManipulator \"GetGamesForPlayer\" Not OK");
        }

        [TestMethod]
        public void TestGetMatchesForPlayer()
        {
            ClearAllData();
            var playerLogic = (IPlayerManipulations)getInterfaceImplementation(typeof(IPlayerManipulations));
            var gameLogic = (IGameManipulations)getInterfaceImplementation(typeof(IGameManipulations));
            var matchLogic = (IMatchManipulations)getInterfaceImplementation(typeof(IMatchManipulations));

            Assert.IsNotNull(playerLogic.GetPlayers(), " - PlayerManipulator.GetPlayers is null after ClearAllData");
            PlayerType player1 = new PlayerType() { Name = "test1", Mail = "test1", Tag = "test1" };
            PlayerType player2 = new PlayerType() { Name = "test2", Mail = "test2", Tag = "test2" };
            PlayerType player3 = new PlayerType() { Name = "test3", Mail = "test3", Tag = "test3" };
            PlayerType player4 = new PlayerType() { Name = "test4", Mail = "test4", Tag = "test4" };
            playerLogic.AddOrUpdatePlayer(player1);
            playerLogic.AddOrUpdatePlayer(player2);
            playerLogic.AddOrUpdatePlayer(player3);
            playerLogic.AddOrUpdatePlayer(player4);
            var game1 = new GameType() { Name = "game1", ParticipantType = ParticipantTypes.All };
            var game2 = new GameType() { Name = "game2", ParticipantType = ParticipantTypes.Solo };
            gameLogic.AddOrUpdateGame(game1);
            gameLogic.AddOrUpdateGame(game2);
            var match1 = new SoloMatch()
            {
                GameID = game1,
                Category = MatchCategories.Competition,
                dateTime = DateTime.Now,
                Players = new List<PlayerType>() { player1, player2 },
                Scores = new List<int>() { 1, 2 }
            };
            var match2 = new SoloMatch()
            {
                GameID = game2,
                Category = MatchCategories.Training,
                dateTime = DateTime.Now,
                Players = new List<PlayerType>() { player1, player3 },
                Scores = new List<int>() { 1, 2 }
            };
            var match3 = new SoloMatch()
            {
                GameID = game2,
                Category = MatchCategories.Training,
                dateTime = DateTime.Now,
                Players = new List<PlayerType>() { player2, player3 },
                Scores = new List<int>() { 1, 2 }
            };
            matchLogic.AddOrUpdateSoloMatch(match1);
            matchLogic.AddOrUpdateSoloMatch(match2);
            matchLogic.AddOrUpdateSoloMatch(match3);

            var matchesForPlayer1 = playerLogic.GetMatchesForPlayer(player1);
            Assert.IsTrue(
                ((matchesForPlayer1.Count == 2) &&
                 (matchesForPlayer1.Contains(match1)) &&
                 (matchesForPlayer1.Contains(match2))),
                " - PlayerManipulator \"GetMatchesForPlayer\" Not OK");
        }


        [TestMethod]
        public void TestGetGameMatchesForPlayer()
        {
            ClearAllData();
            var playerLogic = (IPlayerManipulations)getInterfaceImplementation(typeof(IPlayerManipulations));
            var gameLogic = (IGameManipulations)getInterfaceImplementation(typeof(IGameManipulations));
            var matchLogic = (IMatchManipulations)getInterfaceImplementation(typeof(IMatchManipulations));

            Assert.IsNotNull(playerLogic.GetPlayers(), " - PlayerManipulator.GetPlayers is null after ClearAllData");
            PlayerType player1 = new PlayerType() { Name = "test1", Mail = "test1", Tag = "test1" };
            PlayerType player2 = new PlayerType() { Name = "test2", Mail = "test2", Tag = "test2" };
            PlayerType player3 = new PlayerType() { Name = "test3", Mail = "test3", Tag = "test3" };
            PlayerType player4 = new PlayerType() { Name = "test4", Mail = "test4", Tag = "test4" };
            playerLogic.AddOrUpdatePlayer(player1);
            playerLogic.AddOrUpdatePlayer(player2);
            playerLogic.AddOrUpdatePlayer(player3);
            playerLogic.AddOrUpdatePlayer(player4);
            var game1 = new GameType() { Name = "game1", ParticipantType = ParticipantTypes.All };
            var game2 = new GameType() { Name = "game2", ParticipantType = ParticipantTypes.Solo };
            gameLogic.AddOrUpdateGame(game1);
            gameLogic.AddOrUpdateGame(game2);
            var match1 = new SoloMatch()
            {
                GameID = game1,
                Category = MatchCategories.Competition,
                dateTime = DateTime.Now,
                Players = new List<PlayerType>() { player1, player2 },
                Scores = new List<int>() { 1, 2 }
            };
            var match2 = new SoloMatch()
            {
                GameID = game2,
                Category = MatchCategories.Training,
                dateTime = DateTime.Now,
                Players = new List<PlayerType>() { player1, player3 },
                Scores = new List<int>() { 1, 2 }
            };
            var match3 = new SoloMatch()
            {
                GameID = game2,
                Category = MatchCategories.Training,
                dateTime = DateTime.Now,
                Players = new List<PlayerType>() { player2, player3 },
                Scores = new List<int>() { 1, 2 }
            };
            matchLogic.AddOrUpdateSoloMatch(match1);
            matchLogic.AddOrUpdateSoloMatch(match2);
            matchLogic.AddOrUpdateSoloMatch(match3);

            var matchesForPlayer1 = playerLogic.GetGameMatchesForPlayer(game1, player1);
            Assert.IsTrue(
                ((matchesForPlayer1.Count == 1) &&
                 (matchesForPlayer1.Contains(match1))),
                " - PlayerManipulator \"GetGameMatchesForPlayer\" Not OK");
        }

        #endregion Player Manipulations

        #region Team manipulations

        [TestMethod]
        public void TestSimpleTeamManipulations()
        {
            ClearAllData();
            var teamLogic = (ITeamManipulations)getInterfaceImplementation(typeof(ITeamManipulations));
            Assert.IsNotNull(teamLogic.GetTeams(), " - TeamManipulator.GetPlayers is null after ClearAllData");
            PlayerType player1 = new PlayerType() { Name = "test1", Mail = "test1", Tag = "test1" };
            PlayerType player2 = new PlayerType() { Name = "test2", Mail = "test2", Tag = "test2" };
            var team1 = new TeamType() { Name = "team1", Members = new List<PlayerType>() { player1, player2 } };
            teamLogic.AddOrUpdateTeam(team1);
            teamLogic = null;
            teamLogic = (ITeamManipulations)getInterfaceImplementation(typeof(ITeamManipulations));
            Assert.IsTrue(teamLogic.GetTeams().Contains(team1), " - TeamManipulator does not persist new team");
            var team = teamLogic.GetTeams().First();
            team.Members = new List<PlayerType>() { player1 };
            teamLogic.AddOrUpdateTeam(team);
            teamLogic = null;
            teamLogic = (ITeamManipulations)getInterfaceImplementation(typeof(ITeamManipulations));
            Assert.IsTrue(((teamLogic.GetTeams().First().Members.Count == 1) &&
                           (teamLogic.GetTeams().First().Members.Contains(player1))),
                " - TeamManipulator does not persist team changes");
        }

        [TestMethod]
        public void TestGetTeamsForGame()
        {
            ClearAllData();
            var playerLogic = (IPlayerManipulations)getInterfaceImplementation(typeof(IPlayerManipulations));
            var teamLogic = (ITeamManipulations)getInterfaceImplementation(typeof(ITeamManipulations));
            var gameLogic = (IGameManipulations)getInterfaceImplementation(typeof(IGameManipulations));
            var matchLogic = (IMatchManipulations)getInterfaceImplementation(typeof(IMatchManipulations));

            PlayerType player1 = new PlayerType() { Name = "test1", Mail = "test1", Tag = "test1" };
            PlayerType player2 = new PlayerType() { Name = "test2", Mail = "test2", Tag = "test2" };
            PlayerType player3 = new PlayerType() { Name = "test3", Mail = "test3", Tag = "test3" };
            PlayerType player4 = new PlayerType() { Name = "test4", Mail = "test4", Tag = "test4" };
            PlayerType player5 = new PlayerType() { Name = "test5", Mail = "test5", Tag = "test5" };
            PlayerType player6 = new PlayerType() { Name = "test6", Mail = "test6", Tag = "test6" };
            playerLogic.AddOrUpdatePlayer(player1);
            playerLogic.AddOrUpdatePlayer(player2);
            playerLogic.AddOrUpdatePlayer(player3);
            playerLogic.AddOrUpdatePlayer(player4);
            playerLogic.AddOrUpdatePlayer(player5);
            playerLogic.AddOrUpdatePlayer(player6);
            var game1 = new GameType() { Name = "game1", ParticipantType = ParticipantTypes.All};
            var game2 = new GameType() { Name = "game2", ParticipantType = ParticipantTypes.Team };
            gameLogic.AddOrUpdateGame(game1);
            gameLogic.AddOrUpdateGame(game2);

            var team1 = new TeamType() { Name = "team1", Members = new List<PlayerType>() { player1, player2 } };
            var team2 = new TeamType() { Name = "team2", Members = new List<PlayerType>() { player3, player4 } };
            var team3 = new TeamType() { Name = "team3", Members = new List<PlayerType>() { player5, player6 } };

            var match1 = new TeamMatch()
            {
                GameID = game1,
                Category = MatchCategories.Competition,
                dateTime = DateTime.Now,
                Teams = new List<TeamType>() { team1, team2 },
                Scores = new List<int>() { 1, 2 }
            };
            var match2 = new TeamMatch()
            {
                GameID = game2,
                Category = MatchCategories.Training,
                dateTime = DateTime.Now,
                Teams = new List<TeamType>() { team1, team3 },
                Scores = new List<int>() { 1, 2 }
            };
            var match3 = new TeamMatch()
            {
                GameID = game2,
                Category = MatchCategories.Training,
                dateTime = DateTime.Now,
                Teams = new List<TeamType>() { team2, team3 },
                Scores = new List<int>() { 1, 2 }
            };
            matchLogic.AddOrUpdateTeamMatch(match1);
            matchLogic.AddOrUpdateTeamMatch(match2);
            matchLogic.AddOrUpdateTeamMatch(match3);

            var teamsForGame2 = teamLogic.GetTeamsforGame(game2);
            Assert.IsTrue(
                ((teamsForGame2.Count == 3) &&
                 (teamsForGame2.Contains(team1)) &&
                 (teamsForGame2.Contains(team2)) &&
                 (teamsForGame2.Contains(team3))),
                " - TeamManipulator \"GetteamsforGame\" Not OK");
        }
        
        [TestMethod]
        public void TestGetGamesForTeam()
        {
            ClearAllData();
            var playerLogic = (IPlayerManipulations)getInterfaceImplementation(typeof(IPlayerManipulations));
            var teamLogic = (ITeamManipulations)getInterfaceImplementation(typeof(ITeamManipulations));
            var gameLogic = (IGameManipulations)getInterfaceImplementation(typeof(IGameManipulations));
            var matchLogic = (IMatchManipulations)getInterfaceImplementation(typeof(IMatchManipulations));

            PlayerType player1 = new PlayerType() { Name = "test1", Mail = "test1", Tag = "test1" };
            PlayerType player2 = new PlayerType() { Name = "test2", Mail = "test2", Tag = "test2" };
            PlayerType player3 = new PlayerType() { Name = "test3", Mail = "test3", Tag = "test3" };
            PlayerType player4 = new PlayerType() { Name = "test4", Mail = "test4", Tag = "test4" };
            PlayerType player5 = new PlayerType() { Name = "test5", Mail = "test5", Tag = "test5" };
            PlayerType player6 = new PlayerType() { Name = "test6", Mail = "test6", Tag = "test6" };
            playerLogic.AddOrUpdatePlayer(player1);
            playerLogic.AddOrUpdatePlayer(player2);
            playerLogic.AddOrUpdatePlayer(player3);
            playerLogic.AddOrUpdatePlayer(player4);
            playerLogic.AddOrUpdatePlayer(player5);
            playerLogic.AddOrUpdatePlayer(player6);
            var game1 = new GameType() { Name = "game1", ParticipantType = ParticipantTypes.Team};
            var game2 = new GameType() { Name = "game2", ParticipantType = ParticipantTypes.All };
            gameLogic.AddOrUpdateGame(game1);
            gameLogic.AddOrUpdateGame(game2);

            var team1 = new TeamType() { Name = "team1", Members = new List<PlayerType>() { player1, player2 } };
            var team2 = new TeamType() { Name = "team2", Members = new List<PlayerType>() { player3, player4 } };
            var team3 = new TeamType() { Name = "team3", Members = new List<PlayerType>() { player5, player6 } };

            var match1 = new TeamMatch()
            {
                GameID = game1,
                Category = MatchCategories.Competition,
                dateTime = DateTime.Now,
                Teams = new List<TeamType>() { team1, team2 },
                Scores = new List<int>() { 1, 2 }
            };
            var match2 = new TeamMatch()
            {
                GameID = game2,
                Category = MatchCategories.Training,
                dateTime = DateTime.Now,
                Teams = new List<TeamType>() { team1, team3 },
                Scores = new List<int>() { 1, 2 }
            };
            var match3 = new TeamMatch()
            {
                GameID = game2,
                Category = MatchCategories.Training,
                dateTime = DateTime.Now,
                Teams = new List<TeamType>() { team2, team3 },
                Scores = new List<int>() { 1, 2 }
            };
            matchLogic.AddOrUpdateTeamMatch(match1);
            matchLogic.AddOrUpdateTeamMatch(match2);
            matchLogic.AddOrUpdateTeamMatch(match3);

            var gamesForTeam1 = teamLogic.GetGamesForTeam(team1);
            Assert.IsTrue(
                    ((gamesForTeam1.Count == 2) &&
                     (gamesForTeam1.Contains(game1)) &&
                     (gamesForTeam1.Contains(game2))),
                    " - TeamManipulator \"GetGamesForTeam\" Not OK");
        }

        [TestMethod]
        public void TestGetMatchesForTeam()
        {
            ClearAllData();
            var playerLogic = (IPlayerManipulations)getInterfaceImplementation(typeof(IPlayerManipulations));
            var teamLogic = (ITeamManipulations)getInterfaceImplementation(typeof(ITeamManipulations));
            var gameLogic = (IGameManipulations)getInterfaceImplementation(typeof(IGameManipulations));
            var matchLogic = (IMatchManipulations)getInterfaceImplementation(typeof(IMatchManipulations));

            PlayerType player1 = new PlayerType() { Name = "test1", Mail = "test1", Tag = "test1" };
            PlayerType player2 = new PlayerType() { Name = "test2", Mail = "test2", Tag = "test2" };
            PlayerType player3 = new PlayerType() { Name = "test3", Mail = "test3", Tag = "test3" };
            PlayerType player4 = new PlayerType() { Name = "test4", Mail = "test4", Tag = "test4" };
            PlayerType player5 = new PlayerType() { Name = "test5", Mail = "test5", Tag = "test5" };
            PlayerType player6 = new PlayerType() { Name = "test6", Mail = "test6", Tag = "test6" };
            playerLogic.AddOrUpdatePlayer(player1);
            playerLogic.AddOrUpdatePlayer(player2);
            playerLogic.AddOrUpdatePlayer(player3);
            playerLogic.AddOrUpdatePlayer(player4);
            playerLogic.AddOrUpdatePlayer(player5);
            playerLogic.AddOrUpdatePlayer(player6);
            var game1 = new GameType() { Name = "game1", ParticipantType = ParticipantTypes.Team };
            var game2 = new GameType() { Name = "game2", ParticipantType = ParticipantTypes.All };
            gameLogic.AddOrUpdateGame(game1);
            gameLogic.AddOrUpdateGame(game2);

            var team1 = new TeamType() { Name = "team1", Members = new List<PlayerType>() { player1, player2 } };
            var team2 = new TeamType() { Name = "team2", Members = new List<PlayerType>() { player3, player4 } };
            var team3 = new TeamType() { Name = "team3", Members = new List<PlayerType>() { player5, player6 } };

            var match1 = new TeamMatch()
            {
                GameID = game1,
                Category = MatchCategories.Competition,
                dateTime = DateTime.Now,
                Teams = new List<TeamType>() { team1, team2 },
                Scores = new List<int>() { 1, 2 }
            };
            var match2 = new TeamMatch()
            {
                GameID = game2,
                Category = MatchCategories.Training,
                dateTime = DateTime.Now,
                Teams = new List<TeamType>() { team1, team3 },
                Scores = new List<int>() { 1, 2 }
            };
            var match3 = new TeamMatch()
            {
                GameID = game2,
                Category = MatchCategories.Training,
                dateTime = DateTime.Now,
                Teams = new List<TeamType>() { team2, team3 },
                Scores = new List<int>() { 1, 2 }
            };
            matchLogic.AddOrUpdateTeamMatch(match1);
            matchLogic.AddOrUpdateTeamMatch(match2);
            matchLogic.AddOrUpdateTeamMatch(match3);

            var matchesForTeam1 = teamLogic.GetMatchesForTeam(team1);
            Assert.IsTrue(
                ((matchesForTeam1.Count == 2) &&
                 (matchesForTeam1.Contains(match1)) &&
                 (matchesForTeam1.Contains(match2))),
                " - TeamManipulator \"GetMatchesForTeam\" Not OK");
        }
        
        [TestMethod]
        public void TestGetGameMatchesForTeam()
        {
            ClearAllData();
            var playerLogic = (IPlayerManipulations)getInterfaceImplementation(typeof(IPlayerManipulations));
            var teamLogic = (ITeamManipulations)getInterfaceImplementation(typeof(ITeamManipulations));
            var gameLogic = (IGameManipulations)getInterfaceImplementation(typeof(IGameManipulations));
            var matchLogic = (IMatchManipulations)getInterfaceImplementation(typeof(IMatchManipulations));

            PlayerType player1 = new PlayerType() { Name = "test1", Mail = "test1", Tag = "test1" };
            PlayerType player2 = new PlayerType() { Name = "test2", Mail = "test2", Tag = "test2" };
            PlayerType player3 = new PlayerType() { Name = "test3", Mail = "test3", Tag = "test3" };
            PlayerType player4 = new PlayerType() { Name = "test4", Mail = "test4", Tag = "test4" };
            PlayerType player5 = new PlayerType() { Name = "test5", Mail = "test5", Tag = "test5" };
            PlayerType player6 = new PlayerType() { Name = "test6", Mail = "test6", Tag = "test6" };
            playerLogic.AddOrUpdatePlayer(player1);
            playerLogic.AddOrUpdatePlayer(player2);
            playerLogic.AddOrUpdatePlayer(player3);
            playerLogic.AddOrUpdatePlayer(player4);
            playerLogic.AddOrUpdatePlayer(player5);
            playerLogic.AddOrUpdatePlayer(player6);
            var game1 = new GameType() { Name = "game1", ParticipantType = ParticipantTypes.Team };
            var game2 = new GameType() { Name = "game2", ParticipantType = ParticipantTypes.All };
            gameLogic.AddOrUpdateGame(game1);
            gameLogic.AddOrUpdateGame(game2);

            var team1 = new TeamType() { Name = "team1", Members = new List<PlayerType>() { player1, player2 } };
            var team2 = new TeamType() { Name = "team2", Members = new List<PlayerType>() { player3, player4 } };
            var team3 = new TeamType() { Name = "team3", Members = new List<PlayerType>() { player5, player6 } };

            var match1 = new TeamMatch()
            {
                GameID = game1,
                Category = MatchCategories.Competition,
                dateTime = DateTime.Now,
                Teams = new List<TeamType>() { team1, team2 },
                Scores = new List<int>() { 1, 2 }
            };
            var match2 = new TeamMatch()
            {
                GameID = game2,
                Category = MatchCategories.Training,
                dateTime = DateTime.Now,
                Teams = new List<TeamType>() { team1, team3 },
                Scores = new List<int>() { 1, 2 }
            };
            var match3 = new TeamMatch()
            {
                GameID = game2,
                Category = MatchCategories.Training,
                dateTime = DateTime.Now,
                Teams = new List<TeamType>() { team2, team3 },
                Scores = new List<int>() { 1, 2 }
            };
            matchLogic.AddOrUpdateTeamMatch(match1);
            matchLogic.AddOrUpdateTeamMatch(match2);
            matchLogic.AddOrUpdateTeamMatch(match3);

            var matchesForTeam1 = teamLogic.GetGameMatchesForTeam(game1, team1);
            Assert.IsTrue(
                ((matchesForTeam1.Count == 1) &&
                 (matchesForTeam1.Contains(match1))),
                " - TeamManipulator \"GetGameMatchesForTeam\" Not OK");
        }

        #endregion Team Manipulations
        
        #region Match Manipulations

        [TestMethod]
        public void TestSimpleSoloMatchManipulations()
        {
            ClearAllData();
            var matchLogic = (IMatchManipulations)getInterfaceImplementation(typeof(IMatchManipulations));
            var game1 = new GameType() { Name = "game1", ParticipantType = ParticipantTypes.All };

            Assert.IsNotNull(matchLogic.GetMatchesAll(game1), " - MatchManipulator.GetMatchesAll is null after ClearAllData");
            Assert.IsNotNull(matchLogic.GetMatches(game1,ParticipantTypes.All, MatchCategories.Training), " - MatchManipulator.GetMatches is null after ClearAllData");
            PlayerType player1 = new PlayerType() { Name = "test1", Mail = "test1", Tag = "test1" };
            PlayerType player2 = new PlayerType() { Name = "test2", Mail = "test2", Tag = "test2" };
            PlayerType player3 = new PlayerType() { Name = "test3", Mail = "test3", Tag = "test3" };
            PlayerType player4 = new PlayerType() { Name = "test4", Mail = "test4", Tag = "test4" };

            var match1 = new SoloMatch()
            {
                GameID = game1,
                Category = MatchCategories.Competition,
                dateTime = DateTime.Now,
                Players = new List<PlayerType>() { player1, player2 },
                Scores = new List<int>() { 1, 2 }
            };

            matchLogic.AddOrUpdateSoloMatch(match1);
            matchLogic = null;
            matchLogic = (IMatchManipulations)getInterfaceImplementation(typeof(IMatchManipulations));
            Assert.IsTrue(matchLogic.GetMatchesAll(game1).Contains(match1), " - MatchManipulator does not persist new SoloMatch");
            var match = matchLogic.GetMatchesAll(game1).First() as SoloMatch;
            match.Scores = new List<int>() { 2, 3 };
            matchLogic.AddOrUpdateSoloMatch(match);
            matchLogic = null;
            matchLogic = (IMatchManipulations)getInterfaceImplementation(typeof(IMatchManipulations));
            Assert.IsTrue((((matchLogic.GetMatchesAll(game1).First() as SoloMatch).Scores.Contains(2))&&
                ((matchLogic.GetMatchesAll(game1).First() as SoloMatch).Scores.Contains(2))),
                " - MatchManipulator does not persist SoloMatch changes");
        }

        [TestMethod]
        public void TestSimpleTeamMatchManipulations()
        {
            ClearAllData();
            var playerLogic = (IPlayerManipulations)getInterfaceImplementation(typeof(IPlayerManipulations));
            var teamLogic = (ITeamManipulations)getInterfaceImplementation(typeof(ITeamManipulations));
            var gameLogic = (IGameManipulations)getInterfaceImplementation(typeof(IGameManipulations));
            var matchLogic = (IMatchManipulations)getInterfaceImplementation(typeof(IMatchManipulations));

            PlayerType player1 = new PlayerType() { Name = "test1", Mail = "test1", Tag = "test1" };
            PlayerType player2 = new PlayerType() { Name = "test2", Mail = "test2", Tag = "test2" };
            PlayerType player3 = new PlayerType() { Name = "test3", Mail = "test3", Tag = "test3" };
            PlayerType player4 = new PlayerType() { Name = "test4", Mail = "test4", Tag = "test4" };
            playerLogic.AddOrUpdatePlayer(player1);
            playerLogic.AddOrUpdatePlayer(player2);
            playerLogic.AddOrUpdatePlayer(player3);
            playerLogic.AddOrUpdatePlayer(player4);
            var game1 = new GameType() { Name = "game1", ParticipantType = ParticipantTypes.Team };
            var game2 = new GameType() { Name = "game2", ParticipantType = ParticipantTypes.All };
            gameLogic.AddOrUpdateGame(game1);
            gameLogic.AddOrUpdateGame(game2);

            var team1 = new TeamType() { Name = "team1", Members = new List<PlayerType>() { player1, player2 } };
            var team2 = new TeamType() { Name = "team2", Members = new List<PlayerType>() { player3, player4 } };
            
            var match1 = new TeamMatch()
            {
                GameID = game1,
                Category = MatchCategories.Competition,
                dateTime = DateTime.Now,
                Teams = new List<TeamType>() { team1, team2 },
                Scores = new List<int>() { 1, 2 }
            };
            matchLogic.AddOrUpdateTeamMatch(match1);
            
            matchLogic = null;
            matchLogic = (IMatchManipulations)getInterfaceImplementation(typeof(IMatchManipulations));
            Assert.IsTrue(matchLogic.GetMatchesAll(game1).Contains(match1), " - MatchManipulator does not persist new TeamMatch");
            var match = matchLogic.GetMatchesAll(game1).First() as TeamMatch;
            match.Scores = new List<int>() { 2, 3 };
            matchLogic.AddOrUpdateTeamMatch(match);
            matchLogic = null;
            matchLogic = (IMatchManipulations)getInterfaceImplementation(typeof(IMatchManipulations));
            Assert.IsTrue((((matchLogic.GetMatchesAll(game1).First() as TeamMatch).Scores.Contains(2)) &&
                ((matchLogic.GetMatchesAll(game1).First() as TeamMatch).Scores.Contains(2))),
                " - MatchManipulator does not persist TeamMatch changes");
        }

        [TestMethod]
        public void TestGetMatchesAll()
        {
            ClearAllData();
            var playerLogic = (IPlayerManipulations)getInterfaceImplementation(typeof(IPlayerManipulations));
            var gameLogic = (IGameManipulations)getInterfaceImplementation(typeof(IGameManipulations));
            var matchLogic = (IMatchManipulations)getInterfaceImplementation(typeof(IMatchManipulations));

            PlayerType player1 = new PlayerType() { Name = "test1", Mail = "test1", Tag = "test1" };
            PlayerType player2 = new PlayerType() { Name = "test2", Mail = "test2", Tag = "test2" };
            PlayerType player3 = new PlayerType() { Name = "test3", Mail = "test3", Tag = "test3" };
            PlayerType player4 = new PlayerType() { Name = "test4", Mail = "test4", Tag = "test4" };
            PlayerType player5 = new PlayerType() { Name = "test5", Mail = "test5", Tag = "test5" };
            PlayerType player6 = new PlayerType() { Name = "test6", Mail = "test6", Tag = "test6" };
            playerLogic.AddOrUpdatePlayer(player1);
            playerLogic.AddOrUpdatePlayer(player2);
            playerLogic.AddOrUpdatePlayer(player3);
            playerLogic.AddOrUpdatePlayer(player4);
            playerLogic.AddOrUpdatePlayer(player5);
            playerLogic.AddOrUpdatePlayer(player6);
            var game1 = new GameType() { Name = "game1", ParticipantType = ParticipantTypes.Solo };
            var game2 = new GameType() { Name = "game2", ParticipantType = ParticipantTypes.All };
            gameLogic.AddOrUpdateGame(game1);
            gameLogic.AddOrUpdateGame(game2);

            var team1 = new TeamType() { Name = "team1", Members = new List<PlayerType>() { player1, player2 } };
            var team2 = new TeamType() { Name = "team2", Members = new List<PlayerType>() { player3, player4 } };
            var team3 = new TeamType() { Name = "team3", Members = new List<PlayerType>() { player5, player6 } };

            var match1 = new SoloMatch()
            {
                GameID = game1,
                Category = MatchCategories.Competition,
                dateTime = DateTime.Now,
                Players = new List<PlayerType>() { player1, player2 },
                Scores = new List<int>() { 1, 2 }
            };
            var match2 = new SoloMatch()
            {
                GameID = game2,
                Category = MatchCategories.Training,
                dateTime = DateTime.Now,
                Players = new List<PlayerType>() { player1, player3 },
                Scores = new List<int>() { 1, 2 }
            };
            var match3 = new TeamMatch()
            {
                GameID = game2,
                Category = MatchCategories.Training,
                dateTime = DateTime.Now,
                Teams= new List<TeamType>() { team1, team2 },
                Scores = new List<int>() { 1, 2 }
            };
            matchLogic.AddOrUpdateSoloMatch(match1);
            matchLogic.AddOrUpdateSoloMatch(match2);
            matchLogic.AddOrUpdateTeamMatch(match3);

            var matches = matchLogic.GetMatchesAll(game2);
            Assert.IsTrue(
                ((matches.Count == 2) &&
                 (matches.Contains(match2)) &&
                 (matches.Contains(match3))),
                " - MatchManipulator \"GetMatchesAll\" Not OK");
        }
        
        [TestMethod]
        public void TestGetMatches()
        {
            ClearAllData();
            var playerLogic = (IPlayerManipulations)getInterfaceImplementation(typeof(IPlayerManipulations));
            var gameLogic = (IGameManipulations)getInterfaceImplementation(typeof(IGameManipulations));
            var matchLogic = (IMatchManipulations)getInterfaceImplementation(typeof(IMatchManipulations));

            PlayerType player1 = new PlayerType() { Name = "test1", Mail = "test1", Tag = "test1" };
            PlayerType player2 = new PlayerType() { Name = "test2", Mail = "test2", Tag = "test2" };
            PlayerType player3 = new PlayerType() { Name = "test3", Mail = "test3", Tag = "test3" };
            PlayerType player4 = new PlayerType() { Name = "test4", Mail = "test4", Tag = "test4" };
            PlayerType player5 = new PlayerType() { Name = "test5", Mail = "test5", Tag = "test5" };
            PlayerType player6 = new PlayerType() { Name = "test6", Mail = "test6", Tag = "test6" };
            playerLogic.AddOrUpdatePlayer(player1);
            playerLogic.AddOrUpdatePlayer(player2);
            playerLogic.AddOrUpdatePlayer(player3);
            playerLogic.AddOrUpdatePlayer(player4);
            playerLogic.AddOrUpdatePlayer(player5);
            playerLogic.AddOrUpdatePlayer(player6);
            var game1 = new GameType() { Name = "game1", ParticipantType = ParticipantTypes.Solo };
            var game2 = new GameType() { Name = "game2", ParticipantType = ParticipantTypes.All };
            gameLogic.AddOrUpdateGame(game1);
            gameLogic.AddOrUpdateGame(game2);

            var team1 = new TeamType() { Name = "team1", Members = new List<PlayerType>() { player1, player2 } };
            var team2 = new TeamType() { Name = "team2", Members = new List<PlayerType>() { player3, player4 } };
            var team3 = new TeamType() { Name = "team3", Members = new List<PlayerType>() { player5, player6 } };

            var match1 = new SoloMatch()
            {
                GameID = game1,
                Category = MatchCategories.Training,
                dateTime = DateTime.Now,
                Players = new List<PlayerType>() { player1, player2 },
                Scores = new List<int>() { 1, 2 }
            };
            var match2 = new SoloMatch()
            {
                GameID = game2,
                Category = MatchCategories.Training,
                dateTime = DateTime.Now,
                Players = new List<PlayerType>() { player1, player3 },
                Scores = new List<int>() { 1, 2 }
            };
            var match3 = new TeamMatch()
            {
                GameID = game2,
                Category = MatchCategories.Training,
                dateTime = DateTime.Now,
                Teams = new List<TeamType>() { team1, team2 },
                Scores = new List<int>() { 1, 2 }
            };
            matchLogic.AddOrUpdateSoloMatch(match1);
            matchLogic.AddOrUpdateSoloMatch(match2);
            matchLogic.AddOrUpdateTeamMatch(match3);

            var matches = matchLogic.GetMatches(game2,ParticipantTypes.All, MatchCategories.Training);
            Assert.IsTrue(
                ((matches.Count == 2) &&
                 (matches.Contains(match2)) &&
                 (matches.Contains(match3))),
                " - MatchManipulator \"GetMatches\" Not OK");
            matches = matchLogic.GetMatches(game2, ParticipantTypes.Solo , MatchCategories.Training);
            Assert.IsTrue(
                ((matches.Count == 1) &&
                 (matches.Contains(match2))),
                " - MatchManipulator \"GetMatches\" Not OK");
            matches = matchLogic.GetMatches(game2, ParticipantTypes.Team, MatchCategories.Training);
            Assert.IsTrue(
                ((matches.Count == 1) &&
                 (matches.Contains(match3))),
                " - MatchManipulator \"GetMatches\" Not OK");
            matches = matchLogic.GetMatches(game2, ParticipantTypes.All, MatchCategories.Competition);
            Assert.IsTrue((matches.Count == 0), " - MatchManipulator \"GetMatches\" Not OK");
        }

        #endregion Match Manipulations

        #region Rankings

        [TestMethod]
        public void testSoloTrainingGamePoints()
        {
            ClearAllData();
            var playerLogic = (IPlayerManipulations)getInterfaceImplementation(typeof(IPlayerManipulations));
            var teamLogic = (ITeamManipulations)getInterfaceImplementation(typeof(ITeamManipulations));
            var gameLogic = (IGameManipulations)getInterfaceImplementation(typeof(IGameManipulations));
            var matchLogic = (IMatchManipulations)getInterfaceImplementation(typeof(IMatchManipulations));
            var rankingLogic = (IRankingSource)getInterfaceImplementation(typeof(IRankingSource));

            PlayerType player1 = new PlayerType() { Name = "test1", Mail = "test1", Tag = "test1" };
            PlayerType player2 = new PlayerType() { Name = "test2", Mail = "test2", Tag = "test2" };
            PlayerType player3 = new PlayerType() { Name = "test3", Mail = "test3", Tag = "test3" };
            PlayerType player4 = new PlayerType() { Name = "test4", Mail = "test4", Tag = "test4" };
            PlayerType player5 = new PlayerType() { Name = "test5", Mail = "test5", Tag = "test5" };
            PlayerType player6 = new PlayerType() { Name = "test6", Mail = "test6", Tag = "test6" };
            playerLogic.AddOrUpdatePlayer(player1);
            playerLogic.AddOrUpdatePlayer(player2);
            playerLogic.AddOrUpdatePlayer(player3);
            playerLogic.AddOrUpdatePlayer(player4);
            playerLogic.AddOrUpdatePlayer(player5);
            playerLogic.AddOrUpdatePlayer(player6);
            var game1 = new GameType() { Name = "game1", ParticipantType = ParticipantTypes.All };
            gameLogic.AddOrUpdateGame(game1);
           
            var match1 = new SoloMatch()
            {
                GameID = game1,
                Category = MatchCategories.Training,
                dateTime = DateTime.Now,
                Players = new List<PlayerType>() { player1, player2, player3 },
                Scores = new List<int>() { 1,3,2 }
            };
            matchLogic.AddOrUpdateSoloMatch(match1);
            var rankings = rankingLogic.GetGameRankingsAll(game1, ParticipantTypes.All);
            Assert.IsTrue(rankings.Count == 3, $" - getGameRankingsAll, expected count = {3}, was: {rankings.Count}");
            var rank = rankings.Where(r => r.Player == player1).FirstOrDefault();
            Assert.IsNotNull(rank, $" - getGameRankingsAll does not return correct players in rankings");
            Assert.IsTrue(rank.Points == 0, $" - getGameRankingsAll, points: expected 0, was {rank.Points}");
            Assert.IsTrue(rank.Ranking == Ranks.Unranked, $" - getGameRankingsAll, \"unranked\" expected,  as \"{rank.Ranking}\"");
            rank = rankings.Where(r => r.Player == player2).FirstOrDefault();
            Assert.IsNotNull(rank, $" - getGameRankingsAll does not return correct players in rankings");
            Assert.IsTrue(rank.Points == 4, $" - getGameRankingsAll, points: expected 4, was {rank.Points}");
            Assert.IsTrue(rank.Ranking == Ranks.Unranked, $" - getGameRankingsAll, \"unranked\" expected,  as \"{rank.Ranking}\"");
            rank = rankings.Where(r => r.Player == player3).FirstOrDefault();
            Assert.IsNotNull(rank, $" - getGameRankingsAll does not return correct players in rankings");
            Assert.IsTrue(rank.Points == 2, $" - getGameRankingsAll, points: expected 2, was {rank.Points}");
            Assert.IsTrue(rank.Ranking == Ranks.Unranked, $" - getGameRankingsAll, \"unranked\" expected,  as \"{rank.Ranking}\"");
        }

        [TestMethod]
        public void testSoloCompetitionGamePoints()
        {
            ClearAllData();
            var playerLogic = (IPlayerManipulations)getInterfaceImplementation(typeof(IPlayerManipulations));
            var teamLogic = (ITeamManipulations)getInterfaceImplementation(typeof(ITeamManipulations));
            var gameLogic = (IGameManipulations)getInterfaceImplementation(typeof(IGameManipulations));
            var matchLogic = (IMatchManipulations)getInterfaceImplementation(typeof(IMatchManipulations));
            var rankingLogic = (IRankingSource)getInterfaceImplementation(typeof(IRankingSource));

            PlayerType player1 = new PlayerType() { Name = "test1", Mail = "test1", Tag = "test1" };
            PlayerType player2 = new PlayerType() { Name = "test2", Mail = "test2", Tag = "test2" };
            PlayerType player3 = new PlayerType() { Name = "test3", Mail = "test3", Tag = "test3" };
            PlayerType player4 = new PlayerType() { Name = "test4", Mail = "test4", Tag = "test4" };
            PlayerType player5 = new PlayerType() { Name = "test5", Mail = "test5", Tag = "test5" };
            PlayerType player6 = new PlayerType() { Name = "test6", Mail = "test6", Tag = "test6" };
            playerLogic.AddOrUpdatePlayer(player1);
            playerLogic.AddOrUpdatePlayer(player2);
            playerLogic.AddOrUpdatePlayer(player3);
            playerLogic.AddOrUpdatePlayer(player4);
            playerLogic.AddOrUpdatePlayer(player5);
            playerLogic.AddOrUpdatePlayer(player6);
            var game1 = new GameType() { Name = "game1", ParticipantType = ParticipantTypes.All };
            gameLogic.AddOrUpdateGame(game1);

            var match1 = new SoloMatch()
            {
                GameID = game1,
                Category = MatchCategories.Competition,
                dateTime = DateTime.Now,
                Players = new List<PlayerType>() { player1, player2, player3 },
                Scores = new List<int>() { 1, 3, 2 }
            };
            matchLogic.AddOrUpdateSoloMatch(match1);
            var rankings = rankingLogic.GetGameRankingsAll(game1, ParticipantTypes.All);
            Assert.IsTrue(rankings.Count == 3, $" - getGameRankingsAll, expected count = {3}, was: {rankings.Count}");
            var rank = rankings.Where(r => r.Player == player1).FirstOrDefault();
            Assert.IsNotNull(rank, $" - getGameRankingsAll does not return correct players in rankings");
            Assert.IsTrue(rank.Points == 0, $" - getGameRankingsAll, points: expected 0, was {rank.Points}");
            Assert.IsTrue(rank.Ranking == Ranks.Unranked, $" - getGameRankingsAll, \"unranked\" expected,  as \"{rank.Ranking}\"");
            rank = rankings.Where(r => r.Player == player2).FirstOrDefault();
            Assert.IsNotNull(rank, $" - getGameRankingsAll does not return correct players in rankings");
            Assert.IsTrue(rank.Points == 8, $" - getGameRankingsAll, points: expected 8, was {rank.Points}");
            Assert.IsTrue(rank.Ranking == Ranks.Unranked, $" - getGameRankingsAll, \"unranked\" expected,  as \"{rank.Ranking}\"");
            rank = rankings.Where(r => r.Player == player3).FirstOrDefault();
            Assert.IsNotNull(rank, $" - getGameRankingsAll does not return correct players in rankings");
            Assert.IsTrue(rank.Points == 4, $" - getGameRankingsAll, points: expected 4, was {rank.Points}");
            Assert.IsTrue(rank.Ranking == Ranks.Unranked, $" - getGameRankingsAll, \"unranked\" expected,  as \"{rank.Ranking}\"");
        }

        [TestMethod]
        public void testSoloTournamentGamePoints()
        {
            ClearAllData();
            var playerLogic = (IPlayerManipulations)getInterfaceImplementation(typeof(IPlayerManipulations));
            var teamLogic = (ITeamManipulations)getInterfaceImplementation(typeof(ITeamManipulations));
            var gameLogic = (IGameManipulations)getInterfaceImplementation(typeof(IGameManipulations));
            var matchLogic = (IMatchManipulations)getInterfaceImplementation(typeof(IMatchManipulations));
            var rankingLogic = (IRankingSource)getInterfaceImplementation(typeof(IRankingSource));

            PlayerType player1 = new PlayerType() { Name = "test1", Mail = "test1", Tag = "test1" };
            PlayerType player2 = new PlayerType() { Name = "test2", Mail = "test2", Tag = "test2" };
            PlayerType player3 = new PlayerType() { Name = "test3", Mail = "test3", Tag = "test3" };
            PlayerType player4 = new PlayerType() { Name = "test4", Mail = "test4", Tag = "test4" };
            PlayerType player5 = new PlayerType() { Name = "test5", Mail = "test5", Tag = "test5" };
            PlayerType player6 = new PlayerType() { Name = "test6", Mail = "test6", Tag = "test6" };
            playerLogic.AddOrUpdatePlayer(player1);
            playerLogic.AddOrUpdatePlayer(player2);
            playerLogic.AddOrUpdatePlayer(player3);
            playerLogic.AddOrUpdatePlayer(player4);
            playerLogic.AddOrUpdatePlayer(player5);
            playerLogic.AddOrUpdatePlayer(player6);
            var game1 = new GameType() { Name = "game1", ParticipantType = ParticipantTypes.All };
            gameLogic.AddOrUpdateGame(game1);

            var match1 = new SoloMatch()
            {
                GameID = game1,
                Category = MatchCategories.Tournament,
                dateTime = DateTime.Now,
                Players = new List<PlayerType>() { player1, player2, player3 },
                Scores = new List<int>() { 1, 3, 2 }
            };
            matchLogic.AddOrUpdateSoloMatch(match1);
            var rankings = rankingLogic.GetGameRankingsAll(game1, ParticipantTypes.All);
            Assert.IsTrue(rankings.Count == 3, $" - getGameRankingsAll, expected count = {3}, was: {rankings.Count}");
            var rank = rankings.Where(r => r.Player == player1).FirstOrDefault();
            Assert.IsNotNull(rank, $" - getGameRankingsAll does not return correct players in rankings");
            Assert.IsTrue(rank.Points == 0, $" - getGameRankingsAll, points: expected 0, was {rank.Points}");
            Assert.IsTrue(rank.Ranking == Ranks.Unranked, $" - getGameRankingsAll, \"unranked\" expected,  as \"{rank.Ranking}\"");
            rank = rankings.Where(r => r.Player == player2).FirstOrDefault();
            Assert.IsNotNull(rank, $" - getGameRankingsAll does not return correct players in rankings");
            Assert.IsTrue(rank.Points == 12, $" - getGameRankingsAll,points: expected 12, was {rank.Points}");
            Assert.IsTrue(rank.Ranking == Ranks.Unranked, $" - getGameRankingsAll, \"unranked\" expected,  as \"{rank.Ranking}\"");
            rank = rankings.Where(r => r.Player == player3).FirstOrDefault();
            Assert.IsNotNull(rank, $" - getGameRankingsAll does not return correct players in rankings");
            Assert.IsTrue(rank.Points == 6, $" - getGameRankingsAll, points: expected 6, was {rank.Points}");
            Assert.IsTrue(rank.Ranking == Ranks.Unranked, $" - getGameRankingsAll, \"unranked\" expected,  as \"{rank.Ranking}\"");
        }

        [TestMethod]
        public void TestTeamTrainingGamePoints()
        {
            ClearAllData();
            var playerLogic = (IPlayerManipulations)getInterfaceImplementation(typeof(IPlayerManipulations));
            var teamLogic = (ITeamManipulations)getInterfaceImplementation(typeof(ITeamManipulations));
            var gameLogic = (IGameManipulations)getInterfaceImplementation(typeof(IGameManipulations));
            var matchLogic = (IMatchManipulations)getInterfaceImplementation(typeof(IMatchManipulations));
            var rankingLogic = (IRankingSource)getInterfaceImplementation(typeof(IRankingSource));

            PlayerType player1 = new PlayerType() { Name = "test1", Mail = "test1", Tag = "test1" };
            PlayerType player2 = new PlayerType() { Name = "test2", Mail = "test2", Tag = "test2" };
            PlayerType player3 = new PlayerType() { Name = "test3", Mail = "test3", Tag = "test3" };
            PlayerType player4 = new PlayerType() { Name = "test4", Mail = "test4", Tag = "test4" };
            PlayerType player5 = new PlayerType() { Name = "test5", Mail = "test5", Tag = "test5" };
            PlayerType player6 = new PlayerType() { Name = "test6", Mail = "test6", Tag = "test6" };
            playerLogic.AddOrUpdatePlayer(player1);
            playerLogic.AddOrUpdatePlayer(player2);
            playerLogic.AddOrUpdatePlayer(player3);
            playerLogic.AddOrUpdatePlayer(player4);
            playerLogic.AddOrUpdatePlayer(player5);
            playerLogic.AddOrUpdatePlayer(player6);
            var game1 = new GameType() { Name = "game1", ParticipantType = ParticipantTypes.All };
            gameLogic.AddOrUpdateGame(game1);

            var team1 = new TeamType() { Name = "team1", Members = new List<PlayerType>() { player1, player2 } };
            var team2 = new TeamType() { Name = "team2", Members = new List<PlayerType>() { player3, player4 } };
            var team3 = new TeamType() { Name = "team3", Members = new List<PlayerType>() { player5, player6 } };

            var match1 = new TeamMatch()
            {
                GameID = game1,
                Category = MatchCategories.Training,
                dateTime = DateTime.Now,
                Teams = new List<TeamType>() { team1, team2,team3 },
                Scores = new List<int>() { 1,3,2 }
            };
            matchLogic.AddOrUpdateTeamMatch(match1);
            var rankings = rankingLogic.GetGameRankingsAll(game1, ParticipantTypes.All);
            Assert.IsTrue(rankings.Count == 6, $" - getGameRankingsAll, expected count = {4}, was: {rankings.Count}");
            foreach (var player in team1.Members)
            {
                var rank = rankings.Where(r => r.Player == player).FirstOrDefault();
                Assert.IsNotNull(rank, $" - getGameRankingsAll does not return correct players in rankings");
                Assert.IsTrue(rank.Points == 0, $" - getGameRankingsAll, points: expected 0, was {rank.Points}");
                Assert.IsTrue(rank.Ranking == Ranks.Unranked, $" - getGameRankingsAll, \"unranked\" expected,  as \"{rank.Ranking}\"");
            }
            foreach (var player in team2.Members)
            {
                var rank = rankings.Where(r => r.Player == player).FirstOrDefault();
                Assert.IsNotNull(rank, $" - getGameRankingsAll does not return correct players in rankings");
                Assert.IsTrue(rank.Points == 4, $" - getGameRankingsAll, points: expected 4, was {rank.Points}");
                Assert.IsTrue(rank.Ranking == Ranks.Unranked, $" - getGameRankingsAll, \"unranked\" expected,  as \"{rank.Ranking}\"");
            }
            foreach (var player in team3.Members)
            {
                var rank = rankings.Where(r => r.Player == player).FirstOrDefault();
                Assert.IsNotNull(rank, $" - getGameRankingsAll does not return correct players in rankings");
                Assert.IsTrue(rank.Points == 2, $" - getGameRankingsAll, points: expected 2, was {rank.Points}");
                Assert.IsTrue(rank.Ranking == Ranks.Unranked, $" - getGameRankingsAll, \"unranked\" expected,  as \"{rank.Ranking}\"");
            }
        }

        [TestMethod]
        public void TestTeamCompetitionGamePoints()
        {
            ClearAllData();
            var playerLogic = (IPlayerManipulations)getInterfaceImplementation(typeof(IPlayerManipulations));
            var teamLogic = (ITeamManipulations)getInterfaceImplementation(typeof(ITeamManipulations));
            var gameLogic = (IGameManipulations)getInterfaceImplementation(typeof(IGameManipulations));
            var matchLogic = (IMatchManipulations)getInterfaceImplementation(typeof(IMatchManipulations));
            var rankingLogic = (IRankingSource)getInterfaceImplementation(typeof(IRankingSource));

            PlayerType player1 = new PlayerType() { Name = "test1", Mail = "test1", Tag = "test1" };
            PlayerType player2 = new PlayerType() { Name = "test2", Mail = "test2", Tag = "test2" };
            PlayerType player3 = new PlayerType() { Name = "test3", Mail = "test3", Tag = "test3" };
            PlayerType player4 = new PlayerType() { Name = "test4", Mail = "test4", Tag = "test4" };
            PlayerType player5 = new PlayerType() { Name = "test5", Mail = "test5", Tag = "test5" };
            PlayerType player6 = new PlayerType() { Name = "test6", Mail = "test6", Tag = "test6" };
            playerLogic.AddOrUpdatePlayer(player1);
            playerLogic.AddOrUpdatePlayer(player2);
            playerLogic.AddOrUpdatePlayer(player3);
            playerLogic.AddOrUpdatePlayer(player4);
            playerLogic.AddOrUpdatePlayer(player5);
            playerLogic.AddOrUpdatePlayer(player6);
            var game1 = new GameType() { Name = "game1", ParticipantType = ParticipantTypes.All };
            gameLogic.AddOrUpdateGame(game1);

            var team1 = new TeamType() { Name = "team1", Members = new List<PlayerType>() { player1, player2 } };
            var team2 = new TeamType() { Name = "team2", Members = new List<PlayerType>() { player3, player4 } };
            var team3 = new TeamType() { Name = "team3", Members = new List<PlayerType>() { player5, player6 } };

            var match1 = new TeamMatch()
            {
                GameID = game1,
                Category = MatchCategories.Competition,
                dateTime = DateTime.Now,
                Teams = new List<TeamType>() { team1, team2, team3 },
                Scores = new List<int>() { 1, 3, 2 }
            };
            matchLogic.AddOrUpdateTeamMatch(match1);
            var rankings = rankingLogic.GetGameRankingsAll(game1, ParticipantTypes.All);
            Assert.IsTrue(rankings.Count == 6, $" - getGameRankingsAll, expected count = {4}, was: {rankings.Count}");
            foreach (var player in team1.Members)
            {
                var rank = rankings.Where(r => r.Player == player).FirstOrDefault();
                Assert.IsNotNull(rank, $" - getGameRankingsAll does not return correct players in rankings");
                Assert.IsTrue(rank.Points == 0, $" - getGameRankingsAll, points: expected 0, was {rank.Points}");
                Assert.IsTrue(rank.Ranking == Ranks.Unranked, $" - getGameRankingsAll, \"unranked\" expected,  as \"{rank.Ranking}\"");
            }
            foreach (var player in team2.Members)
            {
                var rank = rankings.Where(r => r.Player == player).FirstOrDefault();
                Assert.IsNotNull(rank, $" - getGameRankingsAll does not return correct players in rankings");
                Assert.IsTrue(rank.Points == 8, $" - getGameRankingsAll, points: expected 8, was {rank.Points}");
                Assert.IsTrue(rank.Ranking == Ranks.Unranked, $" - getGameRankingsAll, \"unranked\" expected,  as \"{rank.Ranking}\"");
            }
            foreach (var player in team3.Members)
            {
                var rank = rankings.Where(r => r.Player == player).FirstOrDefault();
                Assert.IsNotNull(rank, $" - getGameRankingsAll does not return correct players in rankings");
                Assert.IsTrue(rank.Points == 4, $" - getGameRankingsAll, points: expected 4, was {rank.Points}");
                Assert.IsTrue(rank.Ranking == Ranks.Unranked, $" - getGameRankingsAll, \"unranked\" expected,  as \"{rank.Ranking}\"");
            }
        }


        [TestMethod]
        public void TestTeamTournamentGamePoints()
        {
            ClearAllData();
            var playerLogic = (IPlayerManipulations)getInterfaceImplementation(typeof(IPlayerManipulations));
            var teamLogic = (ITeamManipulations)getInterfaceImplementation(typeof(ITeamManipulations));
            var gameLogic = (IGameManipulations)getInterfaceImplementation(typeof(IGameManipulations));
            var matchLogic = (IMatchManipulations)getInterfaceImplementation(typeof(IMatchManipulations));
            var rankingLogic = (IRankingSource)getInterfaceImplementation(typeof(IRankingSource));

            PlayerType player1 = new PlayerType() { Name = "test1", Mail = "test1", Tag = "test1" };
            PlayerType player2 = new PlayerType() { Name = "test2", Mail = "test2", Tag = "test2" };
            PlayerType player3 = new PlayerType() { Name = "test3", Mail = "test3", Tag = "test3" };
            PlayerType player4 = new PlayerType() { Name = "test4", Mail = "test4", Tag = "test4" };
            PlayerType player5 = new PlayerType() { Name = "test5", Mail = "test5", Tag = "test5" };
            PlayerType player6 = new PlayerType() { Name = "test6", Mail = "test6", Tag = "test6" };
            playerLogic.AddOrUpdatePlayer(player1);
            playerLogic.AddOrUpdatePlayer(player2);
            playerLogic.AddOrUpdatePlayer(player3);
            playerLogic.AddOrUpdatePlayer(player4);
            playerLogic.AddOrUpdatePlayer(player5);
            playerLogic.AddOrUpdatePlayer(player6);
            var game1 = new GameType() { Name = "game1", ParticipantType = ParticipantTypes.All };
            gameLogic.AddOrUpdateGame(game1);

            var team1 = new TeamType() { Name = "team1", Members = new List<PlayerType>() { player1, player2 } };
            var team2 = new TeamType() { Name = "team2", Members = new List<PlayerType>() { player3, player4 } };
            var team3 = new TeamType() { Name = "team3", Members = new List<PlayerType>() { player5, player6 } };

            var match1 = new TeamMatch()
            {
                GameID = game1,
                Category = MatchCategories.Tournament,
                dateTime = DateTime.Now,
                Teams = new List<TeamType>() { team1, team2, team3 },
                Scores = new List<int>() { 1, 3, 2 }
            };
            matchLogic.AddOrUpdateTeamMatch(match1);
            var rankings = rankingLogic.GetGameRankingsAll(game1, ParticipantTypes.All);
            Assert.IsTrue(rankings.Count == 6, $" - getGameRankingsAll, expected count = {4}, was: {rankings.Count}");
            foreach (var player in team1.Members)
            {
                var rank = rankings.Where(r => r.Player == player).FirstOrDefault();
                Assert.IsNotNull(rank, $" - getGameRankingsAll does not return correct players in rankings");
                Assert.IsTrue(rank.Points == 0, $" - getGameRankingsAll, points: expected 0, was {rank.Points}");
                Assert.IsTrue(rank.Ranking == Ranks.Unranked, $" - getGameRankingsAll, \"unranked\" expected,  as \"{rank.Ranking}\"");
            }
            foreach (var player in team2.Members)
            {
                var rank = rankings.Where(r => r.Player == player).FirstOrDefault();
                Assert.IsNotNull(rank, $" - getGameRankingsAll does not return correct players in rankings");
                Assert.IsTrue(rank.Points == 12, $" - getGameRankingsAll, points: expected 12, was {rank.Points}");
                Assert.IsTrue(rank.Ranking == Ranks.Unranked, $" - getGameRankingsAll, \"unranked\" expected,  as \"{rank.Ranking}\"");
            }
            foreach (var player in team3.Members)
            {
                var rank = rankings.Where(r => r.Player == player).FirstOrDefault();
                Assert.IsNotNull(rank, $" - getGameRankingsAll does not return correct players in rankings");
                Assert.IsTrue(rank.Points == 6, $" - getGameRankingsAll, points: expected 6, was {rank.Points}");
                Assert.IsTrue(rank.Ranking == Ranks.Unranked, $" - getGameRankingsAll, \"unranked\" expected,  as \"{rank.Ranking}\"");
            }
        }

        [TestMethod]
        public void testRanking()
        {
            ClearAllData();
            var playerLogic = (IPlayerManipulations)getInterfaceImplementation(typeof(IPlayerManipulations));
            var teamLogic = (ITeamManipulations)getInterfaceImplementation(typeof(ITeamManipulations));
            var gameLogic = (IGameManipulations)getInterfaceImplementation(typeof(IGameManipulations));
            var matchLogic = (IMatchManipulations)getInterfaceImplementation(typeof(IMatchManipulations));
            var rankingLogic = (IRankingSource)getInterfaceImplementation(typeof(IRankingSource));

            var players = new PlayerType[20];
            for (int i = 0; i < players.Length; i++)
            {
                players[i] = new PlayerType() { Name = $"test{i}", Mail = $"test{i}", Tag = $"test{i}" };
                playerLogic.AddOrUpdatePlayer(players[i]);
            }
            
            var game1 = new GameType() { Name = "game1", ParticipantType = ParticipantTypes.All };
            gameLogic.AddOrUpdateGame(game1);


            for (int i = 0; i < 5; i++)
            {
                var scores = new List<int>();
                for (int j = 0; j < players.Length; j++)
                {
                    scores.Add( j + 1);
                }
                var match = new SoloMatch()
                {
                    GameID = game1,
                    Category = MatchCategories.Training,
                    dateTime = DateTime.Now,
                    Players = players.ToList(),
                    Scores = scores
                };
                matchLogic.AddOrUpdateSoloMatch(match);
            }
                       
            var rankings = rankingLogic.GetGameRankingsAll(game1, ParticipantTypes.All);
            Assert.IsTrue(rankings.Count == players.Length, $" - getGameRankingsAll, expected count = {players.Length}, was: {rankings.Count}");

            var rank = rankings.Where(r => r.Player == players[0]).FirstOrDefault();
            Assert.IsNotNull(rank, $" - getGameRankingsAll does not return correct players in rankings");
            Assert.IsTrue(rank.Ranking == Ranks.Novice, $" - getGameRankingsAll, \"Novice\" expected,  was \"{rank.Ranking}\"");

            rank = rankings.Where(r => r.Player == players[5]).FirstOrDefault();
            Assert.IsNotNull(rank, $" - getGameRankingsAll does not return correct players in rankings");
            Assert.IsTrue(rank.Ranking == Ranks.Competent, $" - getGameRankingsAll, \"Competent\" expected, was \"{rank.Ranking}\"");

            rank = rankings.Where(r => r.Player == players[14]).FirstOrDefault();
            Assert.IsNotNull(rank, $" - getGameRankingsAll does not return correct players in rankings");
            Assert.IsTrue(rank.Ranking == Ranks.Advanced, $" - getGameRankingsAll, \"Advanced\" expected, was \"{rank.Ranking}\"");
            rank = rankings.Where(r => r.Player == players[19]).FirstOrDefault();
            Assert.IsNotNull(rank, $" - getGameRankingsAll does not return correct players in rankings");
            Assert.IsTrue(rank.Ranking == Ranks.Elite, $" - getGameRankingsAll, \"Elite\" expected, was \"{rank.Ranking}\"");
        }

        [TestMethod]
        public void testGamePointsRecalculation()
        {
            ClearAllData();
            var playerLogic = (IPlayerManipulations)getInterfaceImplementation(typeof(IPlayerManipulations));
            var teamLogic = (ITeamManipulations)getInterfaceImplementation(typeof(ITeamManipulations));
            var gameLogic = (IGameManipulations)getInterfaceImplementation(typeof(IGameManipulations));
            var matchLogic = (IMatchManipulations)getInterfaceImplementation(typeof(IMatchManipulations));
            var rankingLogic = (IRankingSource)getInterfaceImplementation(typeof(IRankingSource));

            PlayerType player1 = new PlayerType() { Name = "test1", Mail = "test1", Tag = "test1" };
            PlayerType player2 = new PlayerType() { Name = "test2", Mail = "test2", Tag = "test2" };
            PlayerType player3 = new PlayerType() { Name = "test3", Mail = "test3", Tag = "test3" };
            PlayerType player4 = new PlayerType() { Name = "test4", Mail = "test4", Tag = "test4" };
            PlayerType player5 = new PlayerType() { Name = "test5", Mail = "test5", Tag = "test5" };
            PlayerType player6 = new PlayerType() { Name = "test6", Mail = "test6", Tag = "test6" };
            playerLogic.AddOrUpdatePlayer(player1);
            playerLogic.AddOrUpdatePlayer(player2);
            playerLogic.AddOrUpdatePlayer(player3);
            playerLogic.AddOrUpdatePlayer(player4);
            playerLogic.AddOrUpdatePlayer(player5);
            playerLogic.AddOrUpdatePlayer(player6);
            var game1 = new GameType() { Name = "game1", ParticipantType = ParticipantTypes.All };
            gameLogic.AddOrUpdateGame(game1);

            var match1 = new SoloMatch()
            {
                GameID = game1,
                Category = MatchCategories.Training,
                dateTime = DateTime.Now,
                Players = new List<PlayerType>() { player1, player2, player3 },
                Scores = new List<int>() { 1, 3, 2 }
            };
            matchLogic.AddOrUpdateSoloMatch(match1);
            match1.Scores = new List<int>() { 1, 2, 3 };
            matchLogic.AddOrUpdateSoloMatch(match1);

            var rankings = rankingLogic.GetGameRankingsAll(game1, ParticipantTypes.All);

            var rank = rankings.Where(r => r.Player == player1).FirstOrDefault();
            Assert.IsNotNull(rank, $" - getGameRankingsAll does not return correct players in rankings");
            Assert.IsTrue(rank.Points == 0, $" - getGameRankingsAll, recalculated points: expected 0, was {rank.Points}");
            rank = rankings.Where(r => r.Player == player2).FirstOrDefault();
            Assert.IsNotNull(rank, $" - getGameRankingsAll does not return correct players in rankings");
            Assert.IsTrue(rank.Points == 2, $" - getGameRankingsAll, recalculated points: expected 2, was {rank.Points}");
            rank = rankings.Where(r => r.Player == player3).FirstOrDefault();
            Assert.IsNotNull(rank, $" - getGameRankingsAll does not return correct players in rankings");
            Assert.IsTrue(rank.Points == 4, $" - getGameRankingsAll, recalculated points: expected 4, was {rank.Points}");            
        }


        #endregion Rankings
    }
}
