using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
[RequireComponent(typeof(Light2D))]
public class LightGlitch : MonoBehaviour
{
    static float MAXTIME = 5f;
    [SerializeField] AnimationCurve animationCurve;
    [SerializeField] float speed = 1;
    [SerializeField] float intensity = 1;
    Light2D light2D;
    float time;
    private void Start()
    {
        light2D = GetComponent<Light2D>();
    }

    private void FixedUpdate()
    {
        time = (time > MAXTIME) ? 0 : time + Time.deltaTime * speed;
        if (animationCurve != null && light2D != null)
        {
            light2D.intensity = intensity * animationCurve.Evaluate(time);
        }
    }
}
