using System.IO;
using UnityEditor;
using UnityEngine;

public class AddNamespaceTool : EditorWindow
{
    private string namespaceName = ""; // 默认命名空间
    private string selectedFolderPath = "Assets/Scripts"; // 默认为 Assets\Scripts目录，可选

    [MenuItem("Tools/Add Namespace")]
    public static void ShowWindow()
    {
        GetWindow<AddNamespaceTool>("Add Namespace");
    }

    private void OnGUI()
    {
        GUILayout.Label("批量添加命名空间", EditorStyles.boldLabel);

        // 输入命名空间
        namespaceName = EditorGUILayout.TextField("命名空间:", namespaceName);

        // 显示当前选定的文件夹路径
        EditorGUILayout.LabelField("选定的文件夹:", selectedFolderPath);

        // 选择文件夹按钮
        if (GUILayout.Button("选择文件夹"))
        {
            string folderPath = EditorUtility.OpenFolderPanel("选择脚本所在文件夹", Application.dataPath, "");
            if (!string.IsNullOrEmpty(folderPath))
            {
                if (folderPath.StartsWith(Application.dataPath))
                {
                    selectedFolderPath = "Assets" + folderPath.Substring(Application.dataPath.Length);
                }
                else
                {
                    EditorUtility.DisplayDialog("错误", "请选择项目中的文件夹！", "确定");
                }
            }
        }

        // 开始添加命名空间的按钮
        if (GUILayout.Button("开始添加"))
        {
            AddNamespaceToScripts(namespaceName, selectedFolderPath);
        }
    }

    private static void AddNamespaceToScripts(string namespaceName, string folderPath)
    {
        // 查找目标文件夹下的所有脚本文件
        string[] scriptPaths = Directory.GetFiles(folderPath, "*.cs", SearchOption.AllDirectories);

        foreach (string scriptPath in scriptPaths)
        {
            string[] lines = File.ReadAllLines(scriptPath);

            // 跳过已经包含命名空间的脚本
            if (lines.Length > 0 && lines[0].Contains("namespace"))
                continue;

            using (StreamWriter writer = new StreamWriter(scriptPath))
            {
                writer.WriteLine($"namespace {namespaceName}");
                writer.WriteLine("{");
                foreach (string line in lines)
                {
                    writer.WriteLine($"    {line}");
                }
                writer.WriteLine("}");
            }
        }

        AssetDatabase.Refresh();
        EditorUtility.DisplayDialog("完成", $"命名空间已添加到 {folderPath} 下的脚本", "确定");
    }
}