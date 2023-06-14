using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GoalObject : MonoBehaviour
{
    public abstract void OnClear();
    private void OnTriggerEnter2D(Collider2D collision)
    { 
        if (collision.gameObject.CompareTag(Strings.PLAYER))
        {
            OnClear();
        }
    }
}
