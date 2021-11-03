using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SurfaceToAirMissile : Weapon
{
    public GameObject missilePrefab;
    public static GameObjectPool missilePool;

    new protected void Awake() {
        base.Awake();

        if(missilePool == null){
            missilePool = new GameObjectPool(missilePrefab);
        }
    }

    protected override void Attack()
    {
        for(int i=0;i<targets.Count;++i){
            // GameObject missile = Instantiate(missilePrefab, transform.position, Quaternion.identity);
            GameObject missile = missilePool.Get(transform.position);
            missile.GetComponent<Missile>().SetTarget(targets[i], ((targets[i].position - transform.position).normalized + new Vector3(Random.Range(-0.3f, 0.3f), Random.Range(-0.3f, 0.3f), 0f).normalized));
        }
    }
}
