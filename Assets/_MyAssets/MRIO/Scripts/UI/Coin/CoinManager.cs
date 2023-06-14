using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using KanKikuchi.AudioManager;
public class CoinManager : SingletonMonoBehaviour<CoinManager>
{
    [SerializeField] int coinMax = 99999999;
    Transform _transform;
    public Transform CoinTransform
    {
        get { return _transform; }
    }
    private void Start()
    {
        _transform = transform;
    }

    public void Addcoin(int amount)
    {
        int coinNum = Mathf.Clamp(SaveDataManager.Instance.saveData.shopData.coinNum + amount, 0,coinMax);
        SaveDataManager.Instance.saveData.shopData.coinNum = coinNum;
    }
}
