using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Face : MonoBehaviour
{
    [SerializeField] SpriteRenderer faceRenderer;
    [SerializeField] SpriteRenderer wholeEye;
    [SerializeField] SpriteRenderer mouth;
    [SerializeField] SpriteRenderer eye;
    public void ChangeFlip(bool isFlipX)
    {
        if (faceRenderer == null || wholeEye == null || mouth == null || eye == null) return;
        faceRenderer.flipX = isFlipX;
        wholeEye.flipX = isFlipX;
        mouth.flipX = isFlipX;
        eye.flipX = isFlipX;
    }
}
