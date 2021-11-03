using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SearchTargetType{
    Nearest,
    Random,
    MostHp
}

public abstract class Weapon : MonoBehaviour
{
    public float radius = 5f;
    public float spawnTime = 1f;
    public int targetNum = 1;
    public SearchTargetType searchTargetType = SearchTargetType.Nearest;
    protected List<Transform> targets = new List<Transform>();
    public LayerMask targetLayer;

    float timeCount = 0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    protected void Update()
    {
        timeCount += Time.deltaTime;
        if(timeCount >= spawnTime){
            Detect();
            if(targets.Count != 0){
                Attack();
                timeCount = 0f;
            }
        }
    }

    protected abstract void Attack();

    void Detect(){
        targets.Clear();
        Collider2D[] objs = Physics2D.OverlapCircleAll(transform.position, radius, targetLayer);

        if(objs.Length <= targetNum){
            for(int i=0;i<objs.Length;++i){
                targets.Add(objs[i].transform);
            }
            return;
        }

        switch(searchTargetType){
            case SearchTargetType.Nearest:
                for(int i=0;i<targetNum;++i){
                    targets.Add(objs[i].transform);
                }
                for(int i=targetNum;i<objs.Length;++i){
                    int id = 0;
                    while(id < targetNum && Vector2.Distance(transform.position, targets[id].position) < Vector2.Distance(transform.position, objs[i].transform.position)){
                        id++;
                    }
                    if(id < targetNum){
                        targets.Insert(id, objs[i].transform);
                        targets.RemoveAt(targetNum);
                    }
                }
                break;
            case SearchTargetType.Random:
                int cnt = 0;
                while(cnt != targetNum){
                    int id = Random.Range(0, objs.Length);
                    if(objs[id] != null){
                        targets.Add(objs[id].transform);
                        objs[id] = null;
                        cnt++;
                    }
                }
                break;
            case SearchTargetType.MostHp:
                for(int i=0;i<targetNum;++i){
                    targets.Add(objs[i].transform);
                }
                for(int i=targetNum;i<objs.Length;++i){
                    int id = 0;
                    while(id < targetNum && targets[id].GetComponent<Attackable>().hp > objs[i].GetComponent<Attackable>().hp){
                        id++;
                    }
                    if(id < targetNum){
                        targets.Insert(id, objs[i].transform);
                        targets.RemoveAt(targetNum);
                    }
                }
                break;
        }
    }
}
