using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Attackable : MonoBehaviour
{
    public int hp = 10;
    protected int _hp;
    public UnityEvent onDeadEvents = new UnityEvent();

    private void Awake() {
        _hp = hp;
    }

    public void Reset(){
        _hp = hp;
    }

    public virtual void AddDamage(int damage){
        _hp -= damage;
        if(_hp <= 0){
            onDeadEvents.Invoke();
            Destroy(gameObject);
        }
    }
}
