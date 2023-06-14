using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "MyGame/StageDataDBSO", fileName = "StageDB")]
public class StageVariableDataDBSO : ScriptableObject
{
    [SerializeField] StageVariableDataSO[] stagevariabledataSOs;

    public StageVariableDataSO[] stageVariableDataSOs
    {
        get { return stagevariabledataSOs; }
    }
    private void OnEnable()
    {
        for (int i = 0; i < stagevariabledataSOs.Length; i++)
        {
            stagevariabledataSOs[i].stageVariableData.stageIndex = i;
        }
    }
}
