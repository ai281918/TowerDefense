using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TimeManager : MonoBehaviour
{
    private static TimeManager _instance;
    public static TimeManager instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType(typeof(TimeManager)) as TimeManager;
                if (_instance == null)
                {
                    GameObject go = new GameObject("TimeManager");
                    _instance = go.AddComponent<TimeManager>();
                }
            }
            return _instance;
        }
    }

    public UnityEvent<float> onTimeUpdate = new UnityEvent<float>();
    public UnityEvent onDay = new UnityEvent();
    public UnityEvent onNight = new UnityEvent();
    public UnityEvent onDayToNight = new UnityEvent();
    public UnityEvent onNightToDay = new UnityEvent();

    public float dayLength = 10f;
    float timeCnt = 0f;
    float nightTime = 0.5f;
    bool isNight = false;

    // Update is called once per frame
    void Update()
    {
        timeCnt += Time.deltaTime;
        if(timeCnt > dayLength) timeCnt -= dayLength;

        float timeRate = timeCnt / dayLength;

        onTimeUpdate.Invoke(timeRate);

        // Day
        if(timeRate < nightTime){
            onDay.Invoke();
            if(isNight){
                onNightToDay.Invoke();
                isNight = false;
            }
        }
        
        // Night
        else{
            onNight.Invoke();
            if(!isNight){
                onDayToNight.Invoke();
                isNight = true;
            }
        }
    }
}
