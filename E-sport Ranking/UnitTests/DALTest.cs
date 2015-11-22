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
    /// <summary>
    /// Summary description for UnitTest1
    /// </summary>
    [TestClass]
    public class DALTest
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

        #region auxiliary methods
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
        private Type GetTypeByName(string typeName)
        {
            var foundClass = (from assembly in AppDomain.CurrentDomain.GetAssemblies()
                              from type in assembly.GetTypes()
                              where type.Name == typeName
                              select type).FirstOrDefault();
            return (Type)foundClass;
            //return Activator.CreateInstance(foundClass);

        }
        private void CheckProperty(Type t, string propName, Type[] propTypes)
        {
            var props = t.GetProperties();
            var prop = props.Where(p => p.Name == propName).FirstOrDefault();
            Assert.IsNotNull(prop, $" - {t.GetType().Name} has no public {propName} property");
            Assert.IsTrue(Array.Exists(propTypes, p => p.Name == prop.PropertyType.Name),
                              $"- {typeof(object).Name}: property {propName} is a {prop.PropertyType.Name}");
        }
        private void CheckMethod(Type t, string methodName, Type[] returnTypes, Type[] parameterTypes)
        {
            var methods = t.GetMethods();
            // check if method exists with right signature
            var method = methods.Where(m =>
            {
                if (m.Name != methodName) return false;
                var parameters = m.GetParameters();
                if ((parameterTypes == null || parameterTypes.Length == 0)) return parameters.Length == 0;
                if (parameters.Length != parameterTypes.Length) return false;
                for (int i = 0; i < parameterTypes.Length; i++)
                {
                    // if (parameters[i].ParameterType != parameterTypes[i])
                    if (!parameters[i].ParameterType.IsAssignableFrom(parameterTypes[i]))
                        return false;
                }
                return true;
            }).FirstOrDefault();
            Assert.IsNotNull(method, $" - {t.GetType().Name} has no public {methodName} method with the right signature");

            // check returnType
            Assert.IsTrue(Array.Exists(returnTypes, r => r.Name == method.ReturnType.Name),
                              $"- {typeof(object).Name}: method {methodName} returns a {method.ReturnType.Name}");
        }
        private void CheckMethodOverridesToString(Type t)
        {
            bool doesOverride = t.GetMethod("ToString").DeclaringType == t;
            Assert.IsTrue(doesOverride, $"{t.FullName} does not override ToString().");
        }

        private void ClearAllData(IGameRankingDataAccess DAL)
        {
            DAL.ClearAllData();
            DAL.SubmitGameListChanges();
            DAL.SubmitPlayerListChanges();
            DAL.SubmitTeamListChanges();
            DAL.SubmitmatchListChanges();
            DAL.SubmitRankingListChanges();
        }
        #endregion auxiliary methods

        [TestMethod]
        public void TestClearAllData()
        {
            var DAL = (IGameRankingDataAccess)getInterfaceImplementation(typeof(IGameRankingDataAccess));
            ClearAllData(DAL);

            Assert.IsTrue(DAL.Games.Count == 0, "DAL property \"Games\" is not empty after calling \"ClearAllData\"");
            GameType game = new GameType() { Name = "test", ParticipantType = ParticipantTypes.Solo };
        }

        [TestMethod]
        public void TestGameSerialisation()
        {
            var DAL = (IGameRankingDataAccess)getInterfaceImplementation(typeof(IGameRankingDataAccess));
            ClearAllData(DAL);
            Assert.IsNotNull(DAL.Games, "DAL property \"Games\" is null after calling \"ClearAllData\"");
            Assert.IsTrue(DAL.Games.Count == 0, "DAL property \"Games\" is not empty after calling \"ClearAllData\"");
            GameType game = new GameType() { Name = "test", ParticipantType = ParticipantTypes.Solo };
            DAL.Games.Add(game);
            Assert.IsTrue(DAL.Games.Count == 1, "Count for DAL property \"Games\" != 1 after adding one game.");
            DAL.SubmitGameListChanges();
            // destroy DAL & force reload from files...
            DAL = null;
            DAL = (IGameRankingDataAccess)getInterfaceImplementation(typeof(IGameRankingDataAccess));
            Assert.IsNotNull(DAL.Games, "DAL property \"Games\" is null after creating a new instance of the DAL.");
            Assert.IsTrue(DAL.Games.Count == 1, "Added game was not persisted.");
            Assert.AreEqual(DAL.Games[0].Name, "test",
                $"Game.Game not OK after serialisation. Expected: {"test"}, was: {DAL.Games[0].Name}");
            Assert.AreEqual(DAL.Games[0].ParticipantType, ParticipantTypes.Solo,
                $"Game.ParticipantType not OK after serialisation. Expected: {ParticipantTypes.Solo}, was: {DAL.Games[0].ParticipantType}");
        }

        [TestMethod]
        public void TestPlayerSerialisation()
        {
            var DAL = (IGameRankingDataAccess)getInterfaceImplementation(typeof(IGameRankingDataAccess));
            ClearAllData(DAL);
            Assert.IsNotNull(DAL.Players, "DAL property \"Players\" is null after calling \"ClearAllData\"");
            Assert.IsTrue(DAL.Players.Count == 0, "DAL property \"Players\" is not empty after calling \"ClearAllData\"");
            PlayerType player = new PlayerType() { Name = "testplayer", Mail = "testplayer@odisee.be", Tag = "Tagname" };
            DAL.Players.Add(player);
            Assert.IsTrue(DAL.Players.Count == 1, "Count for DAL property \"Players\" != 1 after adding one player.");
            DAL.SubmitPlayerListChanges();
            // destroy DAL & force reload from files...
            DAL = null;
            DAL = (IGameRankingDataAccess)getInterfaceImplementation(typeof(IGameRankingDataAccess));
            Assert.IsNotNull(DAL.Players, "DAL property \"Players\" is null after creating a new instance of the DAL.");
            Assert.IsTrue(DAL.Players.Count == 1, "Added player was not persisted.");
            Assert.AreEqual(DAL.Players[0].Name, "testplayer",
                $"Player.Name not OK after serialisation. Expected: {"testplayer"}, was: {DAL.Players[0].Name}");
            Assert.AreEqual(DAL.Players[0].Mail, "testplayer@odisee.be",
                $"Player.Mail not OK after serialisation. Expected: {"testplayer@odisee.be"}, was: {DAL.Players[0].Mail}");
            Assert.AreEqual(DAL.Players[0].Tag, "Tagname",
                $"Player.Tag not OK after serialisation. Expected: {"Tagname"}, was: {DAL.Players[0].Tag}");
        }

        [TestMethod]
        public void TestTeamSerialisation()
        {
            var DAL = (IGameRankingDataAccess)getInterfaceImplementation(typeof(IGameRankingDataAccess));
            ClearAllData(DAL);
            Assert.IsNotNull(DAL.Teams, "DAL property \"teams\" is null after calling \"ClearAllData\"");
            Assert.IsTrue(DAL.Teams.Count == 0, "DAL property \"Teams\" is not empty after calling \"ClearAllData\"");
            PlayerType player1 = new PlayerType() { Name = "test1", Mail = "test1", Tag = "test1" };
            PlayerType player2 = new PlayerType() { Name = "test2", Mail = "test2", Tag = "test2" };
            DAL.Players.Add(player1);
            DAL.Players.Add(player2);
            DAL.Teams.Add(new TeamType()
            {
                Name = "TestTeam",
                Members = new List<PlayerType>() { player1, player2 }
            });
            Assert.IsTrue(DAL.Teams.Count == 1, "Count for DAL property \"Teams\" != 1 after adding one team.");
            DAL.SubmitTeamListChanges();
            // destroy DAL & force reload from files...
            DAL = null;
            DAL = (IGameRankingDataAccess)getInterfaceImplementation(typeof(IGameRankingDataAccess));
            Assert.IsNotNull(DAL.Teams, "DAL property \"Teams\" is null after creating a new instance of the DAL.");
            Assert.IsTrue(DAL.Teams.Count == 1, "Added Team was not persisted.");
            Assert.AreEqual(DAL.Teams[0].Name, "TestTeam",
                $"Team.Name not OK after serialisation. Expected: {"TestTeam"}, was: {DAL.Teams[0].Name}");
            Assert.IsTrue((DAL.Teams[0].Members.Contains(player1)) || (DAL.Teams[0].Members.Contains(player2)),
                "DAL property \"Teams\": not all members were persisted");
            Assert.IsTrue((DAL.Players.Contains(player1)) && (DAL.Players.Contains(player2)),
                "DAL property \"Teams\": loaded teammembers were not added to \"Players\". ");
        }

        [TestMethod]
        public void TestRankingSerialisation()
        {
            var DAL = (IGameRankingDataAccess)getInterfaceImplementation(typeof(IGameRankingDataAccess));
            ClearAllData(DAL);
            Assert.IsNotNull(DAL.RankingList, "DAL property \"RankingList\" is null after calling \"ClearAllData\"");
            Assert.IsTrue(DAL.RankingList.Count == 0, "DAL property \"RankingList\" is not empty after calling \"ClearAllData\"");
            PlayerGameRankingType ranking = new PlayerGameRankingType()
            {
                Game = new GameType() { Name = "testgame", ParticipantType = ParticipantTypes.Team },
                Player = new PlayerType() { Name = "testplayer", Mail = "testplayer@odisee.be", Tag = "Tagname" },
                Points = 123,
                Ranking = Ranks.Competent
            };
            DAL.RankingList.Add(ranking);
            Assert.IsTrue(DAL.RankingList.Count == 1, "Count for DAL property \"RankingList\" != 1 after adding one ranking.");
            DAL.SubmitRankingListChanges();
            // destroy DAL & force reload from files...
            DAL = null;
            DAL = (IGameRankingDataAccess)getInterfaceImplementation(typeof(IGameRankingDataAccess));
            Assert.IsNotNull(DAL.RankingList, "DAL property \"RankingList\" is null after creating a new instance of the DAL.");
            Assert.IsTrue(DAL.RankingList.Count == 1, "Added ranking was not persisted.");
            Assert.AreEqual(DAL.RankingList[0].Game.Name, ranking.Game.Name,
                $"Ranking.Game.Name not OK after serialisation. Expected: {ranking.Game.Name}, was: {DAL.RankingList[0].Game.Name}");
            Assert.AreEqual(DAL.RankingList[0].Player.Name, ranking.Player.Name,
                $"Ranking.Player.Name not OK after serialisation. Expected: {ranking.Player.Name}, was: {DAL.RankingList[0].Player.Name}");
            Assert.AreEqual(DAL.RankingList[0].Points, ranking.Points,
                $"Ranking.Points not OK after serialisation. Expected: {ranking.Points}, was: {DAL.RankingList[0].Points}");
            Assert.AreEqual(DAL.RankingList[0].Ranking, ranking.Ranking,
                $"Ranking.Ranking not OK after serialisation. Expected: {ranking.Ranking}, was: {DAL.RankingList[0].Ranking}");
        }
        
        [TestMethod]
        public void TestMatchSerialisation()
        {
            var DAL = (IGameRankingDataAccess)getInterfaceImplementation(typeof(IGameRankingDataAccess));
            ClearAllData(DAL);
            Assert.IsNotNull(DAL.MatchList, "DAL property \"RankingList\" is null after calling \"ClearAllData\"");
            Assert.IsTrue(DAL.MatchList.Count == 0, "DAL property \"RankingList\" is not empty after calling \"ClearAllData\"");
            PlayerType player1 = new PlayerType() { Name = "test1", Mail = "test1", Tag = "test1" };
            PlayerType player2 = new PlayerType() { Name = "test2", Mail = "test2", Tag = "test2" };
            PlayerType player3 = new PlayerType() { Name = "test3", Mail = "test3", Tag = "test3" };
            PlayerType player4 = new PlayerType() { Name = "test4", Mail = "test4", Tag = "test4" };
            var soloMatch = new SoloMatch()
            {
                GameID = new GameType() { Name = "testSoloGame", ParticipantType = ParticipantTypes.Solo },
                Category = MatchCategories.Competition,
                dateTime = DateTime.Now,
                Players = new List<PlayerType>() { player1, player2 },
                Scores = new List<int>() { 1, 2 }
            };
            DAL.MatchList.Add(soloMatch);
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
            var teamMatch = new TeamMatch()
            {
                GameID = new GameType() { Name = "testTeamGame", ParticipantType = ParticipantTypes.Team },
                Category = MatchCategories.Training,
                dateTime = DateTime.Now,
                Teams = new List<TeamType>() { team1, team2 },
                Scores = new List<int>() { 3, 4 }
            };
            DAL.MatchList.Add(teamMatch);
            Assert.IsTrue(DAL.MatchList.Count == 2, "Count for DAL property \"MatchList\" != 2 after adding two matches.");
            DAL.SubmitmatchListChanges();
            // destroy DAL & force reload from files...
            DAL = null;
            DAL = (IGameRankingDataAccess)getInterfaceImplementation(typeof(IGameRankingDataAccess));
            Assert.IsNotNull(DAL.MatchList, "DAL property \"MatchList\" is null after creating a new instance of the DAL.");
            Assert.IsTrue(DAL.MatchList.Count == 2, "Added matches were not persisted.");
            bool soloMatchFound = false;
            bool teamMatchFound = false;
            for (int i = 0; i < 2; i++)
            {
                if (DAL.MatchList[i] is SoloMatch)
                {
                    soloMatchFound = true;
                    var match = DAL.MatchList[i] as SoloMatch;
                    Assert.AreEqual(match.GameID,soloMatch.GameID,
                        $"SoloMatch.GameID not OK after serialisation. Expected: {soloMatch.GameID}, was: {match.GameID}");
                    Assert.AreEqual(match.Category, soloMatch.Category,
                        $"SoloMatch.Category not OK after serialisation. Expected: {soloMatch.Category}, was: {match.Category}");
                    Assert.AreEqual(match.dateTime, soloMatch.dateTime,
                        $"SoloMatch.dateTime not OK after serialisation. Expected: {soloMatch.dateTime}, was: {match.dateTime}");
                    Assert.AreEqual(match.Players[0], soloMatch.Players[0],
                        $"SoloMatch.Players[0] not OK after serialisation. Expected: {soloMatch.Players[0]}, was: {match.Players[0]}");
                    Assert.AreEqual(match.Scores[1], soloMatch.Scores[1],
                         $"SoloMatch.Scores[1] not OK after serialisation. Expected: {soloMatch.Scores[1]}, was: {match.Scores[1]}");
                }
                else if (DAL.MatchList[i] is TeamMatch)
                {
                    teamMatchFound = true;
                    var match = DAL.MatchList[i] as TeamMatch;
                    Assert.AreEqual(match.GameID, teamMatch.GameID,
                        $"TeamMatch.GameID not OK after serialisation. Expected: {teamMatch.GameID}, was: {match.GameID}");
                    Assert.AreEqual(match.Category, teamMatch.Category,
                        $"TeamMatch.Category not OK after serialisation. Expected: {teamMatch.Category}, was: {match.Category}");
                    Assert.AreEqual(match.dateTime, teamMatch.dateTime,
                        $"TeamMatch.dateTime not OK after serialisation. Expected: {teamMatch.dateTime}, was: {match.dateTime}");
                    Assert.AreEqual(match.Teams[0].Name, teamMatch.Teams[0].Name,
                        $"TeamMatch.Teams[0] not OK after serialisation. Expected: {teamMatch.Teams[0]}, was: {match.Teams[0]}");
                    Assert.AreEqual(match.Scores[1], teamMatch.Scores[1],
                        $"TeamMatch.Scores[1] not OK after serialisation. Expected: {teamMatch.Scores[1]}, was: {match.Scores[1]}");
                }
            }
            Assert.IsTrue(soloMatchFound, "SoloMatch not persisted");
            Assert.IsTrue(teamMatchFound, "TeamMatch not persisted");
        }

    }
}
