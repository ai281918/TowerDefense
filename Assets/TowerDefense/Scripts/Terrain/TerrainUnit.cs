using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainUnit : MonoBehaviour
{
    public Vector3 initialPosition;
    [SerializeField]
    public float _height = 0f;
    public SpriteRenderer spriteRenderer;
    public Vector2Int id;

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

    public void Initialize(Vector2Int id){
        this.id = id;
        initialPosition = transform.position;
    }

    public void Copy(TerrainUnit another){
        initialPosition = another.initialPosition;
        height = another.height;
        sprite = another.sprite;
        id = another.id;
    }
}
