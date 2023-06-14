using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;
using KanKikuchi.AudioManager;
public class CoinCountAnimation : MonoBehaviour, IActivater
{
    [SerializeField] GameObject coinObjectPrefab;
    [SerializeField] int poolingNum = 15;
    [SerializeField] Transform moveToPosTransform;
    [SerializeField] Vector3 scaleAmount;
    [SerializeField] float scaleDuration = 0.2f;
    [SerializeField] float coinAnimationDuration = 1.5f;
    [SerializeField] Transform coinSpawningPointTransform;
    [SerializeField] float randomDistanceMax = 40;
    [SerializeField] Vector3 coinMoveToOffset;
    [SerializeField] TextMeshProUGUI textMeshProUGUI;
    [SerializeField] string coinSEIdentifier;
    [SerializeField] string coinAppearIdentifier;
    Vector3 defaultPosition;
    Transform _transform;
    List<CoinObject> coinObjects;

    private void Awake()
    {
        _transform = transform;
        defaultPosition = _transform.position;
        gameObject.SetActive(false);
        coinObjects = new List<CoinObject>();
        for (int i = 0; i < poolingNum; i++)
        {
            GameObject obj = Instantiate(coinObjectPrefab, transform);
            obj.SetActive(false);
            if (obj.TryGetComponent<CoinObject>(out CoinObject coinObject))
            {
                coinObjects.Add(coinObject);
            }
        }
    }
    CoinObject GetInActiveCoinObject()
    {
        for (int i = 0; i < coinObjects.Count; i++)
        {
            if (!coinObjects[i].gameObject.activeSelf) return coinObjects[i];
        }
        return null;
    }
    Sequence sequence;
    void AnimateCount(int coinNum)
    {

        textMeshProUGUI.SetText(coinNum.ToString());
        if (sequence != null) sequence.Kill();
        textMeshProUGUI.transform.localScale = Vector3.one;
        sequence = DOTween.Sequence().Append(textMeshProUGUI.transform.DOScale(scaleAmount, scaleDuration).SetEase(Ease.OutQuad).SetRelative())
                          .Append(textMeshProUGUI.transform.DOScale(Vector3.one, scaleDuration).SetEase(Ease.OutQuad)).SetLink(gameObject);

    }

    public void DoCoinAddAnimation(int coinFrom, int coinNum, int coinValue)
    {
        int coinObjNum = Mathf.FloorToInt((float)coinNum / (float)coinValue);

        float perDuration = coinAnimationDuration / (float)coinObjNum;
        textMeshProUGUI.SetText(coinFrom.ToString());
        Vector3 basePos = (coinSpawningPointTransform == null) ? Vector3.zero : coinSpawningPointTransform.position;
        AudioData appearAudioData = AudioDBManager.Instance.audioDataDBSO.GetAudioData(coinAppearIdentifier);
        if (appearAudioData != null)
        {
            SEManager.Instance.Play(appearAudioData.audioClip, appearAudioData.volume);
        }
        for (int i = 0; i < coinObjNum; i++)
        {
            Vector3 pos = basePos + new Vector3(Random.Range(-randomDistanceMax, randomDistanceMax), Random.Range(-randomDistanceMax, randomDistanceMax), 0);
            CoinObject coinobj = GetInActiveCoinObject();
            if (coinobj == null) break;
            GameObject obj = coinobj.gameObject;
            
            obj.SetActive(true);
            coinobj.coinTransform.position = basePos;
            int textCoinNum = coinFrom + (i + 1) * coinValue;

            DOTween.Sequence().Append(coinobj.coinTransform.DOMove(pos, coinAnimationDuration / 2).SetEase(Ease.OutQuart))
                              .Append(coinobj.coinTransform.DOMove(moveToPosTransform.position + coinMoveToOffset, perDuration).SetDelay(coinAnimationDuration * ((float)i / (float)coinObjNum)).SetEase(Ease.OutQuart).OnComplete(() =>
                              {
                                  obj.SetActive(false);
                                  AnimateCount(textCoinNum);
                                  AudioData audioData = AudioDBManager.Instance.audioDataDBSO.GetAudioData(coinSEIdentifier);
                                  if (audioData != null) SEManager.Instance.Play(audioData.audioClip, audioData.volume);
                              })).SetLink(gameObject);
        }
    }

    public void Activate(float duration)
    {
        gameObject.SetActive(true);
        _transform.DOKill();
        _transform.DOMove(moveToPosTransform.position, duration).SetEase(Ease.OutQuad).SetLink(gameObject);
    }

    public void DeActivate(float duration)
    {
        gameObject.SetActive(false);
        _transform.DOMove(defaultPosition, duration).SetEase(Ease.OutQuad).SetLink(gameObject);
    }
}
