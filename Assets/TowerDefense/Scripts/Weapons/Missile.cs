using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Missile : MonoBehaviour
{
    public float speed = 5f;
    public int atk = 1;
    Transform target;
    Vector2 direction;
    Vector2 smoothDirection;
    public GameObjectPool missilePool;
    SpriteRenderer spriteRenderer;

    private void Awake() {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

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
            // Destroy(gameObject);
            missilePool.Release(gameObject);
        }
    }

    public void SetTarget(Transform target, Vector3 direction){
        this.direction = direction;
        this.target = target;
    }

    void Move(){
        direction = Vector2.SmoothDamp(direction, ((Vector2)(target.position - transform.position)).normalized, ref smoothDirection, 0.1f).normalized;
        Vector2 moveDis = direction * speed * Time.deltaTime;
        transform.position = new Vector3(transform.position.x + moveDis.x, transform.position.y + moveDis.y, transform.position.y + moveDis.y);
        transform.rotation = Quaternion.Euler(0, 0, Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg);
        spriteRenderer.flipY = (transform.rotation.eulerAngles.z > 90f && transform.rotation.z < 270f);
    }

    private void OnCollisionEnter2D(Collision2D other) {
        if(other.transform == target){
            target.GetComponent<Attackable>().AddDamage(atk);
            // Destroy(gameObject);
            missilePool.Release(gameObject);
        }
    }
}
