using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    public GameObject g1, g2;
    public WorldBuilder worldBuilder;
    GameObject[,] m;
    // Start is called before the first frame update
    void Start()
    {
        m = new GameObject[worldBuilder.mapSize.x, worldBuilder.mapSize.y];
        for(int i=0;i<worldBuilder.mapSize.x;++i){
            for(int j=0;j<worldBuilder.mapSize.y;++j){
                if(m[i, j] == null){
                    if(j%2==0){
                        m[i, j] = Instantiate(g1, new Vector3(i, j*0.25f, j*0.25f), Quaternion.identity);
                        m[i, j].transform.SetParent(transform);
                    }
                    else{
                        m[i, j] = Instantiate(g1, new Vector3(i+0.5f, j*0.25f, j*0.25f), Quaternion.identity);
                        m[i, j].transform.SetParent(transform);
                    }
                    m[i, j].GetComponent<TerrainUnit>().Initialize(new Vector2Int(i, j), Vector2Int.zero);
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
