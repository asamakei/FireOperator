using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class LoopMoverStarter : MonoBehaviour
{
    [SerializeField] LoopMover loopMover;
    [SerializeField] Vector2 defaultWeight;
    [SerializeField] GameObject[] activatingObjects;
    private void Awake()
    {
        Array.ForEach(activatingObjects, obj => obj.SetActive(false));
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(Strings.PLAYER) && loopMover != null)
        {
            loopMover.MoveWeight = defaultWeight;
            Array.ForEach(activatingObjects, obj => obj.SetActive(true));
            gameObject.SetActive(false);
        }
    }
}
