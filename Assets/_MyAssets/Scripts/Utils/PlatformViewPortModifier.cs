using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Camera))]
public class PlatformViewPortModifier : MonoBehaviour
{
    Camera mainCamera;
    private void Awake()
    {
        mainCamera = Camera.main;

        if (Utility.Utils.IsIPad)
        {
            mainCamera.rect = new Rect(new Vector2(0,0.33f),new Vector2(1,1));
        }
        else
        {
            mainCamera.rect = new Rect(new Vector2(0, 0.3f), new Vector2(1, 1));
        }

    }
}
