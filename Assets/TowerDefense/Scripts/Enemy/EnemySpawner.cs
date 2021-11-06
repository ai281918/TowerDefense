using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public Color color = Color.cyan;
    [Range(0f, 10f)]
    public float spawnTime = 1f;
    public List<Vector3> nodes = new List<Vector3>();
    public List<GameObject> enemyPrefabs = new List<GameObject>();

    float timeCount = 0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        SpawnEnemy();
    }

    public void AddNode(Vector3 pos){
        int id = 0;
        float dis = 99999f;
        for(int i=0;i<nodes.Count;++i){
            if(i == 0){
                dis = PointToSegmentDistance(pos, transform.position, nodes[0]);
            }
            else{
                if(PointToSegmentDistance(pos, nodes[i-1], nodes[i]) < dis){
                    dis = PointToSegmentDistance(pos, nodes[i-1], nodes[i]);
                    id = i;
                }
            }
        }
        if(nodes.Count != 0 && Vector2.Distance(pos, nodes[id]) <= dis){
            id++;
        }
        nodes.Insert(id, pos);
    }

    public float PointToSegmentDistance(Vector2 p, Vector2 a, Vector2 b){
        float dot = Vector2.Dot(b-a, p-a);
        if(dot <= 0){
            return Vector2.Distance(p, a);
        }

        float d2 = (a - b).sqrMagnitude;
        if(dot >= d2){
            return Vector2.Distance(p, b);
        }

        float r = dot / d2;
        float px = a.x + (b.x - a.x) * r;
        float py = a.y + (b.y - a.y) * r;
        return Vector2.Distance(p, new Vector2(px, py));
    }

    public void DeleteNode(Vector3 pos){
        if(nodes.Count == 0){
            return;
        }

        int id = 0;
        float dis = 999f;

        for(int i=0;i<nodes.Count;++i){
            if(Vector2.Distance(pos, nodes[i]) < dis){
                dis = Vector2.Distance(pos, nodes[i]);
                id = i;
            }
        }

        nodes.RemoveAt(id);
    }

    void SpawnEnemy(){
        timeCount += Time.deltaTime;

        if(timeCount >= spawnTime){
            GameObject e = Instantiate(enemyPrefabs[Random.Range(0, enemyPrefabs.Count)], transform.position, Quaternion.identity);
            e.GetComponent<Enemy>().Initialize(nodes);
            timeCount = 0f;
        }
    }
}
