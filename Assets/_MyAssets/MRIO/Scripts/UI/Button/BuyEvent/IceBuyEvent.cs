using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;
using KanKikuchi.AudioManager;
[RequireComponent(typeof(BuyButton))]
public class IceBuyEvent : MonoBehaviour
{
    [SerializeField] int maxBuyableNum = 45;
    [SerializeField] float increaseRate = 1.5f;
    [SerializeField] TextMeshProUGUI numText;
    [SerializeField] TextMeshProUGUI costText;
    [SerializeField] TextMeshProUGUI maxText;
    [SerializeField] Vector3 scaleAmount;
    [SerializeField] float scaleDuration;
    [SerializeField] string SEIdentifier;
    BuyButton buyButton;
    private void Start()
    {
        buyButton = GetComponent<BuyButton>();
        buyButton.onPushed += OnBuy;
        buyButton.Cost = (SaveDataManager.Instance.saveData.shopData.iceBuyCost == 0) ? buyButton.Cost : SaveDataManager.Instance.saveData.shopData.iceBuyCost;
        SetIceNum(SaveDataManager.Instance.saveData.shopData.iceNum);
        costText.SetText(buyButton.Cost.ToString());
        buyButton.IsBuyable = SaveDataManager.Instance.saveData.shopData.coinNum > buyButton.Cost;
        buyButton.ReflectBuyable();
    }
    private void OnDestroy()
    {
        buyButton.onPushed -= OnBuy;
    }
    public void OnBuy()
    {
        if (buyButton.Cost > SaveDataManager.Instance.saveData.shopData.coinNum) return;
        AudioData audioData = AudioDataManager.Instance.GetAudioData(SEIdentifier);
        if (audioData != null) SEManager.Instance.Play(audioData.audioClip, audioData.volume);
        SaveDataManager.Instance.saveData.shopData.iceNum++;
        SaveDataManager.Instance.saveData.shopData.coinNum = Mathf.Max(SaveDataManager.Instance.saveData.shopData.coinNum - buyButton.Cost, 0);
        SetIceNum(SaveDataManager.Instance.saveData.shopData.iceNum);
        buyButton.Cost = Mathf.CeilToInt(buyButton.Cost * increaseRate);
        costText.SetText(buyButton.Cost.ToString());
        SaveDataManager.Instance.saveData.shopData.iceBuyCost = buyButton.Cost;
        SaveDataManager.Instance.Save(Values.SAVENUMBER);
        CoinView.Instance.UpdateView();
        if(SaveDataManager.Instance.saveData.shopData.iceNum >= maxBuyableNum && maxText != null)
        {
            maxText.gameObject.SetActive(true);
            buyButton.IsBuyable = false;
        }
        buyButton.ReflectBuyable();
        if (SaveDataManager.Instance.saveData.shopData.iceNum >= maxBuyableNum) buyButton.IsBuyable = false;
    }
    Sequence sequence;
    public void SetIceNum(int num)
    {
        numText.SetText("x" + SaveDataManager.Instance.saveData.shopData.iceNum.ToString());
        if (sequence != null) sequence.Kill();
        sequence = DOTween.Sequence().Append(numText.transform.DOScale(scaleAmount, scaleDuration).SetEase(Ease.OutQuad).SetRelative())
                         .Append(numText.transform.DOScale(Vector3.one, scaleDuration).SetEase(Ease.OutQuad)).SetLink(gameObject).OnKill(() => numText.transform.localScale = Vector3.one);
    }
}
