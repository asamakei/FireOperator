using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceSpawner : ObjectPoolingManager
{
    [SerializeField] float posDistance;
    [SerializeField] float spawnInterval = 0.3f;
    float time;

    private void Update()
    {
        if (spawnInterval == 0) return;
        time += Time.deltaTime;
        if (time < spawnInterval) return;
        time = 0;
        float rad = Random.Range(0, 360) * Mathf.Deg2Rad;
        Spawn(_transform.position + new Vector3(Mathf.Cos(rad), Mathf.Sin(rad), 0) * posDistance);
    }

    public void Spawn(Vector3 pos)
    {
        GameObject obj = CreateInstance();
        if (obj == null) return;
        obj.transform.localScale = poolObjectPrefab.transform.localScale;
        obj.transform.position = pos;
        obj.SetActive(true);
    }

}
