using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SurfaceToAirMissile : Weapon
{
    public GameObject missilePrefab;

    protected override void Attack()
    {
        for(int i=0;i<targets.Count;++i){
            GameObject missile = Instantiate(missilePrefab, transform.position, Quaternion.identity);
            missile.GetComponent<Missile>().SetTarget(targets[i], ((targets[i].position - transform.position).normalized + new Vector3(Random.Range(-0.3f, 0.3f), Random.Range(-0.3f, 0.3f), 0f).normalized));
        }
    }
}
