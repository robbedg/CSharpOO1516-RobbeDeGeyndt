using DataAccessInterfaces;
using DataEntities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessImplementation
{
    public class GameRankingDataAccess : IGameRankingDataAccess
    {
        public List<GameType> Games { get; set; }
        public List<PlayerType> Players { get; set; }
        public List<TeamType> Teams { get; set; }
        public List<MatchType> MatchList { get; set; }
        public List<PlayerGameRankingType> RankingList { get; set; }

        public GameRankingDataAccess()
        {
            var formatter = new BinaryFormatter();

            if (File.Exists("GameList.bin"))
            {
                using (var file = File.OpenRead("GameList.bin"))
                {
                    Games = (List<GameType>)formatter.Deserialize(file);
                }
            }
            else
            {
                Games = new List<GameType>();
            }

            if (File.Exists("PlayerList.bin"))
            {
                using (var file = File.OpenRead("PlayerList.bin"))
                {
                    Players = (List<PlayerType>)formatter.Deserialize(file);
                }
            }
            else
            {
                Players = new List<PlayerType>();
            }

            if (File.Exists("TeamList.bin"))
            {
                using (var file = File.OpenRead("TeamList.bin"))
                {
                    Teams = (List<TeamType>)formatter.Deserialize(file);
                }
            }
            else
            {
                Teams = new List<TeamType>();
            }

            if (File.Exists("MatchList.bin"))
            {
                using (var file = File.OpenRead("MatchList.bin"))
                {
                    MatchList = (List<MatchType>)formatter.Deserialize(file);
                }
            }
            else
            {
                MatchList = new List<MatchType>();
            }

            if (File.Exists("RankingList.bin"))
            {
                using (var file = File.OpenRead("RankingList.bin"))
                {
                    RankingList = (List<PlayerGameRankingType>)formatter.Deserialize(file);
                }
            }
            else
            {
                RankingList = new List<PlayerGameRankingType>();
            }
        }

        public void SubmitGameListChanges()
        {
            var formatter = new BinaryFormatter();
            using (var file = File.OpenWrite("GameList.bin"))
            {
                formatter.Serialize(file, Games);
            }
        }

        public void SubmitPlayerListChanges()
        {
            var formatter = new BinaryFormatter();
            using (var file = File.OpenWrite("PlayerList.bin"))
            {
                formatter.Serialize(file, Players);
            }
        }

        public void SubmitTeamListChanges()
        {
            var formatter = new BinaryFormatter();
            using (var file = File.OpenWrite("TeamList.bin"))
            {
                formatter.Serialize(file, Teams);
            }
        }

        public void SubmitmatchListChanges()
        {
            var formatter = new BinaryFormatter();
            using (var file = File.OpenWrite("MatchList.bin"))
            {
                formatter.Serialize(file, MatchList);
            }
        }

        public void SubmitRankingListChanges()
        {
            var formatter = new BinaryFormatter();
            using (var file = File.OpenWrite("RankingList.bin"))
            {
                formatter.Serialize(file, RankingList);
            }
        }

        public void ClearAllData()
        {
            if (File.Exists("GameList.bin"))
            {
                File.Delete("GameList.bin");
            }
            Games = new List<GameType>();

            if (File.Exists("PlayerList.bin"))
            {
                File.Delete("PlayerList.bin");
            }
            Players = new List<PlayerType>();

            if (File.Exists("TeamList.bin"))
            {
                File.Delete("TeamList.bin");
            }
            Teams = new List<TeamType>();

            if (File.Exists("MatchList.bin"))
            {
                File.Delete("MatchList.bin");
            }
            MatchList = new List<MatchType>();

            if (File.Exists("RankingList.bin"))
            {
                File.Delete("RankingList.bin");
            }
            RankingList = new List<PlayerGameRankingType>();
        }
    }
}
