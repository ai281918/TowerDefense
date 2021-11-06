using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TowerBaseUI : MonoBehaviour
{
    static TowerBaseUI _instance;
    public static TowerBaseUI instance{
        get{
            if (_instance == null){
                _instance = FindObjectOfType(typeof(TowerBaseUI)) as TowerBaseUI;
                if (_instance == null){
                    GameObject go = new GameObject("TowerBaseUI");
                    _instance = go.AddComponent<TowerBaseUI>();
                }
            }
            return _instance;
        }
    }

    Animator animator;
    public GameObject[] buttons = new GameObject[6];
    TowerBase towerBase;
    RectTransform rectTransform;

    private void Awake() {
        animator = GetComponent<Animator>();
        rectTransform = GetComponent<RectTransform>();
    }

    private void Start() {
        Hide();
    }

    private void Update() {
        if(Input.GetMouseButtonDown(1)){
            Hide();
        }
    }

    public void PlayUIAnimation(TowerBase towerBase){
        gameObject.SetActive(true);
        this.towerBase = towerBase;
        for(int i=0;i<towerBase.towerPrefabs.Length;++i){
            if(towerBase.towerPrefabs[i] != null){
                buttons[i].SetActive(true);
                buttons[i].GetComponent<Image>().sprite = towerBase.towerPrefabs[i].GetComponent<SpriteRenderer>().sprite;
            }
            else{
                buttons[i].SetActive(false);
            }
        }
        rectTransform.anchoredPosition = Input.mousePosition;
        animator.Play("TowerBaseUIPlay",  -1, 0f);
    }

    public void CreateTower(int id){
        Hide();
        towerBase.CreateTower(id);
    }

    public void Hide(){
        gameObject.SetActive(false);
    }
}
