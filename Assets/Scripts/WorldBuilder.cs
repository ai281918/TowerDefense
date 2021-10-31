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
    public Vector2[] spriteScrollPos = new Vector2[10];
    public bool[] spriteFoldout = new bool[10];

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
    public float strength = 0f;
    public GameObject prefab;
    // Map
    GameObject[,] map;

    Vector2[] pointBrushTarget = new Vector2[5]{
        new Vector2(0, 0),
        new Vector2(0, 0.5f),
        new Vector2(1, 0),
        new Vector2(1, 0.5f),
        new Vector2(0.5f, 0.25f)
    };

    Vector2Int[] pointBrushTargetID = new Vector2Int[5]{
        new Vector2Int(0, 0),
        new Vector2Int(0, 2),
        new Vector2Int(1, 0),
        new Vector2Int(1, 2),
        new Vector2Int(0, 1)
    };

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
        center.y -= 0.25f;
        
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
        int minID = 0;
        Vector2 ldCorner = new Vector2(Mathf.Floor(center.x), Mathf.Floor(center.y*2)/2);
        for(int i=0;i<5;++i){
            if(Vector2.Distance(center, ldCorner + pointBrushTarget[i]) < minDis){
                minDis = Vector2.Distance(center, ldCorner + pointBrushTarget[i]);
                minID = i;
            }
        }

        Vector2Int id = new Vector2Int(Mathf.FloorToInt(center.x), Mathf.FloorToInt(center.y*2)*2) + pointBrushTargetID[minID];
        if(id.x >= 0 && id.x < mapSize.x && id.y >= 0 && id.y < mapSize.y){
            map[id.x, id.y].GetComponent<SpriteRenderer>().sprite = newSprite;
        }
    }

    // Draw sprite of all units in range
    void DrawSpriteRange(Vector3 center){
        Sprite newSprite = SpriteManager.instance.sprites[spriteType][spriteID];

        float radius = spriteBrushSize / 2f;
        Vector2Int ldID = new Vector2Int(Mathf.Max(0, Mathf.FloorToInt(center.x-radius)), Mathf.Max(0, Mathf.FloorToInt((center.y-radius)*2)*2));
        Vector2Int ruID = new Vector2Int(Mathf.Min(mapSize.x-1, Mathf.CeilToInt(center.x+radius)), Mathf.Min(mapSize.y-1, Mathf.CeilToInt((center.y+radius)*2)*2));

        for(int i=ldID.x;i<=ruID.x;++i){
            for(int j=ldID.y;j<=ruID.y;++j){
                if(j%2==0){
                    if(Vector2.Distance(center, new Vector2(i, j*0.25f)) <= radius){
                        map[i, j].GetComponent<SpriteRenderer>().sprite = newSprite;
                    }
                }
                else{
                    if(Vector2.Distance(center, new Vector2(i+0.5f, j*0.25f)) <= radius){
                        map[i, j].GetComponent<SpriteRenderer>().sprite = newSprite;
                    }
                }
            }
        }
    }

    // Draw height of all units in range
    public void DrawHeight(Vector3 center, bool raise){
        center.y -= 0.25f;
        float radius = heightBrushSize / 2f;
        Vector2Int ldID = new Vector2Int(Mathf.Max(0, Mathf.FloorToInt(center.x-radius)), Mathf.Max(0, Mathf.FloorToInt((center.y-radius)*2)*2));
        Vector2Int ruID = new Vector2Int(Mathf.Min(mapSize.x-1, Mathf.CeilToInt(center.x+radius)), Mathf.Min(mapSize.y-1, Mathf.CeilToInt((center.y+radius)*2)*2));

        float dir = raise ? 1 : -1;

        for(int i=ldID.x;i<=ruID.x;++i){
            for(int j=ldID.y;j<=ruID.y;++j){
                if(j%2==0){
                    if(Vector2.Distance(center, new Vector2(i, j*0.25f)) <= radius){
                        map[i, j].transform.position = new Vector3(map[i, j].transform.position.x, Mathf.Clamp(map[i, j].transform.position.y + strength * Time.deltaTime * 0.5f * dir, j*0.25f-0.5f, j*0.25f+0.5f), map[i, j].transform.position.z);
                    }
                }
                else{
                    if(Vector2.Distance(center, new Vector2(i+0.5f, j*0.25f)) <= radius){
                        map[i, j].transform.position = new Vector3(map[i, j].transform.position.x, Mathf.Clamp(map[i, j].transform.position.y + strength * Time.deltaTime * 0.5f * dir, j*0.25f-0.5f, j*0.25f+0.5f), map[i, j].transform.position.z);
                    }
                }
            }
        }
    }
}
