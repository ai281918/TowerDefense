using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerBase : MonoBehaviour
{
    public GameObject[] towerPrefabs = new GameObject[6];
    TowerBaseUI towerBaseUI;

    private void Awake() {
        towerBaseUI = TowerBaseUI.instance;
    }

    // Start is called before the first frame update
    void Start()
    {
        transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.y + 0.01f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnMouseDown() {
        towerBaseUI.PlayUIAnimation(this);
    }

    public void CreateTower(int id){
        Instantiate(towerPrefabs[id], transform.position + new Vector3(0f, 0f, 0.01f), Quaternion.identity);
        GetComponent<BoxCollider2D>().enabled = false;
    }
}
