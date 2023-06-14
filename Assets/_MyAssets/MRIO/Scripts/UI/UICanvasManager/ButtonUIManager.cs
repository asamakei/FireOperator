using System;
using UnityEngine;
[Flags]
public enum ActivatingState
{
    Retry = 1,
    Title = 1 << 1,
    Skip = 1 << 2
}
[Serializable]
public class ActivatableButton
{
    public GameObject gameobject;
    public ActivatingState activatingState;
}

public class ButtonUIManager : MonoBehaviour
{
    [SerializeField] ActivatableButton[] activatableButtons;
    ActivatingState activatingState;
    public static ButtonUIManager i;
    private void Awake()
    {
        i = this;
    }

    public void ChangeActivatingButton(ActivatingState activatingButtons)
    {
        this.activatingState = activatingButtons;
        ActivateButton();
    }

    void ActivateButton()
    {
        Array.ForEach(activatableButtons, activatableButton =>
         {
             activatableButton.gameobject.SetActive(activatingState.HasFlag(activatableButton.activatingState));
         });
    }
}
