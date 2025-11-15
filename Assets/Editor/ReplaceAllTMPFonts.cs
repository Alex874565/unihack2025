using UnityEngine;
using UnityEditor;
using TMPro;

public class ReplaceAllTMPFonts : EditorWindow
{
    public TMP_FontAsset newFont;

    [MenuItem("Tools/Replace All TMP Fonts")]
    public static void ShowWindow()
    {
        GetWindow<ReplaceAllTMPFonts>();
    }

    void OnGUI()
    {
        newFont = (TMP_FontAsset)EditorGUILayout.ObjectField(
            "New TMP Font", newFont, typeof(TMP_FontAsset), false);

        if (GUILayout.Button("Replace Fonts in Scene"))
        {
            foreach (var text in GameObject.FindObjectsOfType<TextMeshProUGUI>(true))
            {
                Undo.RecordObject(text, "Replace TMP Font");
                text.font = newFont;
            }

            Debug.Log("All fonts replaced!");
        }
    }
}
