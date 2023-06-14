using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialButtonDeleter : MonoBehaviour
{
    private void Start()
    {
        if (Variables.gameState == GameState.Tutorial) ButtonUIManager.i.ChangeActivatingButton(~ActivatingState.Title & ~ActivatingState.Retry & ~ActivatingState.Skip);
        else ButtonUIManager.i.ChangeActivatingButton(ActivatingState.Title | ActivatingState.Retry | ActivatingState.Skip);
    }
}
