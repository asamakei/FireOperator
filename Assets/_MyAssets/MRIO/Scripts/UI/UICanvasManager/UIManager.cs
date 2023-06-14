using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public enum UIState
{
    Title,
    Select,
    Clear,
    Fail,
    Game
}
public class UIManager : MonoBehaviour
{
    [SerializeField] UIState defaultUIState = UIState.Title;
    public UIState DefaultUIState
    {
        set { defaultUIState = value; }
    }
    CanvasManagerBase[] canvasManagerBases;
    public event Action OnSetCanvas;
    public static UIState uIState;
    private void Awake()
    {
        canvasManagerBases = transform.GetComponentsInChildren<CanvasManagerBase>(true);
        uIState = defaultUIState;
    }
    void Start()
    {
        if (OnSetCanvas != null) OnSetCanvas();
        SetCanvasManagers();
    }
    void SetCanvasManagers()
    {
        foreach (var canvasManagerBase in canvasManagerBases)
        {
            canvasManagerBase.Init();
            canvasManagerBase.OnStart();
            if (canvasManagerBase.uIState != defaultUIState) canvasManagerBase.gameObject.SetActive(false);
        }
        uIState = defaultUIState;

    }
    void Update()
    {
        foreach (var canvasManagerBase in canvasManagerBases)
        {
            canvasManagerBase.OnUpdate();
        }
    }
}
