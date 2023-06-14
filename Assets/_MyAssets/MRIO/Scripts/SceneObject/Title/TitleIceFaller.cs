using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleIceFaller : MonoBehaviour
{
    [SerializeField] GameObject icePrefab;
    [SerializeField] int iceMaxNum = 30;
    [SerializeField] RectTransform spawnRect;
    Rect rect;
    GameObject[] objects;
    private void Start()
    {

        Vector2 position = spawnRect.position;
        var bottomLeftPos = Vector2.zero;
        var topRightPos = Vector2.zero;

        bottomLeftPos.x = position.x - spawnRect.rect.width * spawnRect.lossyScale.x * spawnRect.pivot.x;
        bottomLeftPos.y = position.y - spawnRect.rect.height * spawnRect.lossyScale.y * spawnRect.pivot.y;
        topRightPos.x = position.x + spawnRect.rect.width * spawnRect.lossyScale.x * (1 - spawnRect.pivot.x);
        topRightPos.y = position.y + spawnRect.rect.height * spawnRect.lossyScale.y * (1 - spawnRect.pivot.y);

        rect = new Rect(bottomLeftPos, topRightPos - bottomLeftPos);
        List<GameObject> ices = new List<GameObject>();
        for (int i = 0; i < Mathf.Min(SaveDataManager.Instance.saveData.shopData.iceNum, iceMaxNum); i++)
        {
            GameObject obj = Instantiate(icePrefab,transform);
            obj.transform.position = new Vector2(Random.Range(rect.xMin, rect.xMax), Random.Range(rect.yMin, rect.yMax));
            ices.Add(obj);
        }
    }
}
