using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
[System.Serializable]
public class C_AIData
{
    public Transform[] movePath;
    public float[] moveDuration;
    public StraightFireShooter[] straightFires;
    public Ease[] randomEase;
    public SpriteRenderer spriteRenderer;
    public SplitFace[] splitFaces;
    public float splitDuration = 10;
    public Transform[] splitMovedTransforms;
    public int splitFireNum = 10;
    public float startAngle = -30;
    public float endAngle = 30;
    public float fireSpeed = 4;
    public float distance = 10;
    public Vector3 punchScaleAmount;
}
public class C_AIDataManager : MonoBehaviour
{
    [SerializeField] C_AIData aidata;
    public C_AIData aiData
    {
        get { return aidata; }
    }
}
