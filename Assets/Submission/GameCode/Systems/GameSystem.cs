using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSystem : MonoBehaviour
{
    public static bool isGameEnd = false;
    public static int sukmaCorrect = 0;

    [SerializeField]
    DataLevelUpInfo playerLevelUpData;

    PlayerSaveSystem saveData;

    [SerializeField]
    AudioSource speaker;
    static AudioSource sSpeaker;

    public int GetTotalMaxLevel() {
        return playerLevelUpData.GetMaxLevel();
    }

    public STRLevelData GetPlayerLevelData(out int levelPlayer) {
        if (saveData == null)
        {
            saveData = GetComponent<PlayerSaveSystem>();
        }
        levelPlayer = saveData.GetPlayerLevel();
        return playerLevelUpData.GetLevelInfo(levelPlayer);
    }

    public int GetPlayerLevel() {
        return saveData.GetPlayerLevel();
    }

    /// <summary>
    /// player is not at level 0
    /// </summary>
    /// <returns></returns>
    public int GetUpgradeCost() {
        return playerLevelUpData.GetLevelInfo(GetPlayerLevel() + 1).GetupgradeCost();
    }

}


[System.Serializable]
public struct STRLevelData
{
    [SerializeField]
    STRId ids;
    [SerializeField]
    STRStatus status;
    [SerializeField]
    [Tooltip("Coins needed to level up")]
    int maxCoinsToUp;

    public STRStatus GetStatusData() {
        return status;
    }
    public STRId GetIdData() {
        return ids;
    }

    public int GetupgradeCost() {
        return maxCoinsToUp;
    }
}

[System.Serializable]
public struct STRStatus
{
    public float hp;
    public float dmg;
    public float sukma;
    public float rageTime;
}




[System.Serializable]
public struct STRId
{
    public string name;
    public string shortDesc;
}

[System.Serializable]
public struct STRContinueEffect {
    [Tooltip("You may use same name as in ids.name")]
    public string name;
    public float duration;
    public STRStatus effected;
}

[System.Serializable]
public struct STRCamShakeProperties {
    public float duration;
    public float shakeAmount;
    public float decreaseFactor;
}

