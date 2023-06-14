using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KanKikuchi.AudioManager;
public class MasterDataManager : SingletonMonoBehaviour<MasterDataManager>
{
    [SerializeField] StageVariableDataDBSO stagevariableDataDBSO;
    public StageVariableDataDBSO stageVariableDataDBSO
    {
        get { return stagevariableDataDBSO; }
    }
}
