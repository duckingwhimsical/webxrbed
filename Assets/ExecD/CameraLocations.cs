using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ExecD
{
    public class CameraLocations : MonoBehaviour
    {
        public static CameraLocations instance;

        public Transform NoVR;
        public Transform OnBed;

        void OnEnable()
        {
            instance = this;
        }
    }
}