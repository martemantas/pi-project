using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;

public class FileDataHandler
{
    private string dataDirPath = "";
    private string dataFileName = "";

    public FileDataHandler(string dataDirPath, string dataFileName)
    {
        this.dataDirPath = dataDirPath;
        this.dataFileName = dataFileName;
    }

    public GameData Load(string profileId)
    {
        if (profileId == null)
            return null;

        string fullPath = Path.Combine(dataDirPath, profileId, dataFileName);
        GameData loadedData = null;
        if (File.Exists(fullPath))
        {
            try
            {
                string dataToLoad = "";
                using (FileStream stream = new FileStream(fullPath, FileMode.Open))
                {
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        dataToLoad = reader.ReadToEnd();
                    }
                }

                loadedData = JsonUtility.FromJson<GameData>(dataToLoad);
            }
            catch (Exception e)
            {
                Debug.LogError("Error trying to load from: " + fullPath + "\n" + e);
            }
        }
        return loadedData;
    }

    public Dictionary<string, GameData> LoadAllProfiles()
    {
        Dictionary<string, GameData> profileDictionary = new Dictionary<string, GameData>();

        IEnumerable<DirectoryInfo> dirInfos = new DirectoryInfo(dataDirPath).EnumerateDirectories();
        foreach(DirectoryInfo dirInfo in dirInfos)
        {
            string profileId = dirInfo.Name;

            string fullPath = Path.Combine(dataDirPath, profileId, dataFileName);
            if (!File.Exists(fullPath))
            {
                continue;
            }

            GameData profileData = Load(profileId);
            if(profileData != null)
            {
                profileDictionary.Add(profileId, profileData);
            }
            
        }

        return profileDictionary;
    }

    public void Save(GameData data, string profileId)
    {
        if (profileId == null)
            return;

        string fullPath = Path.Combine(dataDirPath, profileId, dataFileName);
        try{
            Directory.CreateDirectory(Path.GetDirectoryName(fullPath));

            string dataToStore = JsonUtility.ToJson(data, true);

            using (FileStream stream = new FileStream(fullPath, FileMode.Create))
            {
                using (StreamWriter writer = new StreamWriter(stream))
                {
                    writer.Write(dataToStore);
                }
            }
        }
        catch(Exception e)
        {
            Debug.LogError("Error trying to save to: " + fullPath + "\n" + e);
        }
    }

    public string GetMostRecentlyUpdatedProfileId()
    {
        string mostRecent = null;

        Dictionary<string, GameData> profilesGameData = LoadAllProfiles();

        foreach(KeyValuePair<string, GameData> pair in profilesGameData)
        {
            string profileId = pair.Key;
            GameData gameData = pair.Value;

            if (gameData == null)
                continue;

            if(mostRecent == null)
            {
                mostRecent = profileId;
            }
            else
            {
                DateTime mostRecentDateTime = DateTime.FromBinary(profilesGameData[mostRecent].lastUpdated);
                DateTime newDateTime = DateTime.FromBinary(gameData.lastUpdated);

                if(newDateTime > mostRecentDateTime)
                {
                    mostRecent = profileId;
                }
            }

        }
        return mostRecent;
    }
}
