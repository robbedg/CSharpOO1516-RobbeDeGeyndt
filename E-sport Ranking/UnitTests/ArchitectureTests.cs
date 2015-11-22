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
    public class ArchitectureTests
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

        private bool ClassHasDALInterfaceField(Type t)
        {
            FieldInfo field = t.GetFields(
                         BindingFlags.NonPublic |
                         BindingFlags.Instance).Where(f => f.FieldType.Name == "IGameRankingDataAccess").FirstOrDefault();

            return (field != null);
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



        [TestMethod]
        public void TestIfGameManipulatorHasDALInterfaceField()
        {
            var gameLogic = getInterfaceImplementatingClass(typeof(IGameManipulations));
            Assert.IsTrue(ClassHasDALInterfaceField(gameLogic), " - GameManipulator does not link to DAL through IGameRankingDataAccess");
        }


        [TestMethod]
        public void TestIfPlayerManipulatorHasDALInterfaceField()
        {
            var playerLogic = getInterfaceImplementatingClass(typeof(IPlayerManipulations));
            Assert.IsTrue(ClassHasDALInterfaceField(playerLogic), " - PlayerManipulator does not link to DAL through IPlayerRankingDataAccess");
        }

        [TestMethod]
        public void TestIfTeamManipulatorHasDALInterfaceField()
        {
            var teamLogic = getInterfaceImplementatingClass(typeof(ITeamManipulations));
            Assert.IsTrue(ClassHasDALInterfaceField(teamLogic), " - TeamManipulator does not link to DAL through IPlayerRankingDataAccess");
        }

        [TestMethod]
        public void TestIfMatchManipulatorHasDALInterfaceField()
        {
            var matchLogic = getInterfaceImplementatingClass(typeof(IMatchManipulations));
            Assert.IsTrue(ClassHasDALInterfaceField(matchLogic), " - MatchManipulator does not link to DAL through IMatchRankingDataAccess");
        }

        [TestMethod]
        public void TestIfRankingSourceHasDALInterfaceField()
        {
            var rankingLogic = getInterfaceImplementatingClass(typeof(IRankingSource));
            Assert.IsTrue(ClassHasDALInterfaceField(rankingLogic), " - RankingSource does not link to DAL through IGameRankingDataAccess");
        }


        [TestMethod]
        public void TestIfDataPersistedThroughDifferentLogicImplementations()
        {

            ClearAllData();
            var playerLogic = (IPlayerManipulations)getInterfaceImplementation(typeof(IPlayerManipulations));
            var gameLogic = (IGameManipulations)getInterfaceImplementation(typeof(IGameManipulations));
            var matchLogic = (IMatchManipulations)getInterfaceImplementation(typeof(IMatchManipulations));

            PlayerType player1 = new PlayerType() { Name = "test1", Mail = "test1", Tag = "test1" };
            PlayerType player2 = new PlayerType() { Name = "test2", Mail = "test2", Tag = "test2" };
            playerLogic.AddOrUpdatePlayer(player1);
            playerLogic.AddOrUpdatePlayer(player2);

            var game1 = new GameType() { Name = "game1", ParticipantType = ParticipantTypes.All };
            gameLogic.AddOrUpdateGame(game1);
            var match1 = new SoloMatch()
            {
                GameID = game1,
                Category = MatchCategories.Competition,
                dateTime = DateTime.Now,
                Players = new List<PlayerType>() { player1, player2 },
                Scores = new List<int>() { 1, 2 }
            };
           
            matchLogic.AddOrUpdateSoloMatch(match1);

            var matchesForPlayer1 = playerLogic.GetMatchesForPlayer(player1);
            Assert.IsTrue(
                ((matchesForPlayer1.Count == 1) &&
                 (matchesForPlayer1.Contains(match1))),
                " - Logic implementations do not persist to same DAL implementation.");

        }

    }
}
