using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
[Serializable]
public class ShopData
{
    public int coinNum;
    public int iceNum;
    public int iceBuyCost;
    public List<int> getterids;

    public ShopData()
    {
        getterids = new List<int>();
    }
}
