using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteManager : MonoBehaviour
{
    static SpriteManager _instance;
    public static SpriteManager instance{
        get{
            if (_instance == null){
                _instance = FindObjectOfType(typeof(SpriteManager)) as SpriteManager;
                if (_instance == null){
                    GameObject go = new GameObject("SpriteManager");
                    _instance = go.AddComponent<SpriteManager>();
                }
            }
            return _instance;
        }
    }

    [HideInInspector]
    public Sprite[][] sprites;
    public Sprite[] brick;
    public Sprite[] dirt;
    public Sprite[] grass;
    public Sprite[] lava;
    public Sprite[] sand;
    public Sprite[] snow;
    public Sprite[] stair;
    public Sprite[] stone;
    public Sprite[] water;
    public Sprite[] wood;

    public string[] spriteName = new string[10]{
        "Brick",
        "Dirt",
        "Grass",
        "Lava",
        "Sand",
        "Snow",
        "Stair",
        "Stone",
        "Water",
        "Wood"
    };

    public void Initialize(){
        if(sprites == null || sprites.Length != spriteName.Length){
            sprites = new Sprite[spriteName.Length][];
            sprites[0] = brick;
            sprites[1] = dirt;
            sprites[2] = grass;
            sprites[3] = lava;
            sprites[4] = sand;
            sprites[5] = snow;
            sprites[6] = stair;
            sprites[7] = stone;
            sprites[8] = water;
            sprites[9] = wood;
        }
    }

    private void Awake() {
        
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

}
