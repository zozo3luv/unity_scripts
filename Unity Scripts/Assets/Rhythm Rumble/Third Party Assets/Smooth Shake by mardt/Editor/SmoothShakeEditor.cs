using UnityEngine;
using UnityEditor;

namespace SmoothShakeScript
{
    [CustomEditor(typeof(SmoothShake))]
    [CanEditMultipleObjects]
    public class SmoothShakeEditor : Editor
    {
        SerializedProperty is3D;
        SerializedProperty positionShake, rotationShake, scaleShake;
        SerializedProperty constantShake;
        SerializedProperty fadeIn, fadeOut;
        SerializedProperty useExtraShakeLength;
        SerializedProperty proportionalPositionIntensity;
        SerializedProperty proportionalRotationIntensity;
        SerializedProperty proportionalScaleIntensity;
        SerializedProperty fadeOutCurve;
        SerializedProperty fadeInCurve;
        SerializedProperty fovShake;
        SerializedProperty enableOnStart;

        private void OnEnable()
        {
            is3D = serializedObject.FindProperty("is3D");
            positionShake = serializedObject.FindProperty("positionShake");
            rotationShake = serializedObject.FindProperty("rotationShake");
            scaleShake = serializedObject.FindProperty("scaleShake");
            constantShake = serializedObject.FindProperty("constantShake");
            fadeIn = serializedObject.FindProperty("fadeIn");
            fadeOut = serializedObject.FindProperty("fadeOut");
            useExtraShakeLength = serializedObject.FindProperty("useExtraShakeLength");
            proportionalPositionIntensity = serializedObject.FindProperty("proportionalPositionIntensity");
            proportionalRotationIntensity = serializedObject.FindProperty("proportionalRotationIntensity");
            proportionalScaleIntensity = serializedObject.FindProperty("proportionalScaleIntensity");
            fadeOutCurve = serializedObject.FindProperty("fadeOutCurve");
            fadeInCurve = serializedObject.FindProperty("fadeInCurve");
            fovShake = serializedObject.FindProperty("fovShake");
            enableOnStart = serializedObject.FindProperty("enableOnStart");
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            SmoothShake smoothShake = (SmoothShake)target;

            EditorGUILayout.LabelField("Main Settings", EditorStyles.boldLabel);

            EditorGUILayout.PropertyField(is3D, new GUIContent("Enable 3D"), GUILayout.Height(20));
            is3D.serializedObject.ApplyModifiedProperties();
            EditorGUILayout.PropertyField(positionShake, new GUIContent("Enable Position Shake"), GUILayout.Height(20));
            positionShake.serializedObject.ApplyModifiedProperties();
            EditorGUILayout.PropertyField(rotationShake, new GUIContent("Enable Rotation Shake"), GUILayout.Height(20));
            rotationShake.serializedObject.ApplyModifiedProperties();

            if(smoothShake.GetComponent<Camera>() == null)
            {
                EditorGUILayout.PropertyField(scaleShake, new GUIContent("Enable Scale Shake"), GUILayout.Height(20));
                scaleShake.serializedObject.ApplyModifiedProperties();
            }


            EditorGUILayout.LabelField("", GUILayout.Height(10));
            EditorGUILayout.LabelField("Shake Settings", EditorStyles.boldLabel, GUILayout.Height(20));

            EditorGUILayout.PropertyField(constantShake, new GUIContent("Enable Constant Shake"), GUILayout.Height(20));
            constantShake.serializedObject.ApplyModifiedProperties();

            EditorGUILayout.LabelField("", GUILayout.Height(10));
            EditorGUILayout.LabelField("Settings for fading in / out", EditorStyles.miniLabel, GUILayout.Height(20));

            EditorGUILayout.PropertyField(fadeIn, new GUIContent("Enable Fade In"), GUILayout.Height(20));
            fadeIn.serializedObject.ApplyModifiedProperties();
            if (smoothShake.fadeIn)
            {
                smoothShake.fadeInDuration = EditorGUILayout.FloatField("Fade In Duration", smoothShake.fadeInDuration, GUILayout.Height(20));
                EditorGUILayout.PropertyField(fadeInCurve, new GUIContent("Fade In Curve"), GUILayout.Height(20));
                fadeInCurve.serializedObject.ApplyModifiedProperties();
                EditorGUILayout.LabelField("", GUILayout.Height(5));
            }
            EditorGUILayout.PropertyField(fadeOut, new GUIContent("Enable Fade Out"), GUILayout.Height(20));
            fadeOut.serializedObject.ApplyModifiedProperties();
            if (smoothShake.fadeOut)
            {
                smoothShake.fadeOutDuration = EditorGUILayout.FloatField("Fade Out Duration", smoothShake.fadeOutDuration, GUILayout.Height(20));
                EditorGUILayout.PropertyField(fadeOutCurve, new GUIContent("Fade Out Curve"), GUILayout.Height(20));
                fadeOutCurve.serializedObject.ApplyModifiedProperties();
                EditorGUILayout.LabelField("", GUILayout.Height(5));
            }
            EditorGUILayout.PropertyField(useExtraShakeLength, new GUIContent("Use Extra Shake Length"), GUILayout.Height(20));
            useExtraShakeLength.serializedObject.ApplyModifiedProperties();
            if (smoothShake.useExtraShakeLength)
            {
                smoothShake.additionalShakeDuration = EditorGUILayout.FloatField("Additional Shake Duration", smoothShake.additionalShakeDuration, GUILayout.Height(20));
                EditorGUILayout.LabelField("", GUILayout.Height(5));
            }

            if (smoothShake.positionShake)
            {
                EditorGUILayout.LabelField("", GUILayout.Height(10));
                EditorGUILayout.LabelField("Position Shake Settings", EditorStyles.boldLabel, GUILayout.Height(20));

                EditorGUILayout.PropertyField(proportionalPositionIntensity, new GUIContent("Proportional Intensity"), GUILayout.Height(20));
                proportionalPositionIntensity.serializedObject.ApplyModifiedProperties();
                if (smoothShake.proportionalPositionIntensity)
                {
                    smoothShake.equalPositionintensity = EditorGUILayout.FloatField("Position Intensity", smoothShake.equalPositionintensity);
                    if (GUI.changed)
                    {
                        smoothShake.SetEqualIntensity("positionIntensity");
                    }
                }
                else if (!smoothShake.proportionalPositionIntensity)
                {
                    if (smoothShake.is3D)
                    {
                        smoothShake.positionIntensity = EditorGUILayout.Vector3Field("Position Intensity", smoothShake.positionIntensity);
                    }
                    else if (!smoothShake.is3D)
                    {
                        smoothShake.positionIntensity = EditorGUILayout.Vector2Field("Position Intensity", smoothShake.positionIntensity);
                    }
                }

                if (smoothShake.is3D)
                {
                    smoothShake.positionFrequency = EditorGUILayout.Vector3Field("Position Frequency", smoothShake.positionFrequency);
                }
                else if (!smoothShake.is3D)
                {
                    smoothShake.positionFrequency = EditorGUILayout.Vector2Field("Position Frequency", smoothShake.positionFrequency);
                }


            }

            if (smoothShake.rotationShake)
            {
                EditorGUILayout.LabelField("", GUILayout.Height(10));
                EditorGUILayout.LabelField("Rotation Shake Settings", EditorStyles.boldLabel, GUILayout.Height(20));

                if (smoothShake.is3D)
                {
                    EditorGUILayout.PropertyField(proportionalRotationIntensity, new GUIContent("Proportional Intensity"), GUILayout.Height(20));
                    proportionalRotationIntensity.serializedObject.ApplyModifiedProperties();
                    if (smoothShake.proportionalRotationIntensity)
                    {
                        smoothShake.equalRotationintensity = EditorGUILayout.FloatField("Rotation Intensity", smoothShake.equalRotationintensity);
                        if (GUI.changed)
                        {
                            smoothShake.SetEqualIntensity("rotationIntensity");
                         
                        }
   
                    }
                    else if (!smoothShake.proportionalRotationIntensity)
                    {
                        if (smoothShake.is3D)
                        {
                            smoothShake.rotationIntensity = EditorGUILayout.Vector3Field("Rotation Intensity", smoothShake.rotationIntensity);
                        }

                    }

                    smoothShake.rotationFrequency = EditorGUILayout.Vector3Field("Rotation Frequency", smoothShake.rotationFrequency);
                }
                else if (!smoothShake.is3D)
                {
                    smoothShake.rotationIntensity.y = EditorGUILayout.FloatField("Rotation Intensity", smoothShake.rotationIntensity.y);
                    smoothShake.rotationFrequency.y = EditorGUILayout.FloatField("Rotation Frequency", smoothShake.rotationFrequency.y);
                }


            }

            if(smoothShake.GetComponent<Camera>() == null)
            {
                if (smoothShake.scaleShake)
                {
                    EditorGUILayout.LabelField("", GUILayout.Height(10));
                    EditorGUILayout.LabelField("Scale Shake Settings", EditorStyles.boldLabel, GUILayout.Height(20));

                    EditorGUILayout.PropertyField(proportionalScaleIntensity, new GUIContent("Proportional Intensity"), GUILayout.Height(20));
                    proportionalScaleIntensity.serializedObject.ApplyModifiedProperties();
                    if (smoothShake.proportionalScaleIntensity)
                    {
                        smoothShake.equalScaleIntensity = EditorGUILayout.FloatField("Scale Intensity", smoothShake.equalScaleIntensity);
                        if (GUI.changed)
                        {
                            smoothShake.SetEqualIntensity("scaleIntensity");
                        }
                    }
                    else if (!smoothShake.proportionalScaleIntensity)
                    {
                        if (smoothShake.is3D)
                        {
                            smoothShake.scaleIntensity = EditorGUILayout.Vector3Field("Scale Intensity", smoothShake.scaleIntensity);
                        }
                        else if (!smoothShake.is3D)
                        {
                            smoothShake.scaleIntensity = EditorGUILayout.Vector2Field("Scale Intensity", smoothShake.scaleIntensity);
                        }
                    }

                    if (smoothShake.is3D)
                    {
                        smoothShake.scaleFrequency = EditorGUILayout.Vector3Field("Scale Frequency", smoothShake.scaleFrequency);
                    }
                    else if (!smoothShake.is3D)
                    {
                        smoothShake.scaleFrequency = EditorGUILayout.Vector2Field("Scale Frequency", smoothShake.scaleFrequency);
                    }
                }
            }
           

            if(smoothShake.GetComponent<Camera>() != null)
            {
                EditorGUILayout.LabelField("", GUILayout.Height(10));
                EditorGUILayout.LabelField("Field of view shake settings (only for 3D cameras)", EditorStyles.miniBoldLabel, GUILayout.Height(20));

                EditorGUILayout.PropertyField(fovShake, new GUIContent("Enable FOV Shake"), GUILayout.Height(20));
                fovShake.serializedObject.ApplyModifiedProperties();

                if (smoothShake.fovShake)
                {
                    smoothShake.fovShakeIntensity = EditorGUILayout.FloatField("FOV Shake Intensity", smoothShake.fovShakeIntensity);
                    smoothShake.fovShakeFrequency = EditorGUILayout.FloatField("FOV Shake Frequency", smoothShake.fovShakeFrequency);
                }
            }

            EditorGUILayout.LabelField("", GUILayout.Height(10));
            EditorGUILayout.PropertyField(enableOnStart, new GUIContent("Enable Shake On Start"), GUILayout.Height(20));
            enableOnStart.serializedObject.ApplyModifiedProperties();


            EditorGUILayout.LabelField("", GUILayout.Height(10));

            GUILayout.BeginHorizontal();

            if (GUILayout.Button("Test Shake"))
            {
                if (smoothShake.hasRunOnce)
                {
                    smoothShake.StartShake();
                }

            }

            if (GUILayout.Button("Stop Test Shake"))
            {
                if (smoothShake.hasRunOnce)
                {
                    smoothShake.StopShake();
                }
            }

            GUILayout.EndHorizontal();
        }
    }


}



