using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KanKikuchi.AudioManager;
[RequireComponent(typeof(Player))]
public class PlayerInstance : SingletonMonoBehaviour<PlayerInstance>
{
    Player player;
    public Player GetPlayer()
    {
        return player;
    }
    protected override void Awake()
    {
        player = GetComponent<Player>();
        int index = Mathf.Min(MasterDataManager.Instance.stageVariableDataDBSO.stageVariableDataSOs.Length - 1, Variables.currentStageIndex);
        Vector3 defaultPos = MasterDataManager.Instance.stageVariableDataDBSO.stageVariableDataSOs[index].stageVariableData.stageData.defaultPlayerPosition;
        if (defaultPos != Vector3.zero) player.transform.position = defaultPos;
        base.Awake();
    }
}
