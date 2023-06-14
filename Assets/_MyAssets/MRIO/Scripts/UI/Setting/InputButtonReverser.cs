using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputButtonReverser : MonoBehaviour
{
    [SerializeField] Transform moveButtonTransform;
    [SerializeField] Transform jumpButtonTransform;

    private void Start()
    {
        if (SaveDataManager.Instance != null && SaveDataManager.Instance.saveData.optionData.shouldReverseUI) ReverseButton();
    }

    void ReverseButton()
    {
        RectTransform moveRectTransform = moveButtonTransform as RectTransform;
        RectTransform jumpRectTransform = jumpButtonTransform as RectTransform;
        Vector2 maxTmp = moveRectTransform.anchorMax;
        Vector2 minTmp = moveRectTransform.anchorMin;
        moveRectTransform.anchorMax = jumpRectTransform.anchorMax;
        moveRectTransform.anchorMin = jumpRectTransform.anchorMin;
        jumpRectTransform.anchorMax = maxTmp;
        jumpRectTransform.anchorMin = minTmp;
        Vector3 buf = moveRectTransform.anchoredPosition;
        moveRectTransform.anchoredPosition = jumpRectTransform.anchoredPosition;
        jumpRectTransform.anchoredPosition = buf;
    }
}
