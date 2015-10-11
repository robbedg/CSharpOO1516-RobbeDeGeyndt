using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Opgave02
{
    class Program
    {
        //Test methode (main)
        static void Main(string[] args)
        {
            Scheduler test = new Scheduler();
            test.Participants = new List<Participant>
            {
                new Participant("persoon-A", new List<int> {4,2,3 }),
                new Participant("persoon-B", new List<int> {1,4,2 }),
                new Participant("persoon-C", new List<int> {1,2,4 })
            };
            test.Sessions = new List<Session>
            {
                new Session("Session-1",3),
                new Session("Session-2",4),
                new Session("Session-3",4),
                new Session("Session-4",4)
            };
            Console.WriteLine(test.SessionPersonSchedule);
            Console.ReadLine();
        }
    }

    //Class session
    public class Session
    {
        //Maximum amount of participants.
        public int MaxParticipants { get; }
        //Name of the session.
        public String Name { get; }
        //Constructor for session.
        public Session(String name, int maxParticipants)
        {
            this.MaxParticipants = maxParticipants;
            this.Name = name;
        }
    }

    public class Participant
    {
        //Name of the participant.
        public String Name { get; }
        //Preferences of the participant.
        public List<int> Preferences { get; }
        //Constructor for participant.
        public Participant(String name, List<int> preferences)
        {
            this.Name = name;
            this.Preferences = preferences;
        }
    }

    //Class scheduler
    public class Scheduler
    {
        public List<Participant> Participants;
        public List<Session> Sessions;
        public Dictionary<Session, List<Participant>> SessionPersonSchedule;
        public Stack participantsToSchedule;
        public int test = 0;

        //Constructor for scheduler
        public Scheduler()
        {
            Participants = new List<Participant> { };
            Sessions = new List<Session> { };
            SessionPersonSchedule = new Dictionary<Session, List<Participant>> { };
            participantsToSchedule = new Stack();
        }

        //Fill the stack wit participants.
        public void fillStack()
        {
            for (int i = 0; i < Participants.Count; i++)
            {
                participantsToSchedule.Push(Participants[i]);
            }
            for (int i = 0; i < Sessions.Count; i++)
            {
                var lp = new List<Participant> { };
                SessionPersonSchedule.Add(Sessions[i], lp);
            }
        }

        //Method for calculating the schedule.
        public bool CalculateSchedule()
        {
            if (test == 0)
            {
                fillStack();
                test++;
            }
            Participant current = (Participant)participantsToSchedule.Pop();
            List<Participant> value;
            int l;
            
            
            foreach (int i in current.Preferences)
            {
                if (SessionPersonSchedule.TryGetValue(Sessions[i-1], out value))
                {
                    l = value.Count();
                }
                else
                {
                    return false;
                }
                if (Sessions[i-1].MaxParticipants > l)
                {
                    if (value != null)
                    {
                        SessionPersonSchedule.Remove(Sessions[i-1]);
                    }
                    value.Add(current);
                    SessionPersonSchedule.Add(Sessions[i-1], value);
                    if (participantsToSchedule.Count == 0)
                    {
                        return true;
                    }
                    else
                    {
                        return (CalculateSchedule());
                    }
                }        
            }
            SessionPersonSchedule.Clear();
            return false;
        }
    }
}
