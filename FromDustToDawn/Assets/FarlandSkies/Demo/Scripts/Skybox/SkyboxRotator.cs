﻿using UnityEngine;

public class SkyboxRotator : MonoBehaviour
{
    public float RotationPerSecond = 1;
    public bool _rotate;

    protected void Update()
    {
        if (_rotate) RenderSettings.skybox.SetFloat("_Rotation", Time.time * RotationPerSecond);
    }

}