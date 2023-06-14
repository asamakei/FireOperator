using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(StageButtonBase))]
public class TutorialIntroducer : MonoBehaviour
{
    [SerializeField] StageVariableDataSO tutorialStageData;
    StageButtonBase stageButtonBase;
    private void Awake()
    {
        stageButtonBase = GetComponent<StageButtonBase>();
        stageButtonBase.onPushed += OnPushed;
    }

    private void OnDestroy()
    {
        stageButtonBase.onPushed -= OnPushed;
    }

    public void OnPushed()
    {
        if (tutorialStageData == null || SaveDataManager.Instance.saveData.wasTutorialPlayed)
        {
            Variables.gameState = GameState.Game;
            return;
        }
        Variables.gameState = GameState.Tutorial;
        Variables.currentStageIndex = tutorialStageData.stageVariableData.stageIndex;
        SceneTransManager.instance.SceneTrans(tutorialStageData.stageVariableData.stageData.loadingScenes);
        SceneTransManager.instance.AddListener(OnSceneLoadEnd);
    }

    public void OnSceneLoadEnd()
    {
        SaveDataManager.Instance.saveData.wasTutorialPlayed = true;
        SceneTransManager.instance.RemoveListener(OnSceneLoadEnd);
    }
}
