 using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class StageInfoManager : MonoBehaviour
{
    [SerializeField] StageVariableDataSO defaultStageVariableDataSO;
    [SerializeField] Image image;
    [SerializeField] TMPro.TextMeshProUGUI stageText;
    [SerializeField] Text stageNumberText;
    [SerializeField] LevelView levelView;
    StageVariableData selectedstageData = null;
    private void Awake()
    {
        if (selectedstageData == null && defaultStageVariableDataSO != null) ChangeStageInfo(defaultStageVariableDataSO.stageVariableData);
    }

    public StageVariableData stageVariableData
    {
        get { return selectedstageData; }
    }

    public void ChangeStageInfo(StageVariableData stageVariableData)
    {
        this.selectedstageData = stageVariableData;
        image.sprite = stageVariableData.stageData.stagePicture;
        stageText.SetText(stageVariableData.stageData.stageText);
        if (stageNumberText != null) stageNumberText.text = stageVariableData.stageData.stageButtonNumber;
        if (levelView != null) levelView.ChangeLevel(stageVariableData.stageData.level);
    }
} 
