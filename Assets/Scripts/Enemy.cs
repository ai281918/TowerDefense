using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    int targetID = 0;
    public float speed = 5f;
    List<Vector3> nodes;

    public void Initialize(List<Vector3> nodes){
        this.nodes = nodes;
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }

    void Move(){
        if(targetID < nodes.Count){
            float targetDis = Vector2.Distance(transform.position, nodes[targetID]);
            float moveDis = speed * Time.deltaTime;
            
            if(targetDis < moveDis){
                transform.position = nodes[targetID];
                targetID++;
                return;
            }

            Vector3 dir = (nodes[targetID] - transform.position).normalized;
            transform.position += dir * moveDis;
            transform.rotation = dir.x > 0f ? Quaternion.Euler(0, 180, 0) : Quaternion.Euler(0, 0, 0);
        }
    }
}
