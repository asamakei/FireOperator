using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BackGroundSetter : MonoBehaviour
{
    [SerializeField] Image backGroundImage;
    private void Start()
    {
        int index = Mathf.Min(MasterDataManager.Instance.stageVariableDataDBSO.stageVariableDataSOs.Length - 1, Variables.currentStageIndex);
        StageData stageData = MasterDataManager.Instance.stageVariableDataDBSO.stageVariableDataSOs[index].stageVariableData.stageData;
        if(stageData.backGroundPicture != null && backGroundImage != null)
        {
            backGroundImage.gameObject.SetActive(true);
            backGroundImage.sprite = stageData.backGroundPicture;
        }
        else
        {
            backGroundImage.gameObject.SetActive(false);
        }
    }
}
