using System.IO;
using UnityEditor;
using UnityEngine;

public class AddNamespaceTool : EditorWindow
{
    private string namespaceName = ""; // Ĭ�������ռ�
    private string selectedFolderPath = "Assets/Scripts"; // Ĭ��Ϊ Assets\ScriptsĿ¼����ѡ

    [MenuItem("Tools/Add Namespace")]
    public static void ShowWindow()
    {
        GetWindow<AddNamespaceTool>("Add Namespace");
    }

    private void OnGUI()
    {
        GUILayout.Label("������������ռ�", EditorStyles.boldLabel);

        // ���������ռ�
        namespaceName = EditorGUILayout.TextField("�����ռ�:", namespaceName);

        // ��ʾ��ǰѡ�����ļ���·��
        EditorGUILayout.LabelField("ѡ�����ļ���:", selectedFolderPath);

        // ѡ���ļ��а�ť
        if (GUILayout.Button("ѡ���ļ���"))
        {
            string folderPath = EditorUtility.OpenFolderPanel("ѡ��ű������ļ���", Application.dataPath, "");
            if (!string.IsNullOrEmpty(folderPath))
            {
                if (folderPath.StartsWith(Application.dataPath))
                {
                    selectedFolderPath = "Assets" + folderPath.Substring(Application.dataPath.Length);
                }
                else
                {
                    EditorUtility.DisplayDialog("����", "��ѡ����Ŀ�е��ļ��У�", "ȷ��");
                }
            }
        }

        // ��ʼ��������ռ�İ�ť
        if (GUILayout.Button("��ʼ���"))
        {
            AddNamespaceToScripts(namespaceName, selectedFolderPath);
        }
    }

    private static void AddNamespaceToScripts(string namespaceName, string folderPath)
    {
        // ����Ŀ���ļ����µ����нű��ļ�
        string[] scriptPaths = Directory.GetFiles(folderPath, "*.cs", SearchOption.AllDirectories);

        foreach (string scriptPath in scriptPaths)
        {
            string[] lines = File.ReadAllLines(scriptPath);

            // �����Ѿ����������ռ�Ľű�
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
        EditorUtility.DisplayDialog("���", $"�����ռ�����ӵ� {folderPath} �µĽű�", "ȷ��");
    }
}