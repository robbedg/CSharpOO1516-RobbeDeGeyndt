using DataAccessInterfaces;
using DataEntities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DataAccessImplementation
{
    public class GameRankingDataAccess : IGameRankingDataAccess
    {
        private BinaryFormatter formatter;

        public GameRankingDataAccess()
        {
            formatter = new BinaryFormatter();
        }

        //List of games
        List<GameType> _games;
        public List<GameType> Games
        {
            get
            {
                if (_games != null || !File.Exists("GameList.bin"))
                {
                    return _games;
                }
                else {
                    List<GameType> output = null;
                    using (var bestand = File.Open("GameList.bin", FileMode.Open, FileAccess.Read, FileShare.None))
                    {
                        output = (List<GameType>)formatter.Deserialize(bestand);
                        bestand.Dispose();
                    }
                    return output;
                }
            }
            set { _games = value; }
        }

        //List of players
        List<PlayerType> _players;
        public List<PlayerType> Players
        {
            get
            {
                if (_players != null || !File.Exists("PlayerList.bin"))
                {
                    return _players;
                }
                else
                {
                    List<PlayerType> output = null;
                    var formatter = new BinaryFormatter();
                    using (var bestand = File.Open("PlayerList.bin", FileMode.Open, FileAccess.Read, FileShare.None))
                    {
                        output = (List<PlayerType>)formatter.Deserialize(bestand);
                        bestand.Dispose();
                    }
                    return output;
                }
            }
            set { _players = value; }
        }

        //List of teams
        public List<TeamType> _teams;
        public List<TeamType> Teams
        {
            get
            {
                if (_teams != null || !File.Exists("TeamList.bin"))
                {
                    return _teams;
                }
                else
                {
                    List<TeamType> output = null;
                    var formatter = new BinaryFormatter();
                    using (var bestand = File.Open("TeamList.bin", FileMode.Open, FileAccess.Read, FileShare.None))
                    {
                        output = (List<TeamType>)formatter.Deserialize(bestand);
                        bestand.Dispose();
                    }
                    return output;
                }
            }
            set { _teams = value; }
        }

        //List of matches
        List<MatchType> _matchList;
        public List<MatchType> MatchList
        {
            get
            {
                if (_matchList != null || !File.Exists("MatchList.bin"))
                {
                    return _matchList;
                }
                else
                {
                    List<MatchType> output = null;
                    var formatter = new BinaryFormatter();
                    using (var bestand = File.Open("MatchList.bin", FileMode.Open, FileAccess.Read, FileShare.None))
                    {
                        output = (List<MatchType>)formatter.Deserialize(bestand);
                        bestand.Dispose();
                    }
                    return output;
                }
            }
            set { _matchList = value; }
        }

        //List with ranking
        private List<PlayerGameRankingType> _rankingList;
        public List<PlayerGameRankingType> RankingList
        {
            get
            {
                if (_rankingList != null || !File.Exists("RankingList.bin"))
                {
                    return _rankingList;
                }
                else {
                    List<PlayerGameRankingType> output = null;
                    var formatter = new BinaryFormatter();
                    using (var bestand = File.Open("RankingList.bin", FileMode.Open, FileAccess.Read, FileShare.None))
                    {
                        output = (List<PlayerGameRankingType>)formatter.Deserialize(bestand);
                        bestand.Dispose();
                    }
                    return output;
                }
            }
            set { _rankingList = value; }
        }

        //Save new gamelist
        public void SubmitGameListChanges()
        {
            var formatter = new BinaryFormatter();
            using (var file = File.Open("GameList.bin", FileMode.OpenOrCreate, FileAccess.Write, FileShare.None))
            {
                formatter.Serialize(file, Games);
                file.Dispose();
            }
        }

        //Save new playerslist
        public void SubmitPlayerListChanges()
        {
            using (var file = File.Open("PlayerList.bin", FileMode.OpenOrCreate, FileAccess.Write, FileShare.None))
            {
                formatter.Serialize(file, Players);
                file.Dispose();
            }
        }

        //Save new teamlist
        public void SubmitTeamListChanges()
        {
            using (var file = File.Open("TeamList.bin", FileMode.OpenOrCreate, FileAccess.Write, FileShare.None))
            {
                formatter.Serialize(file, Teams);
                file.Dispose();
            }
        }

        //Save new matchlist
        public void SubmitmatchListChanges()
        {
            using (var file = File.Open("MatchList.bin", FileMode.OpenOrCreate, FileAccess.Write, FileShare.None))
            {
                formatter.Serialize(file, MatchList);
                file.Dispose();
            }
        }

        //Save new rankinglist
        public void SubmitRankingListChanges()
        {
            using (var file = File.Open("RankingList.bin", FileMode.OpenOrCreate, FileAccess.Write, FileShare.None))
            {
                formatter.Serialize(file, RankingList);
                file.Dispose();
            }
        }

        //Wipe local data
        public void ClearAllData()
        {
            Games = null;
            Players = null;
            Teams = null;
            MatchList = null;
            RankingList = null;
        }
    }
}
