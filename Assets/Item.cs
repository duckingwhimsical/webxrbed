using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    private float MaxVelocityChange = 5f;
    private float MaxAngularVelocityChange = 10f;
    private float VelocityMagic = 2000f;
    private float AngularVelocityMagic = 10f;

    private Vector3 initialPosition;
    private Quaternion initialRotation;
    private Rigidbody Rigidbody;

    private float NewtonVRExpectedDeltaTime = 0.011f;

    private bool initialized = false;

    private IEnumerator Start()
    {
        this.Rigidbody = this.GetComponent<Rigidbody>();

        do
        {//wait till its asleep to get initial positions
            yield return null;
            Debug.Log("waiting till sleep");
        } while (!this.Rigidbody.IsSleeping());

        this.initialPosition = this.transform.position;
        this.initialRotation = this.transform.rotation;
        initialized = true;
    }

    private void FixedUpdate()
    {
        if (initialized)
        {
            UpdateVelocities();
        }
    }

    protected virtual void UpdateVelocities()
    {
        Vector3 targetItemPosition = this.Rigidbody.position;
        Quaternion targetItemRotation = this.Rigidbody.rotation;

        Vector3 targetHandPosition = initialPosition;
        Quaternion targetHandRotation = initialRotation;


        float velocityMagic = VelocityMagic / (Time.deltaTime / NewtonVRExpectedDeltaTime);
        float angularVelocityMagic = AngularVelocityMagic / (Time.deltaTime / NewtonVRExpectedDeltaTime);

        Vector3 positionDelta;
        Quaternion rotationDelta;

        float angle;
        Vector3 axis;

        positionDelta = (targetHandPosition - targetItemPosition);
        rotationDelta = targetHandRotation * Quaternion.Inverse(targetItemRotation);

        if (positionDelta.sqrMagnitude > 0.01f)
        {
            Vector3 velocityTarget = (positionDelta * velocityMagic) * Time.deltaTime;
            if (float.IsNaN(velocityTarget.x) == false)
            {
                this.Rigidbody.velocity = Vector3.MoveTowards(this.Rigidbody.velocity, velocityTarget, MaxVelocityChange);
                Debug.Log("Moving", this.gameObject);
            }
        }

        rotationDelta.ToAngleAxis(out angle, out axis);

        if (angle > 180)
            angle -= 360;

        if (angle != 0)
        {
            Vector3 angularTarget = angle * axis;
            if (float.IsNaN(angularTarget.x) == false && angularTarget.sqrMagnitude > 0.01f)
            {
                angularTarget = (angularTarget * angularVelocityMagic) * Time.deltaTime;
                this.Rigidbody.angularVelocity = Vector3.MoveTowards(this.Rigidbody.angularVelocity, angularTarget, MaxAngularVelocityChange);
                Debug.Log("Rotating", this.gameObject);
            }
        }
    }
}
