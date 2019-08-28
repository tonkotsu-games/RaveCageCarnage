﻿using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Experimental.VFX;

public class VFXIntensity : MonoBehaviour
{
    private Slider juiceMeter;
    //[SerializeField] VisualEffect Example;
    [SerializeField] ParticleSystem weaponTrail;
    [SerializeField] Material bodyMat;
    [SerializeField] Material capeMat;

    [SerializeField] float weaponTrailMulti;
    [SerializeField] float emissionMulti;
    //[SerializeField] float ExampleMulti;    

    ParticleSystem.MainModule weaponParticleModule;

    void Start()
    {
        juiceMeter = GameObject.FindGameObjectWithTag("JuiceMeter").GetComponent<Slider>();
        weaponParticleModule = weaponTrail.main;
        bodyMat.SetFloat("Vector1_206D3D1A", 1f);
    }

    void Update()
    {
        weaponParticleModule.startSizeMultiplier = weaponTrailMulti * juiceMeter.value;
        //Example.SetFloat("Particle Base Size", ExampleMulti * juiceMeter.value);
    }
}