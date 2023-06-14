using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(CanvasScaler))]
public class PlatformScalerChanger : MonoBehaviour
{
    CanvasScaler canvasScaler;
    private void Awake()
    {
        canvasScaler = GetComponent<CanvasScaler>();

        if (Utility.Utils.IsIPad)
        {
            canvasScaler.matchWidthOrHeight = 0;
        }
        else
        {
            canvasScaler.matchWidthOrHeight = 1;
        }

    }
}
