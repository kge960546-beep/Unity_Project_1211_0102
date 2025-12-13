using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(GameManagementServiceWrapper))]
public class GameManagementServiceTypeWrapperPropertyDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        SerializedProperty nameProperty = property.FindPropertyRelative("name");

        var types =
            AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(t => t.GetTypes())
                .Where(t =>
                    typeof(IGameManagementService).IsAssignableFrom(t)
                    && t.IsClass
                    && !t.IsAbstract
                    && typeof(MonoBehaviour).IsAssignableFrom(t))
                .ToList();

        var names = types.Select(t => t.FullName).ToList();
        names.Insert(0, "[None]");

        int currentIdx = Mathf.Max(0, names.IndexOf(nameProperty.stringValue));

        int newIdx = EditorGUI.Popup(position, label.text, currentIdx, names.ToArray());

        if (newIdx != currentIdx) nameProperty.stringValue = newIdx == 0 ? "" : names[newIdx];
    }
}
