using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Missile : MonoBehaviour
{
    public float speed = 5f;
    public int atk = 1;
    Transform target;
    Vector3 direction;
    Vector3 smoothDirection;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(target != null){
            Move();
        }
        else{
            Destroy(gameObject);
        }
    }

    public void SetTarget(Transform target, Vector3 direction){
        this.direction = direction;
        this.target = target;
    }

    void Move(){
        direction = Vector3.SmoothDamp(direction, (target.position - transform.position).normalized, ref smoothDirection, 0.1f).normalized;
        transform.position += direction * speed * Time.deltaTime;
        transform.rotation = Quaternion.Euler(0, 0, Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg);
    }

    private void OnCollisionEnter2D(Collision2D other) {
        if(other.transform == target){
            target.GetComponent<Attackable>().AddDamage(atk);
            Destroy(gameObject);
        }
    }
}
