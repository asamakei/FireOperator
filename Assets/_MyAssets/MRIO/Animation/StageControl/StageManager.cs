using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageManager : MonoBehaviour{
    public static bool IsStageStart = false;
    public static StageManager Manager;


    void Awake(){
        StageManager.Manager = this;
    }

    void Start() {
        StartCoroutine(stageStart());
    }

    IEnumerator stageStart() {
        IsStageStart = true;
        yield return null;
    }
}
