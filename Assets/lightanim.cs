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

    private void Update()
    {
        float lerp = 0.5f * Mathf.Sin(Time.time * speed) + 0.5f;

        position = Vector3.Lerp(closePosition, farPosition, lerp);

        this.transform.position = position;
    }
}
