using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(WorldBuilder))]
public class WorldBuilderEditor : Editor
{
    bool mouseDown = false;
    bool leftShiftDown = false;
    WorldBuilder worldBuilder;
    SpriteManager spriteManager;
    Camera cam;

    public override void OnInspectorGUI()
	{
        worldBuilder = (WorldBuilder)target;
        EditorGUILayout.Vector2Field("Map Size", worldBuilder.mapSize);
        // Sprite manager
        worldBuilder.spriteManager = EditorGUILayout.ObjectField("Sprite Manager", worldBuilder.spriteManager, typeof(SpriteManager), true) as SpriteManager;
        if(worldBuilder.spriteManager == null) return;
        spriteManager = worldBuilder.spriteManager;

		// DrawDefaultInspector();
        worldBuilder.Initialize();

        // Map prefab
        worldBuilder.prefab = EditorGUILayout.ObjectField("Map Prefab", worldBuilder.prefab, typeof(GameObject), true) as GameObject;

        // Map size
        worldBuilder.mapSize_t = EditorGUILayout.Vector2IntField("Map Size", worldBuilder.mapSize_t);
        if(GUILayout.Button("Apply")){
            worldBuilder.mapSize = worldBuilder.mapSize_t;
        }

        GUILayout.Label("");

        // Brush
        worldBuilder.brushTool = (BrushTool)GUILayout.Toolbar((int)worldBuilder.brushTool, new[]{"Sprite Brush", "Height Brush"});

        switch(worldBuilder.brushTool){
            case BrushTool.sprite:
                // Sprite
                GUILayout.Label("Brush Type");
                worldBuilder.brushType = (BrushType)EditorGUILayout.EnumPopup(worldBuilder.brushType);

                switch(worldBuilder.brushType){
                    case BrushType.point:
                        break;
                    case BrushType.range:
                        // Brush size
                        GUILayout.Label("Brush Size");
                        worldBuilder.spriteBrushSize = EditorGUILayout.Slider(worldBuilder.spriteBrushSize, 0f, 10f);
                        break;
                }

                // Select sprite
                GUILayout.Label("Select Sprite");

                for(int n=0;n<spriteManager.spriteTypeNum;++n){
                    worldBuilder.spriteFoldout[n] = EditorGUILayout.BeginFoldoutHeaderGroup(worldBuilder.spriteFoldout[n], spriteManager.spritePacks[n].name);
                    if(worldBuilder.spriteFoldout[n]){
                        worldBuilder.spriteScrollPos[n] = GUILayout.BeginScrollView(worldBuilder.spriteScrollPos[n], true, false, GUILayout.Height(200));
                        GUILayout.BeginHorizontal();
                        
                        for(int i=0;i<spriteManager.spritePacks[n].length;++i){
                            // if(spriteManager.spritePacks[n].sprites[i] == null){
                            //     continue;
                            // }
                            if(GUILayout.Button(spriteManager.spritePacks[n].sprites[i].texture, GUILayout.Width(100))){
                                worldBuilder.spriteType = n;
                                worldBuilder.spriteID = i;
                            }
                        }

                        GUILayout.EndHorizontal();
                        GUILayout.EndScrollView();
                    }
                    EditorGUILayout.EndFoldoutHeaderGroup();
                }
                break;
            case BrushTool.height:
                // Height
                GUILayout.Label("Brush Size");
                worldBuilder.heightBrushSize = EditorGUILayout.Slider(worldBuilder.heightBrushSize, 0f, 10f);
                GUILayout.Label("Strength");
                worldBuilder.strength = EditorGUILayout.Slider(worldBuilder.strength, 0f, 1f);
                break;
        }

        // Reset
        if(GUILayout.Button("Reset")){
            worldBuilder.Reset();
            worldBuilder.mapSize = worldBuilder.mapSize_t;
        }

        if(GUI.changed && !Application.isPlaying){
            EditorUtility.SetDirty(worldBuilder);
        }
	}

    public void OnSceneGUI()
    {
        worldBuilder = (WorldBuilder)target;
        cam = SceneView.lastActiveSceneView.camera;
        
        // Disable left click selection
        HandleUtility.AddDefaultControl(GUIUtility.GetControlID(FocusType.Passive));
 
        // Screen to world
        // Screen.height會比實際視窗高度多40px
        Vector3 mousePos = Event.current.mousePosition;
        if(mousePos.x < 0 || mousePos.x > Screen.width || mousePos.y < 0 || mousePos.y > Screen.height - 40f){
            return;
        }
        mousePos.z = -cam.worldToCameraMatrix.MultiplyPoint(worldBuilder.transform.position).z;
        mousePos.y = Screen.height - mousePos.y - 40f;
        mousePos = cam.ScreenToWorldPoint (mousePos);

        // Show brush
        Handles.color = Color.white;
        switch(worldBuilder.brushTool){
            case BrushTool.sprite:
                switch(worldBuilder.brushType){
                    case BrushType.point:
                        Handles.DrawWireDisc(mousePos, new Vector3(0, 0, 1), 0.25f);
                        break;
                    case BrushType.range:
                        Handles.DrawWireDisc(mousePos, new Vector3(0, 0, 1), worldBuilder.spriteBrushSize/2);
                        break;
                }
                break;
            case BrushTool.height:
                Handles.DrawWireDisc(mousePos, new Vector3(0, 0, 1), worldBuilder.heightBrushSize/2);
                break;
        }

        // Mouse down
        mouseDown |= (Event.current.type == EventType.MouseDown && Event.current.button == 0);
        mouseDown &= !(Event.current.type == EventType.MouseUp && Event.current.button == 0);

        // Left shift
        leftShiftDown |= (Event.current.type == EventType.KeyDown && Event.current.keyCode == KeyCode.LeftShift);
        leftShiftDown &= !(Event.current.type == EventType.KeyUp && Event.current.keyCode == KeyCode.LeftShift);

        if(mouseDown){
            switch(worldBuilder.brushTool){
                case BrushTool.sprite:
                    worldBuilder.DrawSprite(mousePos);
                    break;
                case BrushTool.height:
                    worldBuilder.DrawHeight(mousePos, !leftShiftDown);
                    break;
            }
        }
 
        SceneView.RepaintAll();
    }
}
