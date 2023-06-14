using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;
[RequireComponent(typeof(TextMeshPro))]
public class RandomText : MonoBehaviour
{
    TextMeshPro textMeshPro;
    [SerializeField] string[] randomTexts;
    [SerializeField] float[] randomDurations;
    [SerializeField] float[] randomWaits;
    float waitduration;
    float duration;
    // Start is called before the first frame update
    void Awake()
    {
        textMeshPro = GetComponent<TextMeshPro>();
        StartCoroutine(RandomTextAnimation());
    }

    private void OnEnable()
    {
        StartCoroutine(RandomTextAnimation());
    }

    private void OnDisable()
    {
        StopCoroutine(RandomTextAnimation());
    }

    IEnumerator RandomTextAnimation()
    {
        while (true)
        {
            textMeshPro.DOKill();
            textMeshPro.SetText("");
            string text = randomTexts[Random.Range(0, randomTexts.Length - 1)];
            duration = randomDurations[Random.Range(0, randomDurations.Length - 1)];
            waitduration = randomWaits[Random.Range(0, randomWaits.Length - 1)];

            textMeshPro.DOText(text, duration);
            yield return new WaitForSeconds(duration);

            yield return new WaitForSeconds(waitduration);
        }
        
    }
   
}
