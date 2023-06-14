using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using EnhancedUI.EnhancedScroller;
public class StageRowCellView : EnhancedScrollerCellView
{
    [SerializeField] List<StageCellView> stageCellViews;
    public int NumOfCellViews
    {
        get { return stageCellViews.Count; }
    }

    public void SetDatas(StageVariableData[] stageVariableDatas, int startindex, Action<StageVariableData> onStageSelected, Sprite notPlayableSprite)
    {
        for (int i = 0; i < stageCellViews.Count; i++)
        {
            int stockindex = i + startindex * stageCellViews.Count;

            if (stockindex >= stageVariableDatas.Length || stockindex < 0)//null‚Ìê‡‚Í”ñ•\Ž¦
            {
                stageCellViews[i].gameObject.SetActive(false);
                continue;
            }
            else
            {
                stageCellViews[i].gameObject.SetActive(true);
                stageVariableDatas[stockindex].notPlayableSprite = notPlayableSprite;
                stageCellViews[i].SetData(stageVariableDatas[stockindex], onStageSelected);
            }
            
        }
    }
}
