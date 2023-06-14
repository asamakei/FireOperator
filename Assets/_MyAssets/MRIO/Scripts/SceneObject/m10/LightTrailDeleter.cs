using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(TrailRenderer))]
public class LightTrailDeleter : MonoBehaviour
{
    TrailRenderer trailRenderer;
    private void Awake()
    {
        trailRenderer = GetComponent<TrailRenderer>();
    }

    private void OnDisable()
    {
        trailRenderer.Clear();
        trailRenderer.enabled = false;
    }
}
