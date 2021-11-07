using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Attackable
{
    int targetID = 0;
    public float speed = 5f;
    List<Vector3> nodes;

    public void Initialize(List<Vector3> nodes){
        this.nodes = nodes;
    }

    // Update is called once per frame
    protected void Update()
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

            Vector2 dir = ((Vector2)(nodes[targetID] - transform.position)).normalized * moveDis;
            transform.position = new Vector3(transform.position.x + dir.x, transform.position.y + dir.y, transform.position.y + dir.y);
            transform.rotation = dir.x > 0f ? Quaternion.Euler(0, 180, 0) : Quaternion.Euler(0, 0, 0);
        }
    }
}
