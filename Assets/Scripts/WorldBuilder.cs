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
    Vector2Int _mapSize;
    public Vector2Int mapSize{
        get{
            return _mapSize;
        }
        set{
            value.y += value.y-1;
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
    GameObject[,] map;

    public void Initialize(){
        if(spriteScrollPos == null || spriteScrollPos.Length != SpriteManager.instance.spriteName.Length){
            spriteScrollPos = new Vector2[SpriteManager.instance.spriteName.Length];
            spriteFoldout = new bool[SpriteManager.instance.spriteName.Length];
        }
    }

    public void UpdateMapSize(Vector2Int newSize){
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
                    m[i, j].GetComponent<MapUnit>().Initialize(new Vector2Int(i, j));
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
        Sprite newSprite = SpriteManager.instance.sprites[spriteType][spriteID];

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
        map[minID.x, minID.y].GetComponent<MapUnit>().sprite = newSprite;
    }

    // Draw sprite of all units in range
    void DrawSpriteRange(Vector3 center){
        Sprite newSprite = SpriteManager.instance.sprites[spriteType][spriteID];

        float radius = spriteBrushSize / 2f;
        Vector2Int ldID = new Vector2Int(Mathf.Max(0, Mathf.FloorToInt(center.x-radius)), Mathf.Max(0, Mathf.FloorToInt((center.y-radius)*2)*2));
        Vector2Int ruID = new Vector2Int(Mathf.Min(mapSize.x-1, Mathf.CeilToInt(center.x+radius)), Mathf.Min(mapSize.y-1, Mathf.CeilToInt((center.y+radius)*2)*2));

        for(int i=ldID.x;i<=ruID.x;++i){
            for(int j=ldID.y;j<=ruID.y;++j){
                if(Vector2.Distance(center, map[i, j].transform.position) <= radius){
                    map[i, j].GetComponent<MapUnit>().sprite = newSprite;
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
                    map[i, j].GetComponent<MapUnit>().height += strength * Time.deltaTime * 0.5f * dir;
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
        GameObject[] mapUnits = GameObject.FindGameObjectsWithTag("MapUnit");
        map = new GameObject[mapSize.x, mapSize.y];

        for(int i=0;i<mapUnits.Length;++i){
            if(mapUnits[i].GetComponent<MapUnit>().id.x < mapSize.x && mapUnits[i].GetComponent<MapUnit>().id.y < mapSize.y){
                map[mapUnits[i].GetComponent<MapUnit>().id.x, mapUnits[i].GetComponent<MapUnit>().id.y] = mapUnits[i];
            }
            else{
                DestroyImmediate(mapUnits[i]);
            }
        }
    }
}
