using Opgave02;
using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Opgave02.Tests
{
    [TestClass()]
    public class UnitTest1
    {
        [TestMethod()]
        public void ParticipantTest()
        {
            // Arrange
            string name = "John";
            List<int> list = new List<int> { 1, 2 };
            Participant p = new Participant(name, list);

            // Act
            string name2 = p.Name;
            List<int> list2 = p.Preferences;

            // Assert
            Assert.AreEqual(name, name2);
            Assert.AreEqual(list, list2);

        }

        [TestMethod()]
        public void SessionTest()
        {
            // Arrange
            string name = "session";
            Session s = new Session(name, 3);

            // Act
            string name2 = s.Name;
            int max = s.MaxParticipants;
            
            // Assert
            Assert.AreEqual(name, name2);
            Assert.AreEqual(max, 3);

        }

        [TestMethod()]
        public void CreateShedulerTest()
        {
            // Arrange
            Scheduler sched = new Scheduler();

            // Act
            var s = sched.Sessions;
            var p = sched.Participants;
            var dict = sched.SessionPersonSchedule;
            // Assert
            Assert.IsNotNull(s);
            if (s != null) Assert.AreEqual(s.Count, 0);
            Assert.IsNotNull(p);
            if (p != null) Assert.AreEqual(p.Count, 0);
            Assert.IsNotNull(dict);
            if (dict != null) Assert.AreEqual(dict.Count, 0);
        }

        [TestMethod()]
        public void CreateShedulerSessionTest()
        {
            // Arrange
            Scheduler sched = new Scheduler();
            var list = new List<Session>
            {
                new Session("Session-1",3),
                new Session("Session-2",4),
                new Session("Session-3",4),
                new Session("Session-4",4)
            };

            sched.Sessions = list;
            // Act
            var list2 = sched.Sessions;
            // Assert
            Assert.AreEqual(list, list2);
        }
        [TestMethod()]
        public void CreateShedulerParticipantsTest()
        {
            // Arrange
            Scheduler sched = new Scheduler();
            var list = new List<Participant>
            {
                new Participant("person-A",new List<int> {4,2,3 }),
                new Participant("person-B",new List<int> {1,4,2 }),
                new Participant("person-C",new List<int> {1,2,4 }),
            };

            sched.Participants = list;
            // Act
            var list2 = sched.Participants;
            // Assert
            Assert.AreEqual(list, list2);
        }

        [TestMethod()]
        public void ShedulerTest1()
        {
            // impossible schedule

            // Arrange
            Scheduler sched = new Scheduler();
            sched.Sessions = new List<Session>
            {
                new Session("Session-1",3),
                new Session("Session-2",4),
                new Session("Session-3",4),
                new Session("Session-4",4)
            };
            sched.Participants = new List<Participant>
            {
                new Participant("person-A",new List<int> {1,2}),
                new Participant("person-B",new List<int> {1,2}),
                new Participant("person-C",new List<int> {1,2}),
                new Participant("person-D",new List<int> {1,2}),
                new Participant("person-E",new List<int> {1,2}),
                new Participant("person-F",new List<int> {1,2}),
                new Participant("person-G",new List<int> {1,2}),
                new Participant("person-H",new List<int> {1,2}),
                new Participant("person-I",new List<int> {1,2}),
                new Participant("person-J",new List<int> {1,2})
            };

            // Act
            var success = sched.CalculateSchedule(); ;
            // Assert
            Assert.IsFalse(success);
            Assert.AreEqual(0, sched.SessionPersonSchedule.Count);
        }

        [TestMethod()]
        public void ShedulerTest2()
        {
            // test possible schedule
            
            // Arrange
            Scheduler sched = new Scheduler();
            sched.Sessions = new List<Session>
            {
                new Session("Session-1",3),
                new Session("Session-2",4),
                new Session("Session-3",4),
                new Session("Session-4",4)
            };
            sched.Participants = new List<Participant>
            {
                new Participant("person-A",new List<int> {4,2,3 }),
                new Participant("person-B",new List<int> {1,4,2 }),
                new Participant("person-C",new List<int> {1,2,4 }),
                new Participant("person-D",new List<int> {2,4,3 }),
                new Participant("person-E",new List<int> {3,2,1 }),
                new Participant("person-F",new List<int> {2,4,3 }),
                new Participant("person-G",new List<int> {2,1,3 }),
                new Participant("person-H",new List<int> {4,2,3 }),
                new Participant("person-I",new List<int> {1,3,4 }),
                new Participant("person-J",new List<int> {1,4,2})
            };

            // Act
            var success = sched.CalculateSchedule(); ;
            // Assert
            Assert.IsTrue(success);
            if (success)
            {
                // check schedule for participants
                bool scheduleOK = true;
                foreach (var item in sched.SessionPersonSchedule)
                {
                    int sessionNr = sched.Sessions.IndexOf(item.Key) + 1;
                    foreach (var person in item.Value)
                    {
                        if (!person.Preferences.Contains(sessionNr))
                        {
                            scheduleOK = false;
                            Assert.Fail($"{person.Name} is in session {item.Key.Name}, which was not in the preferences.");
                            break;
                        }
                    }
                }
                if (scheduleOK)
                {
                    // check for overbookings
                    foreach (var item in sched.SessionPersonSchedule)
                    {
                        if (item.Key.MaxParticipants < item.Value.Count)
                        {
                            scheduleOK = false;
                            Assert.Fail($"Session {item.Key.Name} is overbooked: {item.Value.Count} participants, max was {item.Key.MaxParticipants}.");

                            break;
                        }
                    }
                }
            }
        }

        [TestMethod()]
        public void ShedulerTest3()
        {
            // test if all sessions are in schedule

            // Arrange
            Scheduler sched = new Scheduler();
            sched.Sessions = new List<Session>
            {
                new Session("Session-1",3),
                new Session("Session-2",4),
                new Session("Session-3",4),
                new Session("Session-4",4)
            };
            sched.Participants = new List<Participant>
            {
                new Participant("person-A",new List<int> {1,2}),
                new Participant("person-B",new List<int> {1,2}),
                new Participant("person-C",new List<int> {1,2}),              
            };

            // Act
            var success = sched.CalculateSchedule(); ;
            // Assert
            foreach (var item in sched.Sessions)
            {
                if (!sched.SessionPersonSchedule.ContainsKey(item))
                {
                    Assert.Fail($"Session {item.Name} is not present in the schedule.");
                }
            }            
        }
    }
}
