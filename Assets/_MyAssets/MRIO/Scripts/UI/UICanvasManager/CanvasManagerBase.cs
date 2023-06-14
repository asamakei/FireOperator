using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
public abstract class CanvasManagerBase : MonoBehaviour
{
    UIState uiState;
    public UIState uIState
    {
        get { return uiState; }
    }
    protected void SetStateAction(UIState uIState)
    {
        this.uiState = uIState;

        this.ObserveEveryValueChanged(uIState => UIManager.uIState)
            .Where(uIState => uIState == this.uiState)
            .Subscribe(screenState =>
            {
                OnOpen();
            })
            .AddTo(this.gameObject);

        this.ObserveEveryValueChanged(uIState => UIManager.uIState)
            .Buffer(2, 1)
            .Where(uiStates => uiStates.Count > 1)
            .Where(uIStates => uIStates[0] == uIState)
            .Where(uIStates => uIStates[1] != uIState)
            .Subscribe(screenState => OnClose())
            .AddTo(this.gameObject);
    }
    public void Init()
    {
        
    }
    public abstract void OnStart();
    public abstract void OnUpdate();
    protected abstract void OnOpen();
    protected abstract void OnClose();
    public virtual void OnInitialize()
    {
    }
}
