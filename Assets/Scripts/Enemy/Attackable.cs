using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attackable : MonoBehaviour
{
    public int hp = 10;

    public void AddDamage(int damage){
        hp -= damage;
        if(hp <= 0){
            Destroy(gameObject);
        }
    }
}
