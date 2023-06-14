using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "MyGame/StageVariableDataSO", fileName = "Stage 1")]
public class StageVariableDataSO : ScriptableObject
{
    [SerializeField] StageVariableData stagevariabledata;
    public StageVariableData stageVariableData
    {
        get { return stagevariabledata; }
    }
}
