using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishSpwner : ObjectPoolingManager
{
    [SerializeField] float spawnInterval = 1f;
    [SerializeField] RectTransform spawnRect;
    [SerializeField] FixedUpdater fixedUpdater;
    float time = 0;
    Rect rect;
    protected override void Start()
    {

        Vector2 position = spawnRect.position;
        var bottomLeftPos = Vector2.zero;
        var topRightPos = Vector2.zero;

        bottomLeftPos.x = position.x - spawnRect.rect.width * spawnRect.lossyScale.x * spawnRect.pivot.x;
        bottomLeftPos.y = position.y - spawnRect.rect.height * spawnRect.lossyScale.y * spawnRect.pivot.y;
        topRightPos.x = position.x + spawnRect.rect.width * spawnRect.lossyScale.x * (1 - spawnRect.pivot.x);
        topRightPos.y = position.y + spawnRect.rect.height * spawnRect.lossyScale.y * (1 - spawnRect.pivot.y);

        rect = new Rect(bottomLeftPos, topRightPos - bottomLeftPos);
        base.Start();
        if (fixedUpdater != null) fixedUpdater.RefreshUpdaters();
    }
    private void FixedUpdate()
    {
        time += Time.deltaTime;
        if(time > spawnInterval)
        {
            time = 0;
            Spawn();
        }
    }

    void Spawn()
    {
        GameObject obj = CreateInstance();
        if (obj == null) return;
        Vector2 spawnPosition = new Vector2(Random.Range(rect.xMin, rect.xMax), Random.Range(rect.yMin, rect.yMax));
        obj.transform.position = spawnPosition;
        obj.SetActive(true);

    }
}
