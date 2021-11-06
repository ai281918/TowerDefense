using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitEffect : MonoBehaviour
{
    GameObjectPool gameObjectPool;
    public float speed = 1f;

    // Update is called once per frame
    void Update()
    {
        transform.position += new Vector3(0f, speed * Time.deltaTime, 0f);
    }

    public void Play(GameObjectPool gameObjectPool){
        this.gameObjectPool = gameObjectPool;
        GetComponent<Animator>().Play("Heart", -1 ,0f);;
    }

    public void Release(){
        gameObjectPool.Release(gameObject);
    }
}
