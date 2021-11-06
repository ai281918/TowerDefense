using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Attackable : MonoBehaviour
{
    public int hp = 10;
    public UnityEvent onDeadEvents = new UnityEvent();

    public virtual void AddDamage(int damage){
        hp -= damage;
        if(hp <= 0){
            onDeadEvents.Invoke();
            Destroy(gameObject);
        }
    }
}
