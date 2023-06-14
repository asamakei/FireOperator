using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EnhancedUI.EnhancedScroller;
public class StageController : MonoBehaviour, IEnhancedScrollerDelegate, IActivater
{
    [SerializeField] StageInfoManager stageInfoManager;
    [SerializeField] StageRowCellView stageRowCellView;
    [SerializeField] EnhancedScroller enhancedScroller;
    [SerializeField] Sprite notPlayableSprite;
    [SerializeField] float cellViewSize = 100;
    [SerializeField] Transform cursorTransform;
    private void Start()
    {
        if (enhancedScroller != null)
        {
            enhancedScroller.cellViewVisibilityChanged = SetDatas;
        }
        Activate(0);
    }
    public void SetDatas(EnhancedScrollerCellView rowCellView)
    {
        if (rowCellView.active)
        {
            var cellView = (StageRowCellView)rowCellView;

            cellView.SetDatas(SaveDataManager.Instance.ShowableVariableDatas, rowCellView.dataIndex, OnStageSelected, notPlayableSprite);
        }
    }

    public EnhancedScrollerCellView GetCellView(EnhancedScroller scroller, int dataIndex, int cellIndex)
    {
        return scroller.GetCellView(stageRowCellView);
    }

    public float GetCellViewSize(EnhancedScroller scroller, int dataIndex)
    {
        return cellViewSize;
    }

    public int GetNumberOfCells(EnhancedScroller scroller)
    {
        int num = SaveDataManager.Instance.ShowableVariableDatas.Length;
        return Mathf.CeilToInt(((float)num / (float)stageRowCellView.NumOfCellViews));
    }

    public void SetCells()
    {
        enhancedScroller.Delegate = this;
        enhancedScroller.ReloadData();
    }

    public void OnStageSelected(StageVariableData stageVariableData)
    {
        if (cursorTransform != null && stageVariableData.buttonTransform != null) cursorTransform.position = stageVariableData.buttonTransform.position;
        stageInfoManager.ChangeStageInfo(stageVariableData);
    }

    public void Activate(float duration)
    {
        gameObject.SetActive(true);
        SetCells();
    }

    public void DeActivate(float duration)
    {
        gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        Activate(0);
    }
}
