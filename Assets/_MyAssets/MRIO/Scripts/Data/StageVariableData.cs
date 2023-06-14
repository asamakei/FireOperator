using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class StageVariableData
{
    public StageData stageData;
    public bool isPlayable;
    public bool isShowable = true;
    public bool shouldTutorial = false;
    public ActivatingState activatingStates = ActivatingState.Retry | ActivatingState.Skip | ActivatingState.Title;
    [HideInInspector] public Sprite notPlayableSprite;
    [HideInInspector] public int stageIndex;
    [HideInInspector] public Transform buttonTransform;
}
