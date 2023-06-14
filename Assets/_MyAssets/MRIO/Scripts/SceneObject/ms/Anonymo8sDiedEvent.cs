using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
[RequireComponent(typeof(TouchableFriend))]
public class Anonymo8sDiedEvent : MonoBehaviour
{
    [SerializeField] GameObject[] deletingObjects;
    TouchableFriend touchableFriend;

    private void Awake()
    {
        touchableFriend = GetComponent<TouchableFriend>();
        touchableFriend.onDied += OnDied;
    }

    private void OnDestroy()
    {
        touchableFriend.onDied -= OnDied;
    }

    void OnDied()
    {
        Array.ForEach(deletingObjects, obj => obj.SetActive(false));
    }
}
