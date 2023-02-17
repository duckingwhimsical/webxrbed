using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class lightanim : MonoBehaviour
{
    public Transform close;
    public Transform far;
    public float speed = 0.1f;

    Vector3 position;
    Vector3 closePosition;
    Vector3 farPosition;

    private void Start()
    {
        closePosition = close.position;
        farPosition = far.position;
    }

    float sunriseStart = 6;
    float sunsetStart = 17;
    float transitionTime = 4;

    private void Update()
    {
        float hour = Clock_Digital.instance.hour + (Clock_Digital.instance.pm ? 12 : 0);
        float minute = ((float)Clock_Digital.instance.minute / 60f);
        float time = hour + minute;

        float sunPosition = 0;
        if (time < sunriseStart || time > (sunsetStart + transitionTime))
            sunPosition = 0;
        else if (time < (sunriseStart + transitionTime))
            sunPosition = Mathf.InverseLerp(sunriseStart, sunriseStart + transitionTime, time);
        else if (time > sunsetStart)
            sunPosition = Mathf.InverseLerp(sunsetStart + transitionTime, sunsetStart, time);
        else
            sunPosition = 1;

        position = Vector3.Lerp(farPosition, closePosition, sunPosition);
        this.transform.position = position;
    }
}
