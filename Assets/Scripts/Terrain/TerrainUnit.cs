using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainUnit : MonoBehaviour
{
    public Vector3 initialPosition;
    [SerializeField]
    public float _height = 0f;
    public SpriteRenderer spriteRenderer;
    public GameObject child;
    public Vector2Int id;
    public Vector2Int spriteID;

    public Sprite sprite{
        get{
            return spriteRenderer.sprite;
        }
        set{
            spriteRenderer.sprite = value;
        }
    }

    public float height{
        get{
            return _height;
        }
        set{
            _height = Mathf.Clamp(value, -0.5f, 0.5f);
            transform.position = initialPosition + new Vector3(0, _height, 0);
        }
    }

    public void Initialize(Vector2Int id, Vector2Int spriteID){
        this.id = id;
        this.spriteID = spriteID;
        initialPosition = transform.position;
    }

    public void RefreshChild(GameObject go){
        DestroyImmediate(child);
        GameObject c = Instantiate(go);
        c.transform.SetParent(transform);
        c.transform.localPosition = new Vector3(0, -0.35f, 0);
        child = c;
    }
}
