using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightTrailSpawner : ColorfulSpawner
{
    [SerializeField] Transform targetTouchable;
    [SerializeField] float checkInterval = 1f;
    [SerializeField] float minSpawnInterval = 0.1f;
    [SerializeField] float targetOffsetScale = 0.4f;
    float time = 0;
    float defaultInterval;
    private void Awake()
    {
        defaultInterval = spawnInterval;
    }
    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        time += Time.deltaTime;
        if (time > checkInterval && targetTouchable != null)
        {
            time = 0;
            spawnInterval = Mathf.Max(Mathf.Min(defaultInterval * (targetTouchable.localScale.x - targetOffsetScale), defaultInterval), minSpawnInterval);
        }
    }
    protected override void Spawn()
    {
        GameObject obj = CreateInstance();
        if (obj == null) return;
        if (obj.TryGetComponent<TrailRenderer>(out TrailRenderer trailRenderer))
        {
            trailRenderer.material.color = randomColors[Random.Range(0, randomColors.Length - 1)];
            trailRenderer.enabled = true;
        }
        Vector2 spawnPosition = new Vector2(Random.Range(rect.xMin, rect.xMax), Random.Range(rect.yMin, rect.yMax));
        obj.transform.position = spawnPosition;
        obj.SetActive(true);
    }
}
