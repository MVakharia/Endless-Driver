using System;
using UnityEngine;
using TMPro;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace AutoDriverLibrary
{
    [Serializable]
    public class PlayerStatisticsData
    {
        public int score;
        public float distance;
        public int damageTaken;
        public int carsDestroyed;
        public int damageRepaired;
        public int repairPacksCollected;

        public PlayerStatisticsData() { }

        public void SetFirstRoundStats(PostGameUIManager manager)
        {
            score = manager.ScoreThisRound;
            distance = manager.DistanceTravelledThisRoundInMiles;
            damageTaken = manager.DamageTakenThisRound;
            carsDestroyed = manager.CarsDestroyedThisRound;
            damageRepaired = manager.DamageRepairedThisRound;
            repairPacksCollected = manager.RepairPickupsCollectedThisRound;
        }

        public void AddStats(PostGameUIManager addedStats)
        {
            score += addedStats.ScoreThisRound;
            distance += addedStats.DistanceTravelledThisRoundInMiles;
            damageTaken += addedStats.DamageTakenThisRound;
            carsDestroyed += addedStats.CarsDestroyedThisRound;
            damageRepaired += addedStats.DamageRepairedThisRound;
            repairPacksCollected += addedStats.RepairPickupsCollectedThisRound;
        }
    }

    [Serializable]
    public class PlayerStatisticsBoard
    {
        public TMP_Text scoreTMPText;
        public TMP_Text distanceTravelledTMPText;
        public TMP_Text damageTakenTMPText;
        public TMP_Text carsDestroyedTMPText;
        public TMP_Text damageRepairedTMPText;
        public TMP_Text repairPickupsCollectedTMPText;
    }

    public class AutoDriverSaveSystem : SaveSystem
    {
        public static readonly string lifetimeScoreDataFileName = "lifetimeScoreData.adsav";

        public static void SaveAppendedScoreData(PlayerStatisticsData appendedData)
        {
            SetFileLocation(lifetimeScoreDataFileName);

            formatter.Serialize(stream, appendedData);

            stream.Close();
        }

        private static PlayerStatisticsData NewScoreData()
        {
            stream = new FileStream(path, FileMode.Create);

            PlayerStatisticsData data = new PlayerStatisticsData();

            formatter.Serialize(stream, data);

            stream.Close();

            return data;
        }

        public static PlayerStatisticsData LoadedScoreData()
        {
            path = Path.Combine(Application.persistentDataPath, lifetimeScoreDataFileName);

            formatter = new BinaryFormatter();

            PlayerStatisticsData data = new PlayerStatisticsData();

            if (File.Exists(path))
            {
                stream = new FileStream(path, FileMode.Open);

                data = formatter.Deserialize(stream) as PlayerStatisticsData;
            }
            else
            {
                Debug.LogError("Save file not found at " + path + ". Creating new save file.");

                stream = new FileStream(path, FileMode.Create);

                formatter.Serialize(stream, data);
            }

            stream.Close();

            return data;
        }

        public static void EraseScoreData()
        {
            path = Path.Combine(Application.persistentDataPath, lifetimeScoreDataFileName);

            if (File.Exists(path))
            {
                File.Delete(path);
            }
        }
    }
}
