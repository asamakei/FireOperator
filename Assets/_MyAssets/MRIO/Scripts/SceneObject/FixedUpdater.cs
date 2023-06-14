using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FixedUpdater : MonoBehaviour
{
    IFixedUpdater[] iFixedUpdaters;
    public void RefreshUpdaters()
    {
        iFixedUpdaters = transform.GetComponentsInChildren<IFixedUpdater>(true);
    }
    private void Start()
    {
        RefreshUpdaters();
    }

    private void FixedUpdate()
    {
        foreach(IFixedUpdater fixedUpdater in iFixedUpdaters)
        {
            fixedUpdater.OnFixedUpdate();
        }
    }
}
