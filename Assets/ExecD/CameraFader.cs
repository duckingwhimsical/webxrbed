using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WebXR;

namespace ExecD
{
    public class CameraFader : MonoBehaviour
    {
        public Camera CurrentCamera;
        public SphereCollider StayInsideCollider;

        private bool lastInsideArea = true;

        private float radius;

        public GameObject BedOnlyParent;
        public GameObject NormalParent;

        public Transform FadeTransform;
        private Material FadeMaterial;

        public Vector3 FadeTransformOffset = new Vector3(0, 0, 0.11f);

        private Coroutine fadeCoroutine;

        private bool stuffEnabled = false;
        private WebXRCamera webXRCamera;

        private void Start()
        {
            radius = StayInsideCollider.transform.localScale.x * StayInsideCollider.radius;
            FadeMaterial = FadeTransform.gameObject.GetComponent<MeshRenderer>().material;
            FadeTransform.gameObject.SetActive(false);
            webXRCamera = this.gameObject.GetComponent<WebXRCamera>();
        }

        void Update()
        {
            DebugCrap();

            if (stuffEnabled == false)
                return;

            bool currentlyInside = IsInsideArea();
            if (currentlyInside != lastInsideArea)
            {
                FadeBedOnly(currentlyInside);
            }
        }

        private void FadeBedOnly(bool insideArea)
        {
            if (fadeCoroutine == null)
            {
                if (insideArea)
                    fadeCoroutine = StartCoroutine(DoFadeBedOnlyOut());
                else
                    fadeCoroutine = StartCoroutine(DoFadeBedOnlyIn());
            }
        }

        private IEnumerator FadeBlack(float fromAlpha, float toAlpha, float overTime)
        {
            FadeTransform.position = CurrentCamera.transform.TransformPoint(FadeTransformOffset);
            FadeTransform.LookAt(CurrentCamera.transform.position + CurrentCamera.transform.forward);
            FadeTransform.parent = CurrentCamera.transform;

            float startTime = Time.time;
            float endTime = startTime + overTime;
            Color color = FadeMaterial.color;

            while (Time.time < endTime)
            {
                float lerp = (Time.time - startTime) / overTime;
                float alpha = Mathf.Lerp(fromAlpha, toAlpha, lerp);
                color.a = alpha;
                FadeMaterial.color = color;

                yield return null;
            }
            yield return null;

            color.a = toAlpha;
            FadeMaterial.color = color;
        }

        private float fadeToBlackTime = 0.5f;
        private float fadeToClearTime = 0.5f;

        private IEnumerator DoFadeBedOnlyOut()
        {
            FadeTransform.gameObject.SetActive(true);
            yield return FadeBlack(0, 1, fadeToBlackTime);
            NormalParent.SetActive(true);
            BedOnlyParent.SetActive(false);
            yield return FadeBlack(1, 0, fadeToClearTime);
            FadeTransform.gameObject.SetActive(false);

            lastInsideArea = true;
            fadeCoroutine = null;
        }

        private IEnumerator DoFadeBedOnlyIn()
        {
            FadeTransform.gameObject.SetActive(true);
            yield return FadeBlack(0, 1, fadeToBlackTime);
            NormalParent.SetActive(false);
            BedOnlyParent.SetActive(true);
            yield return FadeBlack(1, 0, fadeToClearTime);
            FadeTransform.gameObject.SetActive(false);

            //todo: fuck with the sound

            lastInsideArea = false;
            fadeCoroutine = null;
        }

        bool IsInsideArea()
        {
            float distance = Vector3.Distance(CurrentCamera.transform.position, StayInsideCollider.transform.position);
            return distance <= radius;
        }



        private void OnEnable()
        {
            WebXRManager.OnXRChange += OnXRChange;
        }

        private void OnDisable()
        {
            WebXRManager.OnXRChange -= OnXRChange;
        }

        private void OnXRChange(WebXRState xrState, int viewsCount, Rect leftRect, Rect rightRect)
        {
            switch (xrState)
            {
                case WebXRState.AR:
                    stuffEnabled = true;
                    CurrentCamera = webXRCamera.GetCamera(WebXRCamera.CameraID.RightAR);
                    break;
                case WebXRState.VR:
                    stuffEnabled = true;
                    CurrentCamera = webXRCamera.GetCamera(WebXRCamera.CameraID.RightVR);
                    break;
                default:
                    stuffEnabled = false;
                    CurrentCamera = webXRCamera.GetCamera(WebXRCamera.CameraID.Main);
                    break;
            }
        }

        private void DebugCrap()
        {
            if (Input.GetKeyUp(KeyCode.Alpha1))
            {
                CurrentCamera.transform.position = CameraLocations.instance.OnBed.position;
                CurrentCamera.transform.rotation = CameraLocations.instance.OnBed.rotation;
            }
            if (Input.GetKeyUp(KeyCode.Alpha2))
            {
                CurrentCamera.transform.position = CameraLocations.instance.NoVR.position;
                CurrentCamera.transform.rotation = CameraLocations.instance.NoVR.rotation;
            }
            if (Input.GetKeyUp(KeyCode.Alpha0))
            {
                stuffEnabled = true;
            }
        }
    }
}