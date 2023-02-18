using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    private const float MaxVelocityChange = 5f;
    private const float MaxAngularVelocityChange = 10f;
    private const float VelocityMagic = 3000f;
    private const float AngularVelocityMagic = 25f;

    private Vector3 initialPosition;
    private Quaternion initialRotation;
    private Rigidbody Rigidbody;

    private float NewtonVRExpectedDeltaTime = 0.011f;

    private bool initialized = false;

    public bool beingInteractedWith = false;

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

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.I))
            this.transform.Translate(1, 1, 1);
        if (Input.GetKeyUp(KeyCode.L))
            this.transform.Rotate(0, 90, 0);
    }

    private void FixedUpdate()
    {
        if (initialized)
        {
            UpdateVelocities();
        }
    }

    public void AddExternalVelocity(Vector3 velocity)
    {
        this.Rigidbody.velocity = Vector3.Lerp(this.Rigidbody.velocity, velocity, 0.5f);
    }

    public void AddExternalAngularVelocity(Vector3 angularVelocity)
    {
        this.Rigidbody.angularVelocity = Vector3.Lerp(this.Rigidbody.angularVelocity, angularVelocity, 0.5f);
    }

    private float timeLastStill;

    private float maxDistanceDelta = 0.1f;
    private float maxAngleDelta = 10f;
    public void UpdateVelocities()
    {
        if (beingInteractedWith)
            return;
        bool still = false;

        float positionDifference = Vector3.Distance(this.transform.position, initialPosition);
        if (positionDifference > 0.001f)
        {
            this.transform.position = Vector3.MoveTowards(this.transform.position, initialPosition, maxDistanceDelta);
            if (this.Rigidbody.isKinematic == false)
                this.Rigidbody.velocity /= 2;
            still = false;
        }
        else
        {
            if (this.Rigidbody.isKinematic == false)
                this.Rigidbody.velocity = Vector3.zero;
            still = true;
        }


        float deltaAngle = Quaternion.Angle(initialRotation, this.transform.rotation);

        if (deltaAngle > 1 || deltaAngle < -1)
        {
            this.transform.rotation = Quaternion.RotateTowards(this.transform.rotation, initialRotation, maxAngleDelta);
            if (this.Rigidbody.isKinematic == false)
                this.Rigidbody.angularVelocity /= 2;
            still = false;

        }
        else
        {
            if (this.Rigidbody.isKinematic == false)
                this.Rigidbody.angularVelocity = Vector3.zero;
            still = still && true;
        }

        if (still == true)
        {
            timeLastStill = Time.time;

            if (this.Rigidbody.isKinematic)
                this.Rigidbody.isKinematic = false;
        }
        
        if (still == false && Time.time - timeLastStill > maxTimeToRevert)
        {
            //trying to get back but not there yet. turn it kinematic so it can zip.
            this.Rigidbody.isKinematic = true;
        }
    }
    private float maxTimeToRevert = 2;
}
