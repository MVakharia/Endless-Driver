using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using AutoDriverLibrary;

public class SaveSystem
{
    public static BinaryFormatter formatter;

    public static string path;

    public static FileStream stream;

    public static string fileName;

    /// <summary>
    /// Sets up the binary formatter as a new object. 
    /// Sets up the file path to be a combination of the persistent data path and the file name, completing a path which will allow for access to the file. 
    /// </summary>
    /// <param name="fileName"></param>
    public static void SetFileLocation (string fileName)
    {
        formatter = new BinaryFormatter();

        path = Path.Combine(Application.persistentDataPath, fileName);

        stream = new FileStream(path, FileMode.Create);
    }

    public class SaveSystemExample
    {
        public int number;
        public string word;
        public SaveSystemExample() { }


    }

    /// <summary>
    /// An example to show you how to script a function that creates new data and saves it. 
    /// </summary>
    public static void NewSaveDataExample ()
    {
        SetFileLocation(fileName);
    }

    /*
    public static void SaveRound1ScoreData (PostGameUIManager UIManager)
    {
        Debug.LogError("This is your first round since deleting your data. Creating a new save file.");

        SetFileLocation(lifetimeScoreDataFileName);

        PlayerStatisticsData data = new PlayerStatisticsData();

        data.SetFirstRoundStats(UIManager);

        formatter.Serialize(stream, data);

        stream.Close();
    }
    */

    /*
    public static void SaveAppendedScoreData (PlayerStatisticsData appendedData)
    {
        SetFileLocation(lifetimeScoreDataFileName);

        formatter.Serialize(stream, appendedData);

        stream.Close();
    }

    private static PlayerStatisticsData NewScoreData ()
    {
        stream = new FileStream(path, FileMode.Create);

        PlayerStatisticsData data = new PlayerStatisticsData();

        formatter.Serialize(stream, data);

        stream.Close();

        return data;
    }

    public static PlayerStatisticsData LoadedScoreData ()
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

    public static void EraseScoreData ()
    {
        path = Path.Combine(Application.persistentDataPath, lifetimeScoreDataFileName);

        if(File.Exists(path))
        {
            File.Delete(path);
        }
    }
    */
}