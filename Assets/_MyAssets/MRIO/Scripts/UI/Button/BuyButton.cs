using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
[RequireComponent(typeof(SpriteRenderer))]
public class BuyButton : SpriteButtonBase
{
    [SerializeField] int defaultCost = 10;
    [SerializeField] SpriteRenderer _spriteRenderer;
    [SerializeField] Color unbuyableColor;
    int cost;
    bool isBuyable;

    public int Cost
    {
        set { cost = value; }
        get { return cost; }
    }

    public bool IsBuyable
    {
        set { isBuyable = value; }
        get { return isBuyable; }
    }
    protected override void Awake()
    {
        cost = defaultCost;
       
        base.Awake();
    }
    public void ReflectBuyable()
    {
        _spriteRenderer.color = (isBuyable) ? Color.white : unbuyableColor;
        isPushable = isBuyable;
    }
    public override void OnPointerDown(PointerEventData eventData)
    {
        if (!isPushable) return;
        base.OnPointerDown(eventData);
        isBuyable = SaveDataManager.Instance.saveData.shopData.coinNum > cost;
        ReflectBuyable();
    }
    public override void OnPointerUp(PointerEventData eventData)
    {
        if (!isPushable) return;
        base.OnPointerUp(eventData);
    }
}
