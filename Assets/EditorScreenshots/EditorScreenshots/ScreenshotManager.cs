using System.Collections.Generic;
using System;
using System.Security.AccessControl;
using System.Collections;
using UnityEngine;
using System.IO;
#if UNITY_EDITOR
using UnityEditor;
#endif
[ExecuteInEditMode]
public class ScreenshotManager : MonoBehaviour
{
#if UNITY_EDITOR
    [HideInInspector] public int printIndex;
    public KeyCode screenshotKey = KeyCode.C;
    public int[] editorScreenSizeIndices;
    public string screenshotsFolderName = "Screenshots";
    [TextArea(2, 4)] public string path;

    private void Start()
    {
        path = (Application.dataPath.Replace("/Assets", "")) + "/" + screenshotsFolderName;
        if (Directory.Exists(path) == false)
        {
            Directory.CreateDirectory(path);
            print("Created folder at (" + path + ")");
        }
        if (editorScreenSizeIndices != null && editorScreenSizeIndices.Length > 0)
            for (int i = 0; i < editorScreenSizeIndices.Length && i < GameViewUtils.GetViewListSize(); i++)
            {
                string s = GameViewUtils.GetViewDimentions(editorScreenSizeIndices[i]);
                if (Directory.Exists(path + "/" + s) == false)
                {
                    Directory.CreateDirectory(path + "/" + s);
                }
            }
    }

    internal void PrintSizesTillIndex()
    {
        for (int i = 0; i <= printIndex; i++)
        {
            print(i + ". " + GameViewUtils.GetViewDimentions(i));
        }
    }

    internal void EditorScreenshot()
    {
        StartCoroutine(EditorScreenshotRoutine());
    }

    float temp;
    IEnumerator EditorScreenshotRoutine()
    {
        temp = Time.timeScale;
        Time.timeScale = 0.05f;
        Time.fixedDeltaTime = Time.timeScale * 0.02f;
        path = (Application.dataPath.Replace("/Assets", "")) + "/" + screenshotsFolderName;
        if (Directory.Exists(path) == false)
        {
            Directory.CreateDirectory(path);
            yield return null;
        }
        for (int i = 0; i < editorScreenSizeIndices.Length; i++)
        {
            if (Directory.Exists(path + "/" + Screen.width + "x" + Screen.height) == false)
            {
                Directory.CreateDirectory(path + "/" + Screen.width + "x" + Screen.height);
                yield return null;
            }
        }
        for (int i = 0; i < editorScreenSizeIndices.Length; i++)
        {
            yield return null;
            GameViewUtils.SetSize(editorScreenSizeIndices[i]);
            yield return null;
            ScreenCapture.CaptureScreenshot(screenshotsFolderName + "/" + Screen.width + "x" + Screen.height + "/" + "Screenshot (" + Screen.width + "x" + Screen.height + ")_ " + System.DateTime.Now.ToString("yyyymmdd_hhmmss") + ".png", 1);
            yield return null;
        }
        Time.timeScale = temp;
        Time.fixedDeltaTime = Time.timeScale * 0.02f;
    }

    private void Update()
    {
        if (Input.GetKeyDown(screenshotKey))
        {
            EditorScreenshot();
        }
    }
    private void OnValidate()
    {
        path = (Application.dataPath.Replace("/Assets", "")) + "/" + screenshotsFolderName;
    }
#endif
}

#if UNITY_EDITOR
[CustomEditor(typeof(ScreenshotManager)), CanEditMultipleObjects]
public class ScreenshotManagerInspector : Editor
{
    ScreenshotManager screenshotManager;

    private void OnEnable()
    {
        screenshotManager = (ScreenshotManager)target;
    }

    public override void OnInspectorGUI()
    {
        if (GUILayout.Button("Take Screenshot"))
            screenshotManager.EditorScreenshot();
        EditorGUILayout.Space(10);

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Print Size Till", GUILayout.Width(100));
        screenshotManager.printIndex = EditorGUILayout.IntField(screenshotManager.printIndex);
        if (GUILayout.Button("Print", GUILayout.Width(40)))
            screenshotManager.PrintSizesTillIndex();
        EditorGUILayout.EndHorizontal();

        serializedObject.Update();
        EditorGUILayout.PropertyField(serializedObject.FindProperty("screenshotKey"));

        EditorGUILayout.BeginVertical("Box");
        EditorGUILayout.PropertyField(serializedObject.FindProperty("editorScreenSizeIndices"));
        if (screenshotManager.editorScreenSizeIndices != null && screenshotManager.editorScreenSizeIndices.Length > 0)
        {
            for (int i = 0; i < screenshotManager.editorScreenSizeIndices.Length && i < GameViewUtils.GetViewListSize(); i++)
            {
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("Size " + (i + 1), new GUIStyle(GUI.skin.label), GUILayout.Width(100));
                screenshotManager.editorScreenSizeIndices[i] = EditorGUILayout.IntField(screenshotManager.editorScreenSizeIndices[i]);
                EditorGUILayout.LabelField(GameViewUtils.GetViewDimentions(screenshotManager.editorScreenSizeIndices[i]));
                EditorGUILayout.EndHorizontal();
            }
        }
        EditorGUILayout.EndVertical();

        EditorGUILayout.BeginVertical("Box");
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Folder Name", GUILayout.Width(80));
            screenshotManager.screenshotsFolderName = EditorGUILayout.TextField(screenshotManager.screenshotsFolderName);
            EditorGUILayout.EndHorizontal();
        }
        EditorGUILayout.PropertyField(serializedObject.FindProperty("path"));
        if (GUILayout.Button("Show in Explorer"))
        {
            if (Application.platform == RuntimePlatform.WindowsEditor)
            {
                string newPath = screenshotManager.path.Replace(@"/", @"\");
                System.Diagnostics.Process.Start("explorer.exe", "\"" + newPath + "\"");
            }
            if (Application.platform == RuntimePlatform.OSXEditor)
            {
                EditorUtility.RevealInFinder(screenshotManager.path);
            }
        }
        EditorGUILayout.EndVertical();

        serializedObject.ApplyModifiedProperties();
    }
}
#endif