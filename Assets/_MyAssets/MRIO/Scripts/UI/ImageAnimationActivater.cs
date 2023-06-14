using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(ImageAnimation))]
public class ImageAnimationActivater : MonoBehaviour
{
    ImageAnimation imageAnimation;
    private void Awake()
    {
        imageAnimation = GetComponent<ImageAnimation>();
    }
    private void OnEnable()
    {
        if (imageAnimation != null) imageAnimation.StartAnimation();
    }
    private void OnDisable()
    {
        if (imageAnimation != null) imageAnimation.EndAnimation();
    }
}
