using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorfulSpawner : ObjectPoolingManager
{
    [SerializeField] protected float spawnInterval = 1f;
    [SerializeField] RectTransform spawnRect;
    [SerializeField] FixedUpdater fixedUpdater;
    [SerializeField] protected Color[] randomColors;
    float time = 0;
    protected Rect rect;
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
    protected virtual void FixedUpdate()
    {
        time += Time.deltaTime;
        if (time > spawnInterval)
        {
            time = 0;
            Spawn();
        }
    }

    protected virtual void Spawn()
    {
        GameObject obj = CreateInstance();
        if (obj == null) return;
        if (obj.TryGetComponent<SpriteRenderer>(out SpriteRenderer spriteRenderer))
        {
            spriteRenderer.color = randomColors[Random.Range(0, randomColors.Length - 1)];
        }
        Vector2 spawnPosition = new Vector2(Random.Range(rect.xMin, rect.xMax), Random.Range(rect.yMin, rect.yMax));
        obj.transform.position = spawnPosition;
        obj.SetActive(true);

    }
}
