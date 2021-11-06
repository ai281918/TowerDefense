using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPoolManager : MonoBehaviour
{
    static EnemyPoolManager _instance;
    public static EnemyPoolManager instance{
        get{
            if (_instance == null){
                _instance = FindObjectOfType(typeof(EnemyPoolManager)) as EnemyPoolManager;
                if (_instance == null){
                    GameObject go = new GameObject("EnemyPoolManager");
                    _instance = go.AddComponent<EnemyPoolManager>();
                }
            }
            return _instance;
        }
    }

    public List<GameObject> enemies;
    public List<GameObjectPool> enemyPools;

    private void Awake() {
        enemyPools = new List<GameObjectPool>();
        for(int i=0;i<enemies.Count;++i){
            enemyPools.Add(new GameObjectPool(enemies[i]));
        }
    }

    public GameObject Get(int id){
        GameObject go = enemyPools[id].Get();
        go.GetComponent<Enemy>().Reset();
        return go;
    }

    public GameObject Get(int id, Vector3 position){
        GameObject go = enemyPools[id].Get(position);
        go.GetComponent<Enemy>().Reset();
        return go;
    }

    public GameObject Get(int id, Vector3 position, Quaternion rotation){
        GameObject go = enemyPools[id].Get(position, rotation);
        go.GetComponent<Enemy>().Reset();
        return go;
    }

    public void Release(int id, GameObject go){
        enemyPools[id].Release(go);
    }
}
