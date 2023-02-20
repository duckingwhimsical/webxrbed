using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PulseLight : MonoBehaviour
{
    public float speed = 2f;
    public float amplitude = 1f;
    public float frequency = 2f;

    private Light light;

    void Start()
    {
        light = this.gameObject.GetComponent<Light>();
    }

    // Update is called once per frame
    void Update()
    {
        light.intensity = ((Mathf.Sin(Time.time * frequency) * amplitude + 0.5f) / 2) + 0.25f;
    }
}
