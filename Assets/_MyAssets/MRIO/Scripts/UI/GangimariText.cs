using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;
[RequireComponent(typeof(TextMeshProUGUI))]

public class GangimariText : MonoBehaviour
{
    [SerializeField] float duration;
    [SerializeField] private Color[] colors;
    DOTweenTMPAnimator tmpAnimator;
    TextMeshProUGUI textMeshProUGUI;

    private void Awake()
    {
        DOTween.SetTweensCapacity(500, 50);
        textMeshProUGUI = GetComponent<TextMeshProUGUI>();
        tmpAnimator = new DOTweenTMPAnimator(textMeshProUGUI);
        Play();
    }

    public void Initialize()
    {
        for (var i = 0; i < tmpAnimator.textInfo.characterCount; i++)
        {
            tmpAnimator.DOColorChar(i, colors[i], 0);
            tmpAnimator.DOOffsetChar(i, Vector3.zero, 0);
        }
    }
    List<Sequence> sequences;
    List<Tween> tweens;
    public void Play()
    {
        sequences = new List<Sequence>();
        tweens = new List<Tween>();
        const float EACH_DELAY_RATIO = 0.01f;
        var eachDelay = duration * EACH_DELAY_RATIO;
        var eachDuration = duration - eachDelay;
        var charCount = tmpAnimator.textInfo.characterCount;
       
        for (var i = 0; i < charCount; i++)
        {
            sequences.Add(DOTween.Sequence()
                .Append(tmpAnimator.DOOffsetChar(i, Vector3.up * 30, eachDuration / 4).SetEase(Ease.OutFlash, 2).SetLink(gameObject))
                .SetDelay(eachDelay * i)
                .SetLoops(-1).SetLink(gameObject));
            Debug.Log("A");
            var i2 = i;
            tweens.Add(DOVirtual.Float(0f, charCount, duration, value =>
             {
                 var colorIdx = (i2 + (int)value) % charCount;
                 tmpAnimator.DOColorChar(i2, colors[colorIdx], duration).SetLink(gameObject);
             }).SetEase(Ease.Linear).SetLoops(-1).SetLink(gameObject));
        }
    }
    private void OnDestroy()
    {
        sequences.ForEach(sequence => sequence.Kill());
        tweens.ForEach(tween => tween.Kill());
    }
}
