using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KanKikuchi.AudioManager;
[RequireComponent(typeof(FireManager_1))]
public class FireSEPlayer : SEPlayer
{
    FireManager_1 buf;
    private void Awake()
    {
        buf = GetComponent<FireManager_1>();
        buf.onFire += OnAction;
    }
    private void OnDestroy()
    {
        buf.onFire -= OnAction;
    }
}
