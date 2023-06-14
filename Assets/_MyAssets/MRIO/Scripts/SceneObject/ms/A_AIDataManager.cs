using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class A_AIData
{
    public Transform[] movePath;
    public float[] moveDuration;
    public WholeEye wholeEye;
    public Mouth mouth;
    public float eyeMoveAmount = 5;
    public float eyeMoveDuration = 2;
    public Face face;
    public Vector3 punchScaleAmount;
}
public class A_AIDataManager : MonoBehaviour
{
    [SerializeField] A_AIData aidata;
    public A_AIData aiData
    {
        get { return aidata; }
    }
}
