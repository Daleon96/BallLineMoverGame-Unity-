using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(LevelDate))]

public class LevelDateDrawer : Editor
{
    public override void OnInspectorGUI()
    {
        var script = (LevelDate)target;

        // Відображення стандартного інспектора
        DrawDefaultInspector();

        // Додаткова сітка
        GUILayout.Space(10);
        GUILayout.Label("State Grid Visualization:", EditorStyles.boldLabel);

        for (int y = 0; y < script.rows; y++)
        {
            EditorGUILayout.BeginHorizontal(); // Початок рядка
            for (int x = 0; x < script.columns; x++)
            {
                LevelDate.DrawBlockType state = script.GetState(x, y);
                
                Color drawColor = Color.white;
                switch (state)
                {
                    case LevelDate.DrawBlockType.BLOCK:
                    {
                        drawColor = Color.cyan;
                        break;
                    }
                    case LevelDate.DrawBlockType.BARRIER:
                    {
                        drawColor = Color.black;
                        break;
                    }
                    case LevelDate.DrawBlockType.PLAYER_START_POSITION:
                    {
                        drawColor = Color.green;
                        break;
                    }
                }
                GUIStyle style = new GUIStyle(GUI.skin.box);
                style.normal.background = MakeTex(20, 20, drawColor);

                if (GUILayout.Button("", style, GUILayout.Width(50), GUILayout.Height(50)))
                {
                    script.SetState(x, y, script.drawType);
                }
            }
            EditorGUILayout.EndHorizontal(); // Кінець рядка
        }

        if (GUI.changed)
        {
            EditorUtility.SetDirty(script);
        }
    }

    private Texture2D MakeTex(int width, int height, Color col)
    {
        Color[] pix = new Color[width * height];
        for (int i = 0; i < pix.Length; i++) pix[i] = col;

        Texture2D result = new Texture2D(width, height);
        result.SetPixels(pix);
        result.Apply();
        return result;
    }
}