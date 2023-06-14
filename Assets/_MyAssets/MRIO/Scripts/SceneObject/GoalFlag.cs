using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalFlag : GoalObject
{
    public override void OnClear()
    {
        UIManager.uIState = UIState.Clear;
    }

}
