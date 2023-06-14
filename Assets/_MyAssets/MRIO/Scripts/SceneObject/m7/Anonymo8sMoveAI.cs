using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Anonymo8sMoveAI : MonoBehaviour
{
    [SerializeField] Transform handTransform;
    [SerializeField] Transform[] paths;
    [SerializeField] Ease[] randomEases;
    [SerializeField] float[] randomDurations;
    [SerializeField] float[] randomWaits;
     
    private void Awake()
    {
        Transform _transform = transform;
        Sequence sequence = DOTween.Sequence().Pause();
        for (int i = 0;i < paths.Length; i++)
        {
            float duration = randomDurations[Random.Range(0, randomDurations.Length - 1)];
            float randomWait = randomWaits[Random.Range(0, randomWaits.Length - 1)];
            Ease ease = randomEases[Random.Range(0, randomEases.Length - 1)];
            sequence.Append(handTransform.DOMove(paths[i].position, duration).SetEase(ease));
            sequence.AppendInterval(randomWait);
        }
        sequence.Play().SetLink(handTransform.gameObject).SetLoops(-1,LoopType.Yoyo);
    }
    private void OnDestroy()
    {
        handTransform.DOKill();
    }
}
