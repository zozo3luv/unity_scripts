using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace SmoothShakeScript
{
    public class SmoothShake : MonoBehaviour
    {
        /*

        Thx for purchasing Smooth Shake 3D

        Made by mardt (also known as daburo)

        Documentation and usage explanation video can be found in the documentation.pdf file

        */
        [HideInInspector] public bool is3D = true;
        [HideInInspector] public bool positionShake, rotationShake, scaleShake;

        [HideInInspector] public bool constantShake = false;
        [HideInInspector] public bool fadeIn = false;
        [HideInInspector] public bool fadeOut = true;
        [HideInInspector] public float fadeOutDuration = 1f;
        [HideInInspector] public float fadeInDuration = 1f;
        [Tooltip("The amount of time you want the camera to shake before / after fading")]
        [HideInInspector] public float additionalShakeDuration = 0f;
        [HideInInspector] public bool useExtraShakeLength = false;
        [HideInInspector] public bool proportionalPositionIntensity = true;
        [HideInInspector] public Vector3 positionIntensity = new Vector3(1, 1, 1);
        [HideInInspector] public float equalPositionintensity = 1;
        [HideInInspector] public Vector3 positionFrequency = new Vector3(52, 42, 0);
        [Tooltip("Generally between 0 and 1")]
        [HideInInspector] public Vector3 rotationIntensity = new Vector3(0.1f,0.1f,0.1f);
        [HideInInspector] public float equalRotationintensity = 0.1f;
        [HideInInspector] public bool proportionalRotationIntensity = true;
        [HideInInspector] public Vector3 rotationFrequency = new Vector3(23, 25, 27);
        [HideInInspector] public bool proportionalScaleIntensity = true;
        [HideInInspector] public Vector3 scaleIntensity = new Vector3(0.1f,0.1f,0.1f);
        [HideInInspector] public float equalScaleIntensity = 0.1f;
        [HideInInspector] public Vector3 scaleFrequency = new Vector3(23f, 25f, 27f);
        [HideInInspector] public AnimationCurve fadeOutCurve;
        [HideInInspector] public AnimationCurve fadeInCurve;
        [HideInInspector] public bool fovShake = false;
        [HideInInspector] public float fovShakeIntensity = 0.5f;
        [HideInInspector] public float fovShakeFrequency = 5f;
        [HideInInspector] public bool enableOnStart = false;

        private Vector3 startPosition;
        private Vector3 startRotation;
        private Vector3 startScale;
        private float startFov;
        private bool shaking = false;
        private float shakeTimer;
        private bool fading = false;
        private Camera cam;
        [HideInInspector] public bool hasRunOnce = false;

        private void Awake()
        {
            shaking = false;
            hasRunOnce = true;

            if (GetComponent<Camera>() != null)
            {
                cam = GetComponent<Camera>();
                startFov = cam.fieldOfView;
            }

            startPosition = transform.localPosition;
            startRotation = transform.localEulerAngles;
            startScale = transform.localScale;

        }

        private void Start()
        {
            if (enableOnStart)
            {
                StartShake();
            }
        }

        public void SetEqualIntensity(string type)
        {
            if(type == "positionIntensity")
            {
                positionIntensity = new Vector3(equalPositionintensity, equalPositionintensity, equalPositionintensity);
            }
            else if(type == "rotationIntensity")
            {
                rotationIntensity = new Vector3(equalRotationintensity, equalRotationintensity, equalRotationintensity);
            }
            else if(type == "scaleIntensity")
            {
                scaleIntensity = new Vector3(equalScaleIntensity, equalScaleIntensity, equalScaleIntensity);
            }
            else
            {
                Debug.LogError("no such type to equalize (the options are positionIntensity, rotationIntensity or scaleIntensity)");
            }
           
        }

        public void SetCustomIntensity(float newIntensity, string type)
        {
            if (type == "positionIntensity")
            {
                positionIntensity = new Vector3(newIntensity, newIntensity, newIntensity);
            }
            else if (type == "rotationIntensity")
            {
                rotationIntensity = new Vector3(newIntensity, newIntensity, newIntensity);
            }
            else if (type == "scaleIntensity")
            {
                scaleIntensity = new Vector3(newIntensity, newIntensity, newIntensity);
            }
            else
            {
                Debug.LogError("no such type to equalize (the options are positionIntensity, rotationIntensity or scaleIntensity)");
            }

        }

        public void StartShake()
        {
            if (!shaking)
            {
                shaking = true;

                shakeTimer = 0;

                startPosition = transform.localPosition;
                startRotation = transform.localEulerAngles;
                startScale = transform.localScale;

                if (GetComponent<Camera>() != null)
                {
                    startFov = cam.fieldOfView;
                }

                if (!positionShake && !rotationShake && !scaleShake)
                {
                    Debug.LogError("No shake activated. Enable at least position, rotation or scale shake");
                }

                if (!constantShake)
                {
                    if (fadeOut && !fadeIn)
                    {
                        StartCoroutine(ShakeDurationTimer(additionalShakeDuration));
                    }
                    else if (fadeIn && !fadeOut)
                    {
                        StartCoroutine(FadeInShake(fadeInDuration));
                    }
                    else if (fadeIn && fadeOut)
                    {
                        StartCoroutine(FadeInShake(fadeInDuration));
                    }
                    else if (!fadeIn && !fadeOut)
                    {
                        Debug.LogError("No shake activated. You need to at least activate a constant shake or a fade in / fade out");
                    }
                    else
                    {
                        Debug.LogError("No shake activated. Try checking / unchecking fade in or fade out");
                    }
                }

                if (constantShake)
                {
                    if (fadeIn)
                    {
                        StartCoroutine(FadeInShake(fadeInDuration));
                    }
                    else
                    {
                        shaking = true;
                    }
                }
            }
            else if (shaking && !constantShake)
            {
                StopAllCoroutines();
                transform.localPosition = startPosition;
                transform.localEulerAngles = startRotation;
                transform.localScale = startScale;

                if (GetComponent<Camera>() != null)
                {
                    cam.fieldOfView = startFov;
                }

                shaking = false;
                StartShake();
            }
        }

        public void StopShake()
        {
            if (shaking)
            {
                if (!fadeOut)
                {
                    StopAllCoroutines();
                    transform.localPosition = startPosition;
                    transform.localEulerAngles = startRotation;
                    transform.localScale = startScale;

                    if (GetComponent<Camera>() != null)
                    {
                        cam.fieldOfView = startFov;
                    }

                    shaking = false;
                }
                else if (fadeOut && !fading)
                {
                    StartCoroutine(FadeOutShake(fadeOutDuration));
                }
            }


        }

        private void Update()
        {
            hasRunOnce = true;

            if (shaking)
            {
                UpdateShake(positionIntensity, rotationIntensity, scaleIntensity, fovShakeIntensity);
            }
        }

        private void UpdateShake(Vector3 newPositionIntensity, Vector3 newRotationIntensity, Vector3 newScaleIntensity, float newFovIntensity)
        {
            if (positionShake)
            {
                float x = startPosition.x + Mathf.Sin(Time.time * positionFrequency.x) * newPositionIntensity.x;
                float y = startPosition.y + Mathf.Sin(Time.time * positionFrequency.y) * newPositionIntensity.y;
                if (is3D)
                {
                    float z = startPosition.z + Mathf.Sin(Time.time * positionFrequency.z) * newPositionIntensity.z;
                    transform.localPosition = new Vector3(x, y, z);
                }
                else
                {
                    float z = startPosition.z;
                    transform.localPosition = new Vector3(x, y, z);
                }
            }

            if (rotationShake)
            {

                if (is3D)
                {
                    float rotShakeX = startRotation.x + Mathf.Sin(Time.time * rotationFrequency.x) * newRotationIntensity.x;
                    float rotShakeZ = startRotation.z + Mathf.Sin(Time.time * rotationFrequency.z) * newRotationIntensity.z;
                    float rotShakeY = startRotation.y + Mathf.Sin(Time.time * rotationFrequency.y) * newRotationIntensity.y;
                    transform.localRotation = Quaternion.Euler(rotShakeX, rotShakeY, rotShakeZ);

                }
                else
                {
                    float rotShakeZ = startRotation.z + Mathf.Sin(Time.time * rotationFrequency.y) * newRotationIntensity.y;
                    transform.localRotation = Quaternion.Euler(startRotation.x, startRotation.y, rotShakeZ);
                }
            }

            if (scaleShake)
            {

                float x = startScale.x + Mathf.Sin(Time.time * scaleFrequency.x) * newScaleIntensity.x;
                float y = startScale.y + Mathf.Sin(Time.time * scaleFrequency.y) * newScaleIntensity.y;
                if (is3D)
                {
                    float z = startScale.z + Mathf.Sin(Time.time * scaleFrequency.z) * newScaleIntensity.z;
                    transform.localScale = new Vector3(x, y, z);

                }
                else
                {
                    transform.localScale = new Vector3(x, y, startScale.z);
                }

            }

            if (fovShake)
            {
                if (GetComponent<Camera>() != null)
                {
                    cam.fieldOfView = startFov + Mathf.Sin(Time.time * fovShakeFrequency) * newFovIntensity;
                }
            }
 

        }

        private IEnumerator FadeInShake(float duration)
        {
            Vector3 newPosIntensity = positionIntensity;
            Vector3 newRotIntensity = rotationIntensity;
            Vector3 newScaleIntensity = scaleIntensity;
            float newFovIntensity = fovShakeIntensity;

            shakeTimer += Time.deltaTime;

            for (float t = 0; t < duration; t += Time.deltaTime)
            {
                newPosIntensity = Vector3.Lerp(Vector3.zero, positionIntensity,fadeInCurve.Evaluate(t / duration));
                newRotIntensity = Vector3.Lerp(Vector3.zero, rotationIntensity, fadeInCurve.Evaluate(t / duration));
                newScaleIntensity = Vector3.Lerp(Vector3.zero, scaleIntensity, fadeInCurve.Evaluate(t / duration));
                newFovIntensity = Mathf.Lerp(0, fovShakeIntensity, fadeInCurve.Evaluate(t / duration));
                UpdateShake(newPosIntensity, newRotIntensity, newScaleIntensity, newFovIntensity);

                yield return null;
            }

            shaking = true;
            StartCoroutine(ShakeDurationTimer(additionalShakeDuration));

            yield return null;
        }

        private IEnumerator FadeOutShake(float duration)
        {
            fading = true;

            Vector3 newPosIntensity = positionIntensity;
            Vector3 newRotIntensity = rotationIntensity;
            Vector3 newScaleIntensity = scaleIntensity;
            float newFovIntensity = fovShakeIntensity;

            for (float t = 0; t < duration; t += Time.deltaTime)
            {
                newPosIntensity = Vector3.Lerp(positionIntensity, Vector3.zero, fadeOutCurve.Evaluate(t / duration));
                newRotIntensity = Vector3.Lerp(rotationIntensity, Vector3.zero, fadeOutCurve.Evaluate(t / duration));
                newScaleIntensity = Vector3.Lerp(scaleIntensity, Vector3.zero, fadeOutCurve.Evaluate(t / duration));
                newFovIntensity = Mathf.Lerp(fovShakeIntensity, 0, fadeOutCurve.Evaluate(t / duration));
                UpdateShake(newPosIntensity, newRotIntensity, newScaleIntensity, newFovIntensity);

                yield return null;
            }

            transform.localPosition = startPosition;
            transform.localEulerAngles = startRotation;
            transform.localScale = startScale;

            if (GetComponent<Camera>() != null)
            {
                cam.fieldOfView = startFov;
            }

            shaking = false;
            fading = false;

            yield return null;
        }

        private IEnumerator ShakeDurationTimer(float duration)
        {
            if (useExtraShakeLength)
            {
                yield return new WaitForSeconds(duration);
            }

            shaking = true;
            if (!constantShake)
            {
                if (fadeOut)
                {
                    StartCoroutine(FadeOutShake(fadeOutDuration));
                }
                if (!fadeOut)
                {
                    StopShake();
                }
            }

            yield return null;

        }
    }
}

