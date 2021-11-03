using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(EnemySpawner))]
public class EnemySpawnerEditor : Editor
{
    EnemySpawner enemySpawner;
    Camera cam;

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
    }

    private void OnSceneGUI() {
        // Disable left click selection
        HandleUtility.AddDefaultControl(GUIUtility.GetControlID(FocusType.Passive));

        enemySpawner = (EnemySpawner)target;
        cam = SceneView.lastActiveSceneView.camera;

        // Screen to world
        // Screen.height會比實際視窗高度多40px
        Vector3 mousePos = Event.current.mousePosition;
        if(mousePos.x < 0 || mousePos.x > Screen.width || mousePos.y < 0 || mousePos.y > Screen.height - 40f){
            return;
        }
        mousePos.z = -cam.worldToCameraMatrix.MultiplyPoint(enemySpawner.transform.position).z;
        mousePos.y = Screen.height - mousePos.y - 40f;
        mousePos = cam.ScreenToWorldPoint (mousePos);
        
        Handles.color = enemySpawner.color;
        for(int i=0;i<enemySpawner.nodes.Count;++i){
            enemySpawner.nodes[i] = Handles.PositionHandle(enemySpawner.nodes[i], Quaternion.identity);
            if(i == 0){
                Handles.DrawLine(enemySpawner.transform.position, enemySpawner.nodes[0], 2f);
            }
            else{
                Handles.DrawLine(enemySpawner.nodes[i-1], enemySpawner.nodes[i], 2f);
            }
        }

        // 在滑鼠位置新增一個節點，並且連接最近的線段
        if(Event.current.type == EventType.KeyDown && Event.current.keyCode == KeyCode.A){
            enemySpawner.AddNode(mousePos);
            EditorUtility.SetDirty(enemySpawner);
        }

        // 刪除離滑鼠位置最近的節點
        if(Event.current.type == EventType.KeyDown && Event.current.keyCode == KeyCode.D){
            enemySpawner.DeleteNode(mousePos);
            EditorUtility.SetDirty(enemySpawner);
        }

        if(GUI.changed && !Application.isPlaying){
            EditorUtility.SetDirty(enemySpawner);
        }
    }
}
