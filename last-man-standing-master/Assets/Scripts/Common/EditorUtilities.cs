#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System.Text;

public class EditorUtilities
{

    [MenuItem("Assets/Get List Name Selected asset")]
    static void ListNameSelected()
    {
        Object[] selection = Selection.GetFiltered(typeof(Object), SelectionMode.DeepAssets);
        StringBuilder sb = new StringBuilder();
        for (int i = 0; i < selection.Length; i++)
        {
            sb.AppendLine(selection[i].name);
        }
        Debug.Log(sb.ToString());
    }
}

#endif