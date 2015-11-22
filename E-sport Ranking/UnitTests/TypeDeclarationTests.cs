using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
//using DataEntities;
//using DataAccessInterfaces;
//using DataAccessImplementation;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Reflection;

namespace UnitTests
{
    [TestClass]
    public class TypeDeclarationTests
    {
        [ClassInitialize()]
        public static void MyClassInitialize(TestContext testContext)
        {
            // load all assemblies in the bin-directory for auto detection
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
        private object getInterfaceImplementation(Type wantedInterface ) 
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
            var foundClass =  (from assembly in AppDomain.CurrentDomain.GetAssemblies()
                              from type in assembly.GetTypes()
                              where type.Name == typeName
                              select type).FirstOrDefault();
            return (Type)foundClass;
            //return Activator.CreateInstance(foundClass);
            
        }
        private void  CheckProperty(Type t, string propName, Type[] propTypes)
        {
            var props = t.GetProperties();
            var prop = props.Where(p => p.Name == propName).FirstOrDefault();
            Assert.IsNotNull(prop, $" - {t.GetType().Name} has no public {propName} property");
            Assert.IsTrue(Array.Exists(propTypes,p => p.Name == prop.PropertyType.Name), 
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
            Assert.IsNotNull(method, $" - {t.FullName} has no public {methodName} method with the right signature");           
            
            // check returnType
            Assert.IsTrue(Array.Exists(returnTypes,r => r.Name == method.ReturnType.Name),
                              $"- {typeof(object).Name}: method {methodName} returns a {method.ReturnType.Name}");
        }
        private void CheckMethodOverridesToString(Type t)
        {
            bool doesOverride = t.GetMethod("ToString").DeclaringType == t;
            Assert.IsTrue(doesOverride,$"{t.FullName} does not override ToString()." );
        }
        #endregion auxiliary methods

        #region Data Entities

        [TestMethod]
        public void TestIfGameTypeExists()
        {
            Type x = GetTypeByName("GameType");
            Type participantsType = GetTypeByName("ParticipantTypes");
            Assert.IsNotNull(x,"GameType not declared.");
            // check properties
            CheckProperty(x, "Name", new Type[] {typeof (String), typeof(string)});
            CheckProperty(x, "ParticipantType", new Type[] { participantsType });
            CheckMethod(x, "Equals", new Type[] { typeof(bool),typeof(Boolean)},new Type[] {x});
            CheckMethod(x,"op_Equality",new Type[] { typeof(bool), typeof(Boolean) }, new Type[] { x, x });
            CheckMethodOverridesToString(x);
        }

        [TestMethod]
        public void TestIfPlayerTypeExists()
        {
            Type x = GetTypeByName("PlayerType");
            Assert.IsNotNull(x, "PlayerType not declared.");
            // check properties
            CheckProperty(x, "Name", new Type[] { typeof(String), typeof(string) });
            CheckProperty(x, "Mail", new Type[] { typeof(String), typeof(string) });
            CheckProperty(x, "Tag", new Type[] { typeof(String), typeof(string) });
            CheckMethod(x, "Equals", new Type[] { typeof(bool), typeof(Boolean) }, new Type[] { x });
            CheckMethod(x, "op_Equality", new Type[] { typeof(bool), typeof(Boolean) }, new Type[] { x, x });
            CheckMethodOverridesToString(x);
        }

        [TestMethod]
        public void TestIfPlayerGameRankingTypeExists()
        {
            Type x = GetTypeByName("PlayerGameRankingType");
            Type playerType = GetTypeByName("PlayerType");
            Type gameType = GetTypeByName("GameType");
            Type rankingType = GetTypeByName("Ranks");
            Assert.IsNotNull(x, "PlayerGameRankingType not declared.");
            // check properties
            CheckProperty(x, "Game", new Type[] { gameType });
            CheckProperty(x, "Player", new Type[] { playerType });
            CheckProperty(x, "Points", new Type[] { typeof(int), typeof(Int32) });
            CheckProperty(x, "Ranking", new Type[] { rankingType });
            CheckMethod(x, "Equals", new Type[] { typeof(bool), typeof(Boolean) }, new Type[] { x });
            CheckMethod(x, "CompareTo", new Type[] { typeof(int), typeof(Int32) }, new Type[] { x });
            CheckMethod(x, "op_Equality", new Type[] { typeof(bool), typeof(Boolean) }, new Type[] { x, x });
            CheckMethodOverridesToString(x);
        }

        [TestMethod]
        public void TestIfTeamTypeExists()
        {
            Type x = GetTypeByName("TeamType");
            Type playerType = GetTypeByName("PlayerType");
            Assert.IsNotNull(x, "TeamType not declared.");
            // check properties
            CheckProperty(x, "Name", new Type[] { typeof(String), typeof(string) });
            CheckProperty(x, "Members", new Type[] { typeof(List<>) });
            CheckMethod(x, "Equals", new Type[] { typeof(bool), typeof(Boolean) }, new Type[] { x });
            CheckMethod(x, "op_Equality", new Type[] { typeof(bool), typeof(Boolean) }, new Type[] { x, x });
            CheckMethodOverridesToString(x);
        }

        [TestMethod]
        public void TestIfMatchTypeExists()
        {
            Type x = GetTypeByName("MatchType");
            Type gameType = GetTypeByName("GameType");
            Type  matchCatergories = GetTypeByName("MatchCategories");
            Assert.IsNotNull(x, "MatchType not declared.");
            // check properties
            CheckProperty(x, "dateTime", new Type[] { typeof(DateTime) });
            CheckProperty(x, "GameID", new Type[] { gameType });
            CheckProperty(x, "Category", new Type[] { matchCatergories });

            Assert.IsTrue(x.IsAbstract,$"{x.FullName} should be abstract but is not.");
        }

        [TestMethod]
        public void TestIfSoloMatchTypeExists()
        {
            Type x = GetTypeByName("SoloMatch");
            Type matchType = GetTypeByName("MatchType");
            Assert.IsNotNull(x, $"{x.FullName} not declared.");
            // check properties
            CheckProperty(x, "Players", new Type[] { typeof(List<>) });
            CheckProperty(x, "Scores", new Type[] { typeof(List<>) });            
            Assert.IsTrue(x.IsSubclassOf(matchType), $"{x.FullName} is not a subclass of {matchType.FullName}.");
            CheckMethod(x, "Equals", new Type[] { typeof(bool), typeof(Boolean) }, new Type[] { x });
            CheckMethod(x, "op_Equality", new Type[] { typeof(bool), typeof(Boolean) }, new Type[] { x, x });
            CheckMethodOverridesToString(x);
        }

        [TestMethod]
        public void TestIfTeamMatchTypeExists()
        {
            Type x = GetTypeByName("TeamMatch");
            Type matchType = GetTypeByName("MatchType");
            Assert.IsNotNull(x, $"{x.FullName} not declared.");
            // check properties
            CheckProperty(x, "Teams", new Type[] { typeof(List<>) });
            CheckProperty(x, "Scores", new Type[] { typeof(List<>) });
            Assert.IsTrue(x.IsSubclassOf(matchType), $"{x.FullName} is not a subclass of {matchType.FullName}.");
            CheckMethod(x, "Equals", new Type[] { typeof(bool), typeof(Boolean) }, new Type[] { x });
            CheckMethod(x, "op_Equality", new Type[] { typeof(bool), typeof(Boolean) }, new Type[] { x, x });
            CheckMethodOverridesToString(x);
        }
        
        #endregion Data entities

        #region Test Interface Implementations


        [TestMethod]
        public void TestIfDataAccessLayerImplemented()
        {
            var x = GetTypeByName("IGameRankingDataAccess");
            var y = getInterfaceImplementation(x);
            Assert.IsNotNull(x, " - IGameRankingDataAccess is not implemented");
        }

        [TestMethod]
        public void TestIfIGameManipulationsImplemented()
        {
            var x = GetTypeByName("IGameManipulations");
            var y = getInterfaceImplementation(x);
            Assert.IsNotNull(x, " - IGameManipulations is not implemented");
        }

        [TestMethod]
        public void TestIfIMatchManipulationsImplemented()
        {
            var x = GetTypeByName("IMatchManipulations");
            var y = getInterfaceImplementation(x);
            Assert.IsNotNull(x, " - IMatchManipulations is not implemented");
        }

        [TestMethod]
        public void TestIfIPlayerManipulationsImplemented()
        {
            var x = GetTypeByName("IPlayerManipulations");
            var y = getInterfaceImplementation(x);
            Assert.IsNotNull(x, " - IPlayerManipulations is not implemented");
        }

        [TestMethod]
        public void TestIfITeamManipulationsImplemented()
        {
            var x = GetTypeByName("ITeamManipulations");
            var y = getInterfaceImplementation(x);
            Assert.IsNotNull(x, " - ITeamManipulations is not implemented");
        }

        [TestMethod]
        public void TestIfIRankingSourceImplemented()
        {
            var x = GetTypeByName("IRankingSource");
            var y = getInterfaceImplementation(x);
            Assert.IsNotNull(x, " - IRankingSource is not implemented");
        }

        #endregion Test Interface Implementations




    }
}
