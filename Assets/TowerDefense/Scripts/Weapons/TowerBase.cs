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
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnMouseDown() {
        towerBaseUI.PlayUIAnimation(this);
    }

    public void CreateTower(int id){
        Instantiate(towerPrefabs[id], transform.position, Quaternion.identity);
        GetComponent<BoxCollider2D>().enabled = false;
    }
}
