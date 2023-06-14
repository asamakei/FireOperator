using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(StageButtonBase))]
public class ButtonAdActivater : MonoBehaviour
{
    [SerializeField] bool isActivate;
    [SerializeField] GameObject adObject;
    StageButtonBase stageButtonBase;
    private void Awake()
    {
        stageButtonBase = GetComponent<StageButtonBase>();
        stageButtonBase.onPushed += OnPushed;
    }
    private void OnDestroy()
    {
        stageButtonBase.onPushed -= OnPushed;
    }
    public void OnPushed()
    {
        adObject.SetActive(isActivate);
    }

}
