using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameObjectPool
{
    GameObject prefab;
    Stack<GameObject> pool = new Stack<GameObject>();

    public GameObjectPool(GameObject prefab){
        this.prefab = prefab;
    }

    public GameObject Get(){
        if(pool.Count == 0){
            return MonoBehaviour.Instantiate(prefab);
        }
        return pool.Pop();
    }

    public GameObject Get(Vector3 position){
        GameObject go;
        if(pool.Count == 0){
            go = MonoBehaviour.Instantiate(prefab);
        }
        else{
            go = pool.Pop();
            go.SetActive(true);
        }
        go.transform.position = position;
        return go;
    }

    public GameObject Get(Vector3 position, Quaternion rotation){
        GameObject go;
        if(pool.Count == 0){
            go = MonoBehaviour.Instantiate(prefab);
        }
        else{
            go = pool.Pop();
        }
        go.transform.position = position;
        go.transform.rotation = rotation;
        return go;
    }

    public void Release(GameObject go){
        pool.Push(go);
        go.SetActive(false);
    }
}
