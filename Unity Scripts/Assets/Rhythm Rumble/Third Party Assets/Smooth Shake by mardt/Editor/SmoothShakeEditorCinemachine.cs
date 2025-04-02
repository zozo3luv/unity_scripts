using UnityEngine;
using UnityEditor;
using Cinemachine;

namespace SmoothShakeScript
{
    [CustomEditor(typeof(SmoothShakeCinemachine))]
    [CanEditMultipleObjects]
    public class SmoothShakeEditorCinemachine : Editor
    {
        SerializedProperty is3D;
        SerializedProperty positionShake, rotationShake;
        SerializedProperty constantShake;
        SerializedProperty fadeIn, fadeOut;
        SerializedProperty useExtraShakeLength;
        SerializedProperty proportionalPositionIntensity;
        SerializedProperty proportionalRotationIntensity;
        SerializedProperty fadeOutCurve;
        SerializedProperty fadeInCurve;
        SerializedProperty fovShake;
        SerializedProperty enableOnStart;

        private void OnEnable()
        {
            is3D = serializedObject.FindProperty("is3D");
            positionShake = serializedObject.FindProperty("positionShake");
            rotationShake = serializedObject.FindProperty("rotationShake");
            constantShake = serializedObject.FindProperty("constantShake");
            fadeIn = serializedObject.FindProperty("fadeIn");
            fadeOut = serializedObject.FindProperty("fadeOut");
            useExtraShakeLength = serializedObject.FindProperty("useExtraShakeLength");
            proportionalPositionIntensity = serializedObject.FindProperty("proportionalPositionIntensity");
            proportionalRotationIntensity = serializedObject.FindProperty("proportionalRotationIntensity");
            fadeOutCurve = serializedObject.FindProperty("fadeOutCurve");
            fadeInCurve = serializedObject.FindProperty("fadeInCurve");
            fovShake = serializedObject.FindProperty("fovShake");
            enableOnStart = serializedObject.FindProperty("enableOnStart");
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            SmoothShakeCinemachine smoothShakeCinemachine = (SmoothShakeCinemachine)target;

            EditorGUILayout.LabelField("Main Settings", EditorStyles.boldLabel);

            EditorGUILayout.PropertyField(is3D, new GUIContent("Enable 3D"), GUILayout.Height(20));
            is3D.serializedObject.ApplyModifiedProperties();
            EditorGUILayout.PropertyField(positionShake, new GUIContent("Enable Position Shake"), GUILayout.Height(20));
            positionShake.serializedObject.ApplyModifiedProperties();
            EditorGUILayout.PropertyField(rotationShake, new GUIContent("Enable Rotation Shake"), GUILayout.Height(20));
            rotationShake.serializedObject.ApplyModifiedProperties();

            EditorGUILayout.LabelField("", GUILayout.Height(10));
            EditorGUILayout.LabelField("Shake Settings", EditorStyles.boldLabel, GUILayout.Height(20));

            EditorGUILayout.PropertyField(constantShake, new GUIContent("Enable Constant Shake"), GUILayout.Height(20));
            constantShake.serializedObject.ApplyModifiedProperties();

            EditorGUILayout.LabelField("", GUILayout.Height(10));
            EditorGUILayout.LabelField("Settings for fading in / out", EditorStyles.miniLabel, GUILayout.Height(20));

            EditorGUILayout.PropertyField(fadeIn, new GUIContent("Enable Fade In"), GUILayout.Height(20));
            fadeIn.serializedObject.ApplyModifiedProperties();
            if (smoothShakeCinemachine.fadeIn)
            {
                smoothShakeCinemachine.fadeInDuration = EditorGUILayout.FloatField("Fade In Duration", smoothShakeCinemachine.fadeInDuration, GUILayout.Height(20));
                EditorGUILayout.PropertyField(fadeInCurve, new GUIContent("Fade In Curve"), GUILayout.Height(20));
                fadeInCurve.serializedObject.ApplyModifiedProperties();
                EditorGUILayout.LabelField("", GUILayout.Height(5));
            }
            EditorGUILayout.PropertyField(fadeOut, new GUIContent("Enable Fade Out"), GUILayout.Height(20));
            fadeOut.serializedObject.ApplyModifiedProperties();
            if (smoothShakeCinemachine.fadeOut)
            {
                smoothShakeCinemachine.fadeOutDuration = EditorGUILayout.FloatField("Fade Out Duration", smoothShakeCinemachine.fadeOutDuration, GUILayout.Height(20));
                EditorGUILayout.PropertyField(fadeOutCurve, new GUIContent("Fade Out Curve"), GUILayout.Height(20));
                fadeOutCurve.serializedObject.ApplyModifiedProperties();
                EditorGUILayout.LabelField("", GUILayout.Height(5));
            }
            EditorGUILayout.PropertyField(useExtraShakeLength, new GUIContent("Use Extra Shake Length"), GUILayout.Height(20));
            useExtraShakeLength.serializedObject.ApplyModifiedProperties();
            if (smoothShakeCinemachine.useExtraShakeLength)
            {
                smoothShakeCinemachine.additionalShakeDuration = EditorGUILayout.FloatField("Additional Shake Duration", smoothShakeCinemachine.additionalShakeDuration, GUILayout.Height(20));
                EditorGUILayout.LabelField("", GUILayout.Height(5));
            }

            if (smoothShakeCinemachine.positionShake)
            {
                EditorGUILayout.LabelField("", GUILayout.Height(10));
                EditorGUILayout.LabelField("Position Shake Settings", EditorStyles.boldLabel, GUILayout.Height(20));

                EditorGUILayout.PropertyField(proportionalPositionIntensity, new GUIContent("Proportional Intensity"), GUILayout.Height(20));
                proportionalPositionIntensity.serializedObject.ApplyModifiedProperties();
                if (smoothShakeCinemachine.proportionalPositionIntensity)
                {
                    smoothShakeCinemachine.equalPositionintensity = EditorGUILayout.FloatField("Position Intensity", smoothShakeCinemachine.equalPositionintensity);
                    if (GUI.changed)
                    {
                        smoothShakeCinemachine.SetEqualIntensity("positionIntensity");
                    }
                }
                else if (!smoothShakeCinemachine.proportionalPositionIntensity)
                {
                    if (smoothShakeCinemachine.is3D)
                    {
                        smoothShakeCinemachine.positionIntensity = EditorGUILayout.Vector3Field("Position Intensity", smoothShakeCinemachine.positionIntensity);
                    }
                    else if (!smoothShakeCinemachine.is3D)
                    {
                        smoothShakeCinemachine.positionIntensity = EditorGUILayout.Vector2Field("Position Intensity", smoothShakeCinemachine.positionIntensity);
                    }
                }

                if (smoothShakeCinemachine.is3D)
                {
                    smoothShakeCinemachine.positionFrequency = EditorGUILayout.Vector3Field("Position Frequency", smoothShakeCinemachine.positionFrequency);
                }
                else if (!smoothShakeCinemachine.is3D)
                {
                    smoothShakeCinemachine.positionFrequency = EditorGUILayout.Vector2Field("Position Frequency", smoothShakeCinemachine.positionFrequency);
                }
            }

            if (smoothShakeCinemachine.rotationShake)
            {
                EditorGUILayout.LabelField("", GUILayout.Height(10));
                EditorGUILayout.LabelField("Rotation Shake Settings", EditorStyles.boldLabel, GUILayout.Height(20));

                if (smoothShakeCinemachine.is3D)
                {
                    EditorGUILayout.PropertyField(proportionalRotationIntensity, new GUIContent("Proportional Intensity"), GUILayout.Height(20));
                    proportionalRotationIntensity.serializedObject.ApplyModifiedProperties();
                    if (smoothShakeCinemachine.proportionalRotationIntensity)
                    {
                        smoothShakeCinemachine.equalRotationintensity = EditorGUILayout.FloatField("Rotation Intensity", smoothShakeCinemachine.equalRotationintensity);
                        if (GUI.changed)
                        {
                            smoothShakeCinemachine.SetEqualIntensity("rotationIntensity");

                        }

                    }
                    else if (!smoothShakeCinemachine.proportionalRotationIntensity)
                    {
                        if (smoothShakeCinemachine.is3D)
                        {
                            smoothShakeCinemachine.rotationIntensity = EditorGUILayout.Vector3Field("Rotation Intensity", smoothShakeCinemachine.rotationIntensity);
                        }

                    }

                    smoothShakeCinemachine.rotationFrequency = EditorGUILayout.Vector3Field("Rotation Frequency", smoothShakeCinemachine.rotationFrequency);
                }
                else if (!smoothShakeCinemachine.is3D)
                {
                    smoothShakeCinemachine.rotationIntensity.z = EditorGUILayout.FloatField("Rotation Intensity", smoothShakeCinemachine.rotationIntensity.z);
                    smoothShakeCinemachine.rotationFrequency.z = EditorGUILayout.FloatField("Rotation Frequency", smoothShakeCinemachine.rotationFrequency.z);
                }


            }

            if (smoothShakeCinemachine.GetComponent<CinemachineVirtualCamera>() != null || smoothShakeCinemachine.GetComponent<CinemachineFreeLook>() != null)
            {
                EditorGUILayout.LabelField("", GUILayout.Height(10));
                EditorGUILayout.LabelField("Field of view shake settings (only for 3D cameras)", EditorStyles.miniBoldLabel, GUILayout.Height(20));

                EditorGUILayout.PropertyField(fovShake, new GUIContent("Enable FOV Shake"), GUILayout.Height(20));
                fovShake.serializedObject.ApplyModifiedProperties();

                if (smoothShakeCinemachine.fovShake)
                {
                    smoothShakeCinemachine.fovShakeIntensity = EditorGUILayout.FloatField("FOV Shake Intensity", smoothShakeCinemachine.fovShakeIntensity);
                    smoothShakeCinemachine.fovShakeFrequency = EditorGUILayout.FloatField("FOV Shake Frequency", smoothShakeCinemachine.fovShakeFrequency);
                }
            }

            EditorGUILayout.LabelField("", GUILayout.Height(10));
            EditorGUILayout.PropertyField(enableOnStart, new GUIContent("Enable Shake On Start"), GUILayout.Height(20));
            enableOnStart.serializedObject.ApplyModifiedProperties();

            EditorGUILayout.LabelField("", GUILayout.Height(10));

            GUILayout.BeginHorizontal();

            if (GUILayout.Button("Test Shake"))
            {
                if (smoothShakeCinemachine.hasRunOnce)
                {
                    smoothShakeCinemachine.StartShake();
                }

            }

            if (GUILayout.Button("Stop Test Shake"))
            {
                if (smoothShakeCinemachine.hasRunOnce)
                {
                    smoothShakeCinemachine.StopShake();
                }
            }

            GUILayout.EndHorizontal();
        }
    }


}



