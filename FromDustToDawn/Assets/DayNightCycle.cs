using System;
using System.Collections.Generic;
using UnityEngine;

public class DayNightCycle : MonoBehaviour
{
    //public List<Material> skyMaterials;

    //[Header("   Day & Night parameters")]
    //public float dayDuration = 120f;


    //private float currentDuration;


    //private void Update()
    //{
    //    if (currentDuration < dayDuration)
    //    {
    //        currentDuration += Time.deltaTime;

    //        if(currentDuration <= 10)
    //        {
    //            print(10 / currentDuration);
    //            RenderSettings.skybox.Lerp(skyMaterials[0], skyMaterials[1], currentDuration);
    //            //RenderSettings.skybox = skyMaterials[1];
    //        }

    //    }
    //    else
    //    {
    //        currentDuration = 0;
    //    }


    //}
    public Material skybox;
    public float ActualTime;



    private void Update()
    {
        ActualTime = DateTime.Now.Hour;
        float normalizedTime = Mathf.InverseLerp(0, 24, ActualTime);
        float ValueIncreaseTime = Mathf.Lerp(skybox.GetFloat("_CubemapTransition"), normalizedTime, Time.deltaTime);
        skybox.SetFloat("_CubemapTransition", ValueIncreaseTime);



    }
}
