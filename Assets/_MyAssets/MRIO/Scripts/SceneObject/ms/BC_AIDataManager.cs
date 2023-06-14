using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
[System.Serializable]
public class BC_AIData
{
    public Transform[] movePath;
    public float[] moveDuration;
    public Ease[] randomEase;
    public bool isShake = false;
    public bool isIceBorn = false;
    public SpriteRenderer spriteRenderer;
    public IceSpawner iceSpawner;
    public ToPlayerShoot toPlayerShoot;
    public float superShootSpeed = 5;
    public float superShootInterval = 0.4f;
    public Vector3 hitScaleAmount;
}
public class BC_AIDataManager : MonoBehaviour
{
    [SerializeField] BC_AIData aidata;
    public BC_AIData aiData
    {
        get { return aidata; }
    }
}
