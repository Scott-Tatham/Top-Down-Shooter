/// <summary>
/// A collection of useful code pieces.
/// </summary>
namespace LazyTitan
{
    /// <summary>
    /// Serialization.
    /// </summary>
    namespace Serialization
    {
        /// <summary>
        /// Attributes.
        /// </summary>
        namespace Attributes
        {
            using System;
            using UnityEngine;
            using UnityEditor;

            /// <summary>
            /// Determines when the field appears in the inspector.
            /// </summary>
            public enum RuntimeBehaviour
            {
                EDITOR,
                RUNTIME,
                BOTH,
                NEITHER
            }

            #region Attributes/PropertyAttribute

            /// <summary>
            /// Determines various properties of how a field appears in the inspector.
            /// </summary>
            [AttributeUsage(AttributeTargets.Field, AllowMultiple = true, Inherited = true)]
            public class FieldPropertiesAttribute : PropertyAttribute
            {
                bool rename, readOnly;
                RuntimeBehaviour runtimeOnly;
                string label;

                public bool GetRename() { return rename; }
                public RuntimeBehaviour GetRuntimeOnly() { return runtimeOnly; }
                public bool GetReadOnly() { return readOnly; }
                public string GetLabel() { return label; }

                public FieldPropertiesAttribute(RuntimeBehaviour runtimeOnly = RuntimeBehaviour.BOTH, bool readOnly = false)
                {
                    rename = false;
                    this.readOnly = readOnly;
                    this.runtimeOnly = runtimeOnly;
                }

                public FieldPropertiesAttribute(string label, RuntimeBehaviour runtimeOnly = RuntimeBehaviour.BOTH, bool readOnly = false)
                {
                    rename = true;
                    this.readOnly = readOnly;
                    this.runtimeOnly = runtimeOnly;
                    this.label = label;
                }
            }

            #endregion

            #region Drawers

            /// <summary>
            /// Determines various properties of how a field appears in the inspector.
            /// </summary>
            [CustomPropertyDrawer(typeof(FieldPropertiesAttribute))]
            public class FieldPropertiesDrawer : PropertyDrawer
            {
                public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
                {
                    FieldPropertiesAttribute propAttribute = (FieldPropertiesAttribute)attribute;

                    if ((Application.isPlaying && (propAttribute.GetRuntimeOnly() == RuntimeBehaviour.BOTH || propAttribute.GetRuntimeOnly() == RuntimeBehaviour.RUNTIME)) || (!Application.isPlaying && (propAttribute.GetRuntimeOnly() == RuntimeBehaviour.BOTH || propAttribute.GetRuntimeOnly() == RuntimeBehaviour.EDITOR)))
                    {

                        string propertyLabel = propAttribute.GetRename() ? propAttribute.GetLabel() : label.text;
                        int multiplier = 1;

                        if (property.propertyType == SerializedPropertyType.Color)
                        {
                            multiplier = 2;
                        }

                        return EditorGUI.GetPropertyHeight(property, new GUIContent(propertyLabel), true) * multiplier;
                    }

                    return 0;
                }

                public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
                {
                    FieldPropertiesAttribute propAttribute = (FieldPropertiesAttribute)attribute;
                    string propertyLabel = propAttribute.GetRename() ? propAttribute.GetLabel() : label.text;

                    if ((Application.isPlaying && (propAttribute.GetRuntimeOnly() == RuntimeBehaviour.BOTH || propAttribute.GetRuntimeOnly() == RuntimeBehaviour.RUNTIME)) || (!Application.isPlaying && (propAttribute.GetRuntimeOnly() == RuntimeBehaviour.BOTH || propAttribute.GetRuntimeOnly() == RuntimeBehaviour.EDITOR)))
                    {
                        if (propAttribute.GetReadOnly())
                        {
                            EditorGUIUtility.labelWidth = 75.0f;

                            switch (property.propertyType)
                            {
                                case SerializedPropertyType.Generic:
                                    Debug.Log(property.isArray);
                                    // TODO: Make List work as array. Form correct indentation. Cleanup and sort foldouts.
                                    if (property.isArray)
                                    {
                                        Debug.Log(property.isExpanded);
                                        int length;

                                        property.Next(true);
                                        property.Next(true);

                                        length = property.intValue;
                                        length = EditorGUI.IntField(position, new GUIContent("Length"), length, GUIStyle.none);
                                        property.intValue = length;

                                        if (length > 0)
                                        {
                                            property.isExpanded = EditorGUI.Foldout(position, property.isExpanded, new GUIContent("Elements"), true, GUIStyle.none);

                                            for (int i = 0; i < length; i++)
                                            {
                                                property.Next(true);
                                                OnGUI(position, property, label);
                                            }
                                        }
                                    }

                                    else
                                    {
                                        property.Next(true);
                                        OnGUI(position, property, label);
                                    }

                                    break;

                                case SerializedPropertyType.Integer:
                                    EditorGUI.LabelField(position, propertyLabel, property.intValue.ToString(), GUIStyle.none);

                                    break;

                                case SerializedPropertyType.Boolean:
                                    EditorGUI.LabelField(position, propertyLabel, property.boolValue.ToString(), GUIStyle.none);

                                    break;

                                case SerializedPropertyType.Float:
                                    EditorGUI.LabelField(position, propertyLabel, property.floatValue.ToString() + "f", GUIStyle.none);

                                    break;

                                case SerializedPropertyType.String:
                                    EditorGUI.LabelField(position, propertyLabel, property.stringValue, GUIStyle.none);

                                    break;

                                case SerializedPropertyType.Color:
                                    EditorGUI.LabelField(new Rect(position.x, position.y, position.width, position.height / 2), propertyLabel, property.colorValue.ToString(), GUIStyle.none);
                                    Texture2D colour = new Texture2D(1, 1);
                                    colour.SetPixel(0, 0, property.colorValue);
                                    colour.Apply();
                                    EditorGUI.DrawTextureTransparent(new Rect(position.x, position.y + (position.height / 2), position.width, position.height / 2), colour, ScaleMode.StretchToFill, 0f);

                                    break;

                                case SerializedPropertyType.ObjectReference:
                                    EditorGUI.LabelField(position, propertyLabel, property.objectReferenceValue != null ? property.objectReferenceValue.ToString() : "Null", GUIStyle.none);

                                    break;

                                case SerializedPropertyType.LayerMask:
                                    Debug.LogError("Layermask property type cannot be readonly.");

                                    break;

                                case SerializedPropertyType.Enum:
                                    EditorGUI.LabelField(position, propertyLabel, property.enumDisplayNames[property.enumValueIndex], GUIStyle.none);

                                    break;

                                case SerializedPropertyType.Vector2:
                                    EditorGUI.LabelField(position, propertyLabel, "[X: " + property.vector2Value.x.ToString() + ", Y: " + property.vector2Value.y.ToString() + "]", GUIStyle.none);

                                    break;

                                case SerializedPropertyType.Vector3:
                                    EditorGUI.LabelField(position, propertyLabel, "[X: " + property.vector3Value.x.ToString() + ", Y: " + property.vector3Value.y.ToString() + ", Z: " + property.vector3Value.z.ToString() + "]", GUIStyle.none);

                                    break;

                                case SerializedPropertyType.Vector4:
                                    EditorGUI.LabelField(position, propertyLabel, "[X: " + property.vector4Value.x.ToString() + ", Y: " + property.vector4Value.y.ToString() + ", Z: " + property.vector4Value.z.ToString() + ", W: " + property.vector4Value.w.ToString() + "]", GUIStyle.none);

                                    break;

                                case SerializedPropertyType.Rect:
                                    EditorGUI.LabelField(position, propertyLabel, "[X: " + property.rectValue.x.ToString() + ", Y: " + property.rectValue.y.ToString() + ", Width: " + property.rectValue.width.ToString() + ", Height: " + property.rectValue.height.ToString() + "]", GUIStyle.none);

                                    break;

                                case SerializedPropertyType.ArraySize:
                                    EditorGUI.LabelField(position, propertyLabel, property.arraySize.ToString(), GUIStyle.none);

                                    break;

                                case SerializedPropertyType.Character:
                                    Debug.LogError("Character property type cannot be readonly. Try a string as an alternative.");

                                    break;

                                case SerializedPropertyType.AnimationCurve:
                                    EditorGUI.LabelField(position, new GUIContent(propertyLabel));
                                    GUI.enabled = false;
                                    EditorGUI.CurveField(new Rect(position.x + EditorGUIUtility.labelWidth, position.y, position.width - EditorGUIUtility.labelWidth, position.height), GUIContent.none, property.animationCurveValue);
                                    GUI.enabled = true;

                                    break;

                                case SerializedPropertyType.Bounds:
                                    EditorGUI.LabelField(new Rect(position.x, position.y, position.width, position.height / 2), propertyLabel, "[Center] [X: " + property.boundsValue.center.x.ToString() + ", Y: " + property.boundsValue.center.y.ToString() + ", Z: " + property.boundsValue.center.z.ToString() + "]", GUIStyle.none);
                                    EditorGUI.LabelField(new Rect(position.x + EditorGUIUtility.labelWidth, position.y + (position.height / 2), position.width, position.height / 2), string.Empty, "[Extents] [X: " + property.boundsValue.extents.x.ToString() + ", Y: " + property.boundsValue.extents.y.ToString() + ", Z: " + property.boundsValue.extents.z.ToString() + "]", GUIStyle.none);

                                    break;

                                case SerializedPropertyType.Gradient:
                                    Debug.LogError("Gradient property type cannot be readonly.");

                                    break;

                                case SerializedPropertyType.Quaternion:
                                    EditorGUI.LabelField(position, propertyLabel, "[X: " + property.quaternionValue.x.ToString() + ", Y: " + property.quaternionValue.y.ToString() + ", Z: " + property.quaternionValue.z.ToString() + ", W: " + property.quaternionValue.w.ToString() + "]", GUIStyle.none);

                                    break;

                                case SerializedPropertyType.ExposedReference:
                                    EditorGUI.LabelField(position, propertyLabel, property.exposedReferenceValue != null ? property.exposedReferenceValue.ToString() : "Null", GUIStyle.none);

                                    break;

                                case SerializedPropertyType.FixedBufferSize:
                                    EditorGUI.LabelField(position, propertyLabel, property.fixedBufferSize.ToString(), GUIStyle.none);

                                    break;

                                case SerializedPropertyType.Vector2Int:
                                    EditorGUI.LabelField(position, propertyLabel, "[X: " + property.vector2Value.x.ToString() + ", Y: " + property.vector2Value.y.ToString() + "]", GUIStyle.none);

                                    break;

                                case SerializedPropertyType.Vector3Int:
                                    EditorGUI.LabelField(position, propertyLabel, "[X: " + property.vector3IntValue.x.ToString() + ", Y: " + property.vector3IntValue.y.ToString() + ", Z: " + property.vector3IntValue.z.ToString() + "]", GUIStyle.none);

                                    break;

                                case SerializedPropertyType.RectInt:
                                    EditorGUI.LabelField(position, propertyLabel, "[X: " + property.rectIntValue.x.ToString() + ", Y: " + property.rectIntValue.y.ToString() + ", Width: " + property.rectIntValue.width.ToString() + ", Height: " + property.rectIntValue.height.ToString() + "]", GUIStyle.none);

                                    break;

                                case SerializedPropertyType.BoundsInt:
                                    EditorGUI.LabelField(new Rect(position.x, position.y, position.width, position.height / 2), propertyLabel, "[Position] [X: " + property.boundsIntValue.position.x.ToString() + ", Y: " + property.boundsIntValue.position.y.ToString() + ", Z: " + property.boundsIntValue.position.z.ToString() + "]", GUIStyle.none);
                                    EditorGUI.LabelField(new Rect(position.x + EditorGUIUtility.labelWidth, position.y + (position.height / 2), position.width, position.height / 2), string.Empty, "[Size] [X: " + property.boundsIntValue.size.x.ToString() + ", Y: " + property.boundsIntValue.size.y.ToString() + ", Z: " + property.boundsIntValue.size.z.ToString() + "]", GUIStyle.none);

                                    break;

                                default:
                                    Debug.LogException(new Exception("Property type not recognized."));

                                    break;
                            }
                        }

                        else
                        {
                            EditorGUI.PropertyField(position, property, new GUIContent(propertyLabel), true);
                        }
                    }
                }
            }

            #endregion

        }
    }
}