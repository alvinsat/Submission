using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Submission_Status_", menuName = "Submission/Status/New ")]
public class DataStatus : ScriptableObject
{
    [SerializeField]
    STRId ids;
    [SerializeField]
    STRStatus status;
}
