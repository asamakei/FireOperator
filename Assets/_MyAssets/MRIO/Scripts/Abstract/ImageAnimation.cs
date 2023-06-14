using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
[RequireComponent(typeof(Image))]
public abstract class ImageAnimation : MonoBehaviour
{
    protected Image image;
    protected Transform _transform;
    private void Awake()
    {
        image = GetComponent<Image>();
        _transform = transform;
    }
    public abstract void StartAnimation();
    public abstract void EndAnimation();
}
