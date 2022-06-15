using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Submission_LevelUpInfo_", menuName = "Submission/Level Up Info/New ")]
public class DataLevelUpInfo : ScriptableObject
{
    [SerializeField]
    STRLevelData[] levels;

    public int GetMaxLevel() {
        return levels.Length;
    }

    public STRLevelData GetLevelInfo(int indexLevel) {
        return levels[indexLevel];
    }
}
