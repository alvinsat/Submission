using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class PlayerSaveSystem : MonoBehaviour
{
    [SerializeField]
    PlayerSaveData playerData;


    const string saveName = "SubmissionGame";

    [Header("Debug")]
    [SerializeField]
    bool isForceSave;
    [SerializeField]
    bool isForceLoad;
    [SerializeField]
    bool isForceReset;


    void Update()
    {
        if (isForceSave)
        {
            SaveGameData();
            isForceSave = false;
        }
        if (isForceLoad)
        {
            LoadGameData();
            isForceLoad = false;
        }
        if (isForceReset) {
            ResetGameData();
            isForceReset = false;
        }
    }



    public int GetCoins() {
        return playerData.coins;
    }

    public void UpgradePlayer(int upgradeCost) {
        playerData.levelPlayer++;
        playerData.coins -= upgradeCost;
        isForceSave = true;
    }

    public void IncrementCoins(int coins) {
        playerData.coins += coins;
    }


    public int GetPlayerLevel()
    {
        Debug.Log("Player level is " + playerData.levelPlayer + 1);
        return playerData.levelPlayer;
    }

    public void SaveGame() {
        isForceSave = true;
    }

    void ParseLoadedData(PlayerSaveData savedData) {
        playerData.ids = savedData.ids;
        playerData.status = savedData.status;
        playerData.levelPlayer = savedData.levelPlayer;
        playerData.coins = savedData.coins;
    }


    public void SaveGameData()
    {
        BinaryFormatter bin = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + ("/" + saveName + ".bin"));
        PlayerSaveData newSave = new PlayerSaveData();
        newSave.ParseSaveData(playerData);
        bin.Serialize(file, newSave);
        file.Close();
        Debug.Log("Player Data saved successfully");
    }

    void LoadGameData()
    {
        if (File.Exists(Application.persistentDataPath + ("/" + saveName + ".bin")))
        {
            BinaryFormatter bin = new BinaryFormatter();
            //access
            FileStream file = File.OpenRead(Application.persistentDataPath + ("/" + saveName + ".bin"));
            PlayerSaveData loaded = (PlayerSaveData)bin.Deserialize(file);
            file.Close();
            ParseLoadedData(loaded);
            Debug.Log("Player Data loaded successfully");
        }
        else { 
            Debug.Log("Player Data not found");
        }
    }

    void ResetGameData()
    {
        if (File.Exists(Application.persistentDataPath + ("/" + saveName + ".bin")))
        {
            File.Delete(Application.persistentDataPath + ("/" + saveName + ".bin"));
            Debug.Log("File deleted next time boot up will be a new data");
        }
    }
}

[System.Serializable]
public class PlayerSaveData {
    public STRId ids;
    public STRStatus status;
    public int levelPlayer;
    public int playerUpgradeProgress;
    public int coins;

    public void ParseSaveData(PlayerSaveData saveData)
    {
        ids = saveData.ids;
        status = saveData.status;
        levelPlayer = saveData.levelPlayer;
        coins = saveData.coins;
    }

}