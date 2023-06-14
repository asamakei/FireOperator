using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(SceneTransManager))]
public class TutorialManager : MonoBehaviour
{
    SceneTransManager sceneTransManager;
    private void Awake()
    {
        sceneTransManager = GetComponent<SceneTransManager>();
        sceneTransManager.AddListener(OnStageLoadEnd);
    }
    private void OnDestroy()
    {
        sceneTransManager.RemoveListener(OnStageLoadEnd);
    }

    public void OnStageLoadEnd()
    {
        int index = Mathf.Min(MasterDataManager.Instance.stageVariableDataDBSO.stageVariableDataSOs.Length - 1, Variables.currentStageIndex);
        StageVariableData stageVariableData = MasterDataManager.Instance.stageVariableDataDBSO.stageVariableDataSOs[index].stageVariableData;
        if (!stageVariableData.shouldTutorial) return;
        
        if (EditableTextWindowWithVideo.i == null) return;
        EditableTextWindowWithVideo.i.EditText(stageVariableData.stageData.tutorialText);
        EditableTextWindowWithVideo.i.SetVideo(stageVariableData.stageData.tutorialClip);
        EditableTextWindowWithVideo.i.Activate();
    }
}
