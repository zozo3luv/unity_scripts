using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

namespace SmoothShakeScript
{
    [RequireComponent(typeof(CinemachineCameraOffset))]
    [RequireComponent(typeof(CinemachineRecomposer))]
    public class SmoothShakeCinemachine : MonoBehaviour
    {
        /*

        Thx for purchasing Smooth Shake 3D

        Made by mardt (also known as daburo)

        Documentation and usage explanation video can be found in the documentation.pdf file

        */
        [HideInInspector] public bool is3D = true;
        [HideInInspector] public bool positionShake, rotationShake, scaleShake;
        [HideInInspector] public CinemachineCameraOffset cinemachinePosOffset;
        [HideInInspector] public CinemachineRecomposer cinemachineRotOffset;
        [HideInInspector] public Vector3 cinemachineCameraPos;
        [HideInInspector] public Vector3 cinemachineCameraRot;

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

        [HideInInspector] public AnimationCurve fadeOutCurve;
        [HideInInspector] public AnimationCurve fadeInCurve;

        [HideInInspector] public bool fovShake = false;
        [HideInInspector] public float fovShakeIntensity = 0.5f;
        [HideInInspector] public float fovShakeFrequency = 5f;

        [HideInInspector] public bool enableOnStart = false;

        private Vector3 startPosition;
        private Vector3 startRotation;
        private bool shaking = false;
        private float shakeTimer;
        private bool fading = false;
        private CinemachineVirtualCamera cvcCam;
        private CinemachineFreeLook cflCam;
        private float startFov;
        [HideInInspector] public bool hasRunOnce;

        private void Awake()
        {
            shaking = false;
            hasRunOnce = true;

            if(GetComponent<CinemachineVirtualCamera>() != null)
            {
                cvcCam = GetComponent<CinemachineVirtualCamera>();
                startFov = cvcCam.m_Lens.FieldOfView;
            }
            if(GetComponent<CinemachineFreeLook>() != null)
            {
                cflCam = GetComponent<CinemachineFreeLook>();
                startFov = cflCam.m_Lens.FieldOfView;
            }

            cinemachinePosOffset = GetComponent<CinemachineCameraOffset>();
            cinemachineRotOffset = GetComponent<CinemachineRecomposer>();
            startPosition = cinemachinePosOffset.m_Offset;
            startRotation = new Vector3(cinemachineRotOffset.m_Tilt, cinemachineRotOffset.m_Pan, cinemachineRotOffset.m_Dutch);

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
            else
            {
                Debug.LogError("no such type to equalize (you shouldn't be seeing this, if you do please contact me at itsmardt@gmail.com to report a bug)");
            }
           
        }

        public void StartShake()
        {
            if (!shaking)
            {
                shaking = true;

                shakeTimer = 0;

                startPosition = cinemachinePosOffset.m_Offset;
                startRotation = new Vector3(cinemachineRotOffset.m_Tilt, cinemachineRotOffset.m_Pan, cinemachineRotOffset.m_Dutch);

                if (GetComponent<CinemachineVirtualCamera>() != null)
                {
                    startFov = cvcCam.m_Lens.FieldOfView;
                }
                if (GetComponent<CinemachineFreeLook>() != null)
                {
                    startFov = cflCam.m_Lens.FieldOfView;
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
                cinemachinePosOffset.m_Offset = startPosition;
                cinemachineRotOffset.m_Tilt = startRotation.x;
                cinemachineRotOffset.m_Pan = startRotation.y;
                cinemachineRotOffset.m_Dutch = startRotation.z;


                if (GetComponent<CinemachineVirtualCamera>() != null)
                {
                    cvcCam.m_Lens.FieldOfView = startFov;
                }
                if (GetComponent<CinemachineFreeLook>() != null)
                {
                    cflCam.m_Lens.FieldOfView = startFov;
                }

                shaking = false;
                StartShake();
            }
        }

        public void StopShake()
        {
            if (shaking && !fadeOut)
            {
                StopAllCoroutines();
                cinemachinePosOffset.m_Offset = startPosition;
                cinemachineRotOffset.m_Tilt = startRotation.x;
                cinemachineRotOffset.m_Pan = startRotation.y;
                cinemachineRotOffset.m_Dutch = startRotation.z;

                if (GetComponent<CinemachineVirtualCamera>() != null)
                {
                    cvcCam.m_Lens.FieldOfView = startFov;
                }
                if (GetComponent<CinemachineFreeLook>() != null)
                {
                    cflCam.m_Lens.FieldOfView = startFov;
                }
                shaking = false;
            }
            else if(shaking && fadeOut && !fading)
            {
                StartCoroutine(FadeOutShake(fadeOutDuration));
            }

        }

        private void Update()
        {
            hasRunOnce = true;

            if (shaking)
            {
                UpdateShake(positionIntensity, rotationIntensity, fovShakeIntensity);
            }
        }

        private void UpdateShake(Vector3 newPositionIntensity, Vector3 newRotationIntensity, float newFovIntensity)
        {
            if (positionShake)
            {
                float x = startPosition.x + Mathf.Sin(Time.time * positionFrequency.x) * newPositionIntensity.x;
                float y = startPosition.y + Mathf.Sin(Time.time * positionFrequency.y) * newPositionIntensity.y;
                if (is3D)
                {
                    float z = startPosition.z + Mathf.Sin(Time.time * positionFrequency.z) * newPositionIntensity.z;
                    cinemachinePosOffset.m_Offset = new Vector3(x, y, z);
                }
                else
                {
                    float z = startPosition.z;
                    cinemachinePosOffset.m_Offset = new Vector3(x, y, z);
                }
            }

            if (rotationShake)
            {
                float rotShakeX = startRotation.x + Mathf.Sin(Time.time * rotationFrequency.x) * newRotationIntensity.x;
                float rotShakeY = startRotation.y + Mathf.Sin(Time.time * rotationFrequency.y) * newRotationIntensity.y;
                float rotShakeZ = startRotation.z + Mathf.Sin(Time.time * rotationFrequency.z) * newRotationIntensity.z;

                if (is3D)
                {

                    cinemachineRotOffset.m_Tilt = rotShakeX;
                    cinemachineRotOffset.m_Pan = rotShakeY;
                    cinemachineRotOffset.m_Dutch = rotShakeZ;
                }
                else
                {
                    cinemachineRotOffset.m_Tilt = startRotation.x;
                    cinemachineRotOffset.m_Pan = startRotation.y;
                    cinemachineRotOffset.m_Dutch = rotShakeZ;
                }
            }

            if (fovShake)
            {
                if (GetComponent<CinemachineVirtualCamera>() != null)
                {
                    cvcCam.m_Lens.FieldOfView = startFov + Mathf.Sin(Time.time * fovShakeFrequency) * newFovIntensity;
                }
                if (GetComponent<CinemachineFreeLook>() != null)
                {
                    cflCam.m_Lens.FieldOfView = startFov + Mathf.Sin(Time.time * fovShakeFrequency) * newFovIntensity;
                }
            }



        }

        private IEnumerator FadeInShake(float duration)
        {
            Vector3 newPosIntensity = positionIntensity;
            Vector3 newRotIntensity = rotationIntensity;
            float newFovIntensity = fovShakeIntensity;

            shakeTimer += Time.deltaTime;

            for (float t = 0; t < duration; t += Time.deltaTime)
            {
                newPosIntensity = Vector3.Lerp(Vector3.zero, positionIntensity,fadeInCurve.Evaluate(t / duration));
                newRotIntensity = Vector3.Lerp(Vector3.zero, rotationIntensity, fadeInCurve.Evaluate(t / duration));
                newFovIntensity = Mathf.Lerp(0, fovShakeIntensity, fadeInCurve.Evaluate(t / duration));
                UpdateShake(newPosIntensity, newRotIntensity, newFovIntensity);

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
            float newFovIntensity = fovShakeIntensity;

            for (float t = 0; t < duration; t += Time.deltaTime)
            {
                newPosIntensity = Vector3.Lerp(positionIntensity, Vector3.zero, fadeOutCurve.Evaluate(t / duration));
                newRotIntensity = Vector3.Lerp(rotationIntensity, Vector3.zero, fadeOutCurve.Evaluate(t / duration));
                newFovIntensity = Mathf.Lerp(fovShakeIntensity, 0, fadeOutCurve.Evaluate(t / duration));
                UpdateShake(newPosIntensity, newRotIntensity, newFovIntensity);

                yield return null;
            }

            cinemachinePosOffset.m_Offset = startPosition;
            cinemachineRotOffset.m_Tilt = startRotation.x;
            cinemachineRotOffset.m_Pan = startRotation.y;
            cinemachineRotOffset.m_Dutch = startRotation.z;

            if (GetComponent<CinemachineVirtualCamera>() != null)
            {
                cvcCam.m_Lens.FieldOfView = startFov;
            }
            if (GetComponent<CinemachineFreeLook>() != null)
            {
                cflCam.m_Lens.FieldOfView = startFov;
            }

            shaking = false;

            fading = false;

            yield return null;
        }

        IEnumerator ShakeDurationTimer(float duration)
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


