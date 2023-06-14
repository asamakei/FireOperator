using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearButton : MonoBehaviour
{
    [SerializeField] UIState changingState = UIState.Clear;
    public void Clear()
    {
        UIManager.uIState = changingState;
    }
}
