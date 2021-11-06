using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rabbit : Enemy
{
    public GameObject hitEffect;
    static GameObjectPool gameObjectPool;

    private void Awake() {
        if(gameObjectPool == null){
            gameObjectPool = new GameObjectPool(hitEffect);
        }
        onDeadEvents.AddListener(DeadEffect);
    }

    // Update is called once per frame
    new protected void Update()
    {
        base.Update();
    }

    public void DeadEffect(){
        GameObject go = gameObjectPool.Get(transform.position);
        go.GetComponent<HitEffect>().Play(gameObjectPool);
    }
}
