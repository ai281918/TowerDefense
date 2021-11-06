using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public enum BrushTool{
    sprite,
    height
}

public enum BrushType{
    point,
    range
}

public class WorldBuilder : MonoBehaviour
{
    // Editor
    public Vector2[] spriteScrollPos;
    public bool[] spriteFoldout;

    // Map size
    [SerializeField]
    Vector2Int _mapSize;
    public Vector2Int mapSize{
        get{
            return _mapSize;
        }
        set{
            value.y += Mathf.Max(value.y-1, 0);
            UpdateMapSize(value);
            _mapSize = value;
        }
    }
    public Vector2Int mapSize_t;
    // Brush
    public BrushTool brushTool = BrushTool.sprite;
    public BrushType brushType = BrushType.point;
    // Sprite brush
    public float spriteBrushSize = 1f;
    public int spriteType = 0;
    public int spriteID = 0;
    // Height brush
    public float heightBrushSize = 1f;
    public float strength = 0.2f;
    public GameObject prefab;
    // Map
    [SerializeField]
    GameObject[,] map;
    public SpriteManager spriteManager;

    public void Initialize(){
        transform.position = Vector3.zero;
        if(spriteManager.spritePacks.Length == 0){
            spriteScrollPos = null;
            spriteFoldout = null;
        }
        else if(spriteScrollPos == null || spriteScrollPos.Length != spriteManager.spritePacks.Length){
            spriteScrollPos = new Vector2[spriteManager.spritePacks.Length];
            spriteFoldout = new bool[spriteManager.spritePacks.Length];
        }
    }

    public void UpdateMapSize(Vector2Int newSize){
        if(newSize.x == 0 || newSize.y == 0){
            Clear();
            return;
        }
        GameObject[,] m = new GameObject[newSize.x, newSize.y];

        // keep old area
        if(map != null){
            for(int i=0;i<Mathf.Min(newSize.x, map.GetLength(0));++i){
                for(int j=0;j<Mathf.Min(newSize.y, map.GetLength(1));++j){
                    m[i, j] = map[i, j];
                }
            }
        }

        // spawn new area
        for(int i=0;i<newSize.x;++i){
            for(int j=0;j<newSize.y;++j){
                if(m[i, j] == null){
                    if(j%2==0){
                        m[i, j] = Instantiate(prefab, new Vector3(i, j*0.25f, j*0.25f), Quaternion.identity);
                        m[i, j].transform.SetParent(transform);
                    }
                    else{
                        m[i, j] = Instantiate(prefab, new Vector3(i+0.5f, j*0.25f, j*0.25f), Quaternion.identity);
                        m[i, j].transform.SetParent(transform);
                    }
                    m[i, j].GetComponent<TerrainUnit>().Initialize(new Vector2Int(i, j));
                }
            }
        }

        // destroy extra area
        if(map != null){
            for(int i=0;i<map.GetLength(0);++i){
                for(int j=0;j<map.GetLength(1);++j){
                    if(i >= newSize.x || j >= newSize.y){
                        DestroyImmediate(map[i, j]);
                    }
                }
            }
        }

        map = m;
        Camera.main.transform.position = new Vector3(mapSize_t.x/2f, mapSize_t.y/4f, Camera.main.transform.position.z);
    }

    public void DrawSprite(Vector3 center){ 
        Check();       
        switch(brushType){
            case BrushType.point:
                DrawSpritePoint(center);
                break;
            case BrushType.range:
                DrawSpriteRange(center);
                break;
        }
    }

    // Draw sprite of the nearest unit
    void DrawSpritePoint(Vector3 center){
        Sprite newSprite = spriteManager.spritePacks[spriteType].sprites[spriteID];

        float minDis = 999f;
        Vector2Int minID = Vector2Int.zero;
        float radius = 1f;
        Vector2Int ldID = new Vector2Int(Mathf.Max(0, Mathf.FloorToInt(center.x-radius)), Mathf.Max(0, Mathf.FloorToInt((center.y-radius)*2)*2));
        Vector2Int ruID = new Vector2Int(Mathf.Min(mapSize.x-1, Mathf.CeilToInt(center.x+radius)), Mathf.Min(mapSize.y-1, Mathf.CeilToInt((center.y+radius)*2)*2));

        for(int i=ldID.x;i<=ruID.x;++i){
            for(int j=ldID.y;j<=ruID.y;++j){
                if(Vector2.Distance(center, map[i, j].transform.position) <= minDis){
                    minDis = Vector2.Distance(center, map[i, j].transform.position);
                    minID = new Vector2Int(i,j );
                }
            }
        }
        map[minID.x, minID.y].GetComponent<TerrainUnit>().sprite = newSprite;
    }

    // Draw sprite of all units in range
    void DrawSpriteRange(Vector3 center){
        Sprite newSprite = spriteManager.spritePacks[spriteType].sprites[spriteID];

        float radius = spriteBrushSize / 2f;
        Vector2Int ldID = new Vector2Int(Mathf.Max(0, Mathf.FloorToInt(center.x-radius)), Mathf.Max(0, Mathf.FloorToInt((center.y-radius)*2)*2));
        Vector2Int ruID = new Vector2Int(Mathf.Min(mapSize.x-1, Mathf.CeilToInt(center.x+radius)), Mathf.Min(mapSize.y-1, Mathf.CeilToInt((center.y+radius)*2)*2));

        for(int i=ldID.x;i<=ruID.x;++i){
            for(int j=ldID.y;j<=ruID.y;++j){
                if(Vector2.Distance(center, map[i, j].transform.position) <= radius){
                    map[i, j].GetComponent<TerrainUnit>().sprite = newSprite;
                }
            }
        }
    }

    // Draw height of all units in range
    public void DrawHeight(Vector3 center, bool raise){
        Check();
        center.y -= 0.25f;
        float radius = heightBrushSize / 2f;
        Vector2Int ldID = new Vector2Int(Mathf.Max(0, Mathf.FloorToInt(center.x-radius)), Mathf.Max(0, Mathf.FloorToInt((center.y-radius)*2)*2));
        Vector2Int ruID = new Vector2Int(Mathf.Min(mapSize.x-1, Mathf.CeilToInt(center.x+radius)), Mathf.Min(mapSize.y-1, Mathf.CeilToInt((center.y+radius)*2)*2));

        float dir = raise ? 1 : -1;

        for(int i=ldID.x;i<=ruID.x;++i){
            for(int j=ldID.y;j<=ruID.y;++j){
                if(Vector2.Distance(center, map[i, j].transform.position) <= radius){
                    map[i, j].GetComponent<TerrainUnit>().height += strength * Time.deltaTime * 0.5f * dir;
                }
            }
        }
    }

    void Check(){
        if(map == null || map.GetLength(0) != mapSize.x || map.GetLength(1) != mapSize.y){
            Reset();
        }
    }

    public void Reset(){
        GameObject[] terrainUnits = GameObject.FindGameObjectsWithTag("TerrainUnit");
        map = new GameObject[mapSize.x, mapSize.y];

        for(int i=0;i<terrainUnits.Length;++i){
            if(terrainUnits[i].GetComponent<TerrainUnit>().id.x < mapSize.x && terrainUnits[i].GetComponent<TerrainUnit>().id.y < mapSize.y){
                map[terrainUnits[i].GetComponent<TerrainUnit>().id.x, terrainUnits[i].GetComponent<TerrainUnit>().id.y] = terrainUnits[i];
            }
            else{
                DestroyImmediate(terrainUnits[i]);
            }
        }
    }

    public void ReFresh(){
        Reset();

        for(int i=0;i<mapSize.x;++i){
            for(int j=0;j<mapSize.y;++j){
                GameObject go = Instantiate(prefab, map[i, j].transform.position, map[i, j].transform.rotation);
                go.GetComponent<TerrainUnit>().Copy(map[i, j].GetComponent<TerrainUnit>());
                go.transform.SetParent(transform);
                GameObject t = map[i, j];
                map[i, j] = go;
                DestroyImmediate(t);
            }
        }
    }

    public void Clear(){
        GameObject[] terrainUnits = GameObject.FindGameObjectsWithTag("TerrainUnit");
        for(int i=0;i<terrainUnits.Length;++i){
            DestroyImmediate(terrainUnits[i]);
        }
    }
}
