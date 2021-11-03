using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct SpritePack{
    public string name;
    public Sprite[] sprites;
    public int length{
        get{
            if(sprites == null){
                return 0;
            }
            return sprites.Length;
        }
    }
}

public class SpriteManager : MonoBehaviour
{
    [SerializeField]
    public SpritePack[] spritePacks;

    public int spriteTypeNum{
        get{
            if(spritePacks == null){
                return 0;
            }
            return spritePacks.Length;
        }
    }
}
