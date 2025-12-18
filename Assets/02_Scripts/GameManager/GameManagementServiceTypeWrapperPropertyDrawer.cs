using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(GameManagementServiceWrapper))]
public class GameManagementServiceTypeWrapperPropertyDrawer : PropertyDrawer
{
    private static List<Type> types;
    private static List<string> names;

    [InitializeOnLoadMethod]
    static void CacheTypeNames()
    {
        types = TypeCache.GetTypesDerivedFrom<IGameManagementService>()
            .Where(t => !t.IsAbstract && typeof(MonoBehaviour).IsAssignableFrom(t))
            .ToList();
        names = types.Select(t => t.FullName).ToList();
        names.Insert(0, "[None]");
    }

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        if (null == types || null == names) CacheTypeNames();

        SerializedProperty nameProperty = property.FindPropertyRelative("name");

        int currentIdx = Mathf.Max(0, names.IndexOf(nameProperty.stringValue));

        int newIdx = EditorGUI.Popup(position, label.text, currentIdx, names.ToArray());

        if (newIdx != currentIdx) nameProperty.stringValue = newIdx == 0 ? "[Not Selected]" : names[newIdx];
    }
}
