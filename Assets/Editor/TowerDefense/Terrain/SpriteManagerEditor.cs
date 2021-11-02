using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(SpriteManager))]
public class SpriteManagerEditor : Editor
{
    public override void OnInspectorGUI() {
        DrawDefaultInspector();
        // SpriteManager spriteManager = (SpriteManager)target;
        // spriteManager.Initialize();

        // GUILayout.BeginHorizontal();
        // spriteManager.spriteNameFoldout = EditorGUILayout.BeginFoldoutHeaderGroup(spriteManager.spriteNameFoldout, "Sprite Name");
        // spriteManager.spriteTypeNum = EditorGUILayout.IntField(spriteManager.spriteTypeNum, new []{GUILayout.Width(40)});
        // GUILayout.EndHorizontal();
        // if(spriteManager.spriteNameFoldout){
        //     EditorGUI.indentLevel++;
        //     for(int i=0;i<spriteManager.spriteTypeNum;++i){
        //         spriteManager.spritePacks[i].name = EditorGUILayout.TextField("Element " + i, spriteManager.spritePacks[i].name);
        //     }
        //     EditorGUI.indentLevel--;
        // }
        // EditorGUILayout.EndFoldoutHeaderGroup();
        
        // for(int i=0;i<spriteManager.spriteTypeNum;++i){
        //     GUILayout.BeginHorizontal();
        //     spriteManager.spritePacks[i].foldout = EditorGUILayout.BeginFoldoutHeaderGroup(spriteManager.spritePacks[i].foldout, spriteManager.spritePacks[i].name);
        //     spriteManager.spritePacks[i].length = EditorGUILayout.IntField(spriteManager.spritePacks[i].length, new []{GUILayout.Width(40)});
        //     GUILayout.EndHorizontal();
        //     if(spriteManager.spritePacks[i].foldout){
        //         EditorGUI.indentLevel++;
        //         for(int j=0;j<spriteManager.spritePacks[i].length;++j){
        //             // spriteManager.spriteName[j] = EditorGUILayout.TextField("Element " + j, spriteManager.spriteName[j]);
        //             spriteManager.spritePacks[i].sprites[j] = (Sprite)EditorGUILayout.ObjectField(spriteManager.spritePacks[i].sprites[j], typeof(Sprite), false);
        //         }
        //         EditorGUI.indentLevel--;
        //     }
        //     EditorGUILayout.EndFoldoutHeaderGroup();
        // }

        // if(GUI.changed && !Application.isPlaying){
        //     EditorUtility.SetDirty(spriteManager);
        // }
    }
}
