using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ShapeData), false)]
[CanEditMultipleObjects]
[System.Serializable]
public class Shaperdatadrawr : Editor
{
    private ShapeData shapeDataInstance => target as ShapeData;

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        ClearBroadButtion();
        EditorGUILayout.Space();

        DrawColumnsInputFields();
        EditorGUILayout.Space();

        if (shapeDataInstance.broad != null && shapeDataInstance.columns > 0 && shapeDataInstance.rows > 0)
        {
            DrawBoardTable();
        }

        serializedObject.ApplyModifiedProperties();

        if (GUI.changed)
        {
            EditorUtility.SetDirty(shapeDataInstance);
        }
    }
        private void ClearBroadButtion()
    {
        if (GUILayout.Button("Clear Board"))
        {
            shapeDataInstance.Clear();
        }
    }
    private void DrawColumnsInputFields()
    {
        var columnsTemp = shapeDataInstance.columns;
        var rowsTemp = shapeDataInstance.rows;

        shapeDataInstance.columns = EditorGUILayout.IntField("Columns", shapeDataInstance.columns);
        shapeDataInstance.rows = EditorGUILayout.IntField("Rows", shapeDataInstance.rows);

        if ((shapeDataInstance.columns != columnsTemp || shapeDataInstance.rows != rowsTemp)
           && shapeDataInstance.columns > 0 && shapeDataInstance.rows > 0)
        {
            shapeDataInstance.CreateNewBroar();
        }
    }
    private void DrawBoardTable()
    {
        
        var tableStyle = new GUIStyle("box");
        tableStyle.padding = new RectOffset(10, 10, 10, 10);// chinh khung vien o vong edit
        tableStyle.margin.left = 32;

        var hearColumnsStyle = new GUIStyle();
        hearColumnsStyle.fixedWidth = 50;// be ngang cua o vuong trong edit
        hearColumnsStyle.alignment = TextAnchor.MiddleCenter; // vi tri o vuong dc can giua theo ca chieu ngang va doc

        var rowStyle = new GUIStyle();
        rowStyle.fixedHeight = 27; // khoang cach hang tren vs hang duoi edit
        rowStyle.alignment = TextAnchor.MiddleCenter;

        var dataFieldSyle = new GUIStyle(EditorStyles.miniButtonMid);
        dataFieldSyle.normal.background = Texture2D.grayTexture; // chon mau o vong
        dataFieldSyle.onNormal.background = Texture2D.whiteTexture; 
        
        for(var row = 0;row<shapeDataInstance.rows;row++)
        {
            EditorGUILayout.BeginHorizontal(hearColumnsStyle);
            for(var column = 0;column<shapeDataInstance.columns;column++)
            {
                EditorGUILayout.BeginHorizontal(rowStyle);
                var data = EditorGUILayout.Toggle(shapeDataInstance.broad[row].column[column], dataFieldSyle);
                shapeDataInstance.broad[row].column[column] = data;
                EditorGUILayout.EndHorizontal();
            }
            EditorGUILayout.EndHorizontal();
        }
       

    }

}
