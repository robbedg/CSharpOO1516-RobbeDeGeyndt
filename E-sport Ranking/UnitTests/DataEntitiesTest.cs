using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DataEntities;
using DataAccessInterfaces;
using DataAccessImplementation;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Reflection;

namespace UnitTests
{
    [TestClass]
    public class DataEntitiesTest
    {


        [TestMethod]
        public void TestGameType()
        {
            var game1 = new GameType { Name = "game", ParticipantType = ParticipantTypes.Solo };
            var game2 = new GameType { Name = "game", ParticipantType = ParticipantTypes.Solo };
            Assert.AreEqual(game1, game2, "GameType Equals does not return true for cloned game");
            Assert.IsTrue(game1==game2, "GameType '==' does not return true for cloned game");
            game2 = new GameType { Name = "game2", ParticipantType = ParticipantTypes.Solo };
            Assert.AreNotEqual(game1, game2, "GameType Equals returns true for games with different names");
            Assert.IsTrue(game1 != game2, "GameType '!=' does not return true for different games");
        }

        [TestMethod]
        public void TestPlayerType()
        {
            PlayerType player1 = new PlayerType() { Name = "testplayer", Mail = "testplayer@odisee.be", Tag = "Tagname" };
            PlayerType player2 = new PlayerType() { Name = "testplayer", Mail = "testplayer@odisee.be", Tag = "Tagname" };

            Assert.AreEqual(player1, player2, "PlayerType Equals does not return true for cloned player");
            Assert.IsTrue(player1 == player2, "playerType '==' does not return true for cloned game");
            player2 = new PlayerType() { Name = "testplayer2", Mail = "testplayer2@odisee.be", Tag = "Tagname2" };
            Assert.AreNotEqual(player1, player2, "PlayerType Equals returns true for different players");
            Assert.IsTrue(player1 != player2, "PlayerType '!=' does not return true for different players");
        }

        [TestMethod]
        public void TestTeamType()
        {
            PlayerType player1 = new PlayerType() { Name = "testplayer1", Mail = "testplayer1@odisee.be", Tag = "Tagname1" };
            PlayerType player2 = new PlayerType() { Name = "testplayer2", Mail = "testplayer2@odisee.be", Tag = "Tagname2" };
            PlayerType player3 = new PlayerType() { Name = "testplayer3", Mail = "testplayer3@odisee.be", Tag = "Tagname3" };

            TeamType team1 = new TeamType() { Name = "team1", Members = new List<PlayerType>() { player1, player2 } };
            TeamType team2 = new TeamType() { Name = "team1", Members = new List<PlayerType>() { player1, player2 } };
            Assert.AreEqual(team1, team2, " - TeamType Equals does not return true for cloned team");
            Assert.IsTrue(team1 == team2, " - TeamType '==' does not return true for cloned team");
            team2 = new TeamType() { Name = "team2", Members = new List<PlayerType>() { player2, player3 } };
            Assert.AreNotEqual(team1, team2, " - TeamType Equals returns true for different teams");
            Assert.IsTrue(team1 != team2, " - TeamType '!=' does not return true for different teams");
        }

        [TestMethod]
        public void TestSoloMatchType()
        {
            PlayerType player1 = new PlayerType() { Name = "test1", Mail = "test1", Tag = "test1" };
            PlayerType player2 = new PlayerType() { Name = "test2", Mail = "test2", Tag = "test2" };
            
            var soloMatch1 = new SoloMatch()
            {
                GameID = new GameType() { Name = "testSoloGame", ParticipantType = ParticipantTypes.Solo },
                Category = MatchCategories.Competition,
                dateTime = DateTime.Now,
                Players = new List<PlayerType>() { player1, player2 },
                Scores = new List<int>() { 1, 2 }
            };
            PlayerType player3 = new PlayerType() { Name = "test1", Mail = "test1", Tag = "test1" };
            PlayerType player4 = new PlayerType() { Name = "test2", Mail = "test2", Tag = "test2" };

            var soloMatch2 = new SoloMatch()
            {
                GameID = new GameType() { Name = "testSoloGame", ParticipantType = ParticipantTypes.Solo },
                Category = MatchCategories.Competition,
                dateTime = soloMatch1.dateTime,
                Players = new List<PlayerType>() { player3, player4 },
                Scores = new List<int>() { 1, 2 }
            };

            Assert.AreEqual(soloMatch1, soloMatch2, "SoloMatch Equals does not return true for cloned matches");
            Assert.IsTrue(soloMatch1 == soloMatch2, "SoloMatch '==' does not return true for cloned matches");

            player3 = new PlayerType() { Name = "test3", Mail = "test3", Tag = "test3" };
            player4 = new PlayerType() { Name = "test4", Mail = "test4", Tag = "test4" };

            soloMatch2 = new SoloMatch()
            {
                GameID = new GameType() { Name = "testSoloGame2", ParticipantType = ParticipantTypes.Team },
                Category = MatchCategories.Tournament,
                dateTime = DateTime.Now,
                Players = new List<PlayerType>() { player3, player4 },
                Scores = new List<int>() { 3,4 }
            };
            
            Assert.AreNotEqual(soloMatch1, soloMatch2, "SoloMatch Equals returns true for different solomatches");
            Assert.IsTrue(soloMatch1 != soloMatch2, "SoloMatch  '!=' does not return true for different solomatches");
        }

        [TestMethod]
        public void TestTeamMatchType()
        {
            PlayerType player1 = new PlayerType() { Name = "test1", Mail = "test1", Tag = "test1" };
            PlayerType player2 = new PlayerType() { Name = "test2", Mail = "test2", Tag = "test2" };
            PlayerType player3 = new PlayerType() { Name = "test3", Mail = "test3", Tag = "test3" };
            PlayerType player4 = new PlayerType() { Name = "test4", Mail = "test4", Tag = "test4" };

            var team1 = new TeamType()
            {
                Name = "team1",
                Members = new List<PlayerType>() { player1, player2 }
            };
            var team2 = new TeamType()
            {
                Name = "team2",
                Members = new List<PlayerType>() { player3, player4 }
            };

            var teamMatch1 = new TeamMatch()
            {
                GameID = new GameType() { Name = "testTeamGame", ParticipantType = ParticipantTypes.Team },
                Category = MatchCategories.Competition,
                dateTime = DateTime.Now,
                Teams = new List<TeamType>() { team1, team2 },
                Scores = new List<int>() { 1, 2 }
            };
            
            var teamMatch2 = new TeamMatch()
            {
                GameID = new GameType() { Name = "testTeamGame", ParticipantType = ParticipantTypes.Team },
                Category = MatchCategories.Competition,
                dateTime = teamMatch1.dateTime,
                Teams = new List<TeamType>() { team1, team2 },
                Scores = new List<int>() { 1, 2 }
            };

            Assert.AreEqual(teamMatch1, teamMatch2, " - TeamMatch Equals does not return true for cloned matches");
            Assert.IsTrue(teamMatch1 == teamMatch2, " - TeamMatch '==' does not return true for cloned matches");

            player3 = new PlayerType() { Name = "test5", Mail = "test5", Tag = "test5" };
            player4 = new PlayerType() { Name = "test5", Mail = "test6", Tag = "test6" };
            team2 = new TeamType()
            {
                Name = "team3",
                Members = new List<PlayerType>() { player3, player4 }
            };

            teamMatch2 = new TeamMatch()
            {
                GameID = new GameType() { Name = "testTeamGame", ParticipantType = ParticipantTypes.Team },
                Category = MatchCategories.Training,
                dateTime = DateTime.Now,
                Teams = new List<TeamType>() { team1, team2 },
                Scores = new List<int>() { 3, 4 }
            };

            Assert.AreNotEqual(teamMatch1, teamMatch2, " - TeamMatch Equals returns true for different teammatches");
            Assert.IsTrue(teamMatch1 != teamMatch2, " - TeamMatch  '!=' does not return true for different teammatches");
        }
    }
}
