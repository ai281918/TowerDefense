using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class SkyColor : MonoBehaviour
{
    public Gradient dayColor;
    Light2D light2D;
    
    // Start is called before the first frame update
    void Start()
    {
        light2D = GetComponent<Light2D>();
        TimeManager.instance.onTimeUpdate.AddListener(UpdateLightColor);
    }

    public void UpdateLightColor(float timeRate){
        Color skyColor = dayColor.Evaluate(timeRate);
        light2D.color = skyColor;
    }
}
