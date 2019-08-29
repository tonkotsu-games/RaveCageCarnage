﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnhancedSkills : MonoBehaviour
{
    public static EnhancedSkills instance;

    [HideInInspector]
    public enum EnhancedState { First, Second, Active ,Inactive}
    [HideInInspector]
    public enum ActionsToEnhance { Attack,Dash}
    [SerializeField] GameObject enhancedDashHitbox;
    [SerializeField] GameObject projectileSpawn;
    [SerializeField] GameObject projectilePrefab;

    public EnhancedState currentEnhancedState;

    [SerializeField] GameObject spotlights;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        
    }

    private void Start()
    {
        //currentEnhancedState = EnhancedState.Inactive;
    }

    public void ChangeEnhancedState(EnhancedState requestedState)
    {
        if(requestedState == currentEnhancedState)
        {
            //Debug.Log("Already in Enhanced State: " + requestedState);
            return;
        }

        else
        {
            switch (requestedState)
            {
                case EnhancedState.First:
                    currentEnhancedState = EnhancedState.First;
                    spotlights.GetComponent<SpotlightGroup>().EnableLights(0);
                    break;
                case EnhancedState.Second:
                    currentEnhancedState = EnhancedState.Second;
                    spotlights.GetComponent<SpotlightGroup>().EnableLights(1);
                    if (TiffanyController.instance == null)
                    {
                        Debug.Log("NO TIFFANY IN SCENE");
                    }
                    else
                    {
                        TiffanyController.instance.ChangeTiffState(TiffanyController.TiffStates.FocusAttention);
                    }
                    break;
                case EnhancedState.Active:

                    currentEnhancedState = EnhancedState.Active;
                    Debug.Log("Enhanced State now " + currentEnhancedState);
                    spotlights.GetComponent<SpotlightGroup>().EnableLights(2);
                    break;
                case EnhancedState.Inactive:

                    currentEnhancedState = EnhancedState.Inactive;
                    spotlights.GetComponent<SpotlightGroup>().DisableAllActiveLights();
                    Debug.Log("Enhanced State now " + currentEnhancedState);
                    break;
            }
        }
    }

    public void UseEnhancedSkill (ActionsToEnhance baseSkill)
    {
        //Add to the tracker for specials used (for scoreboard)
       //Debug.Log("Adding score");
       //ScoreTracker.instance.statContainer[2]++;
        if(baseSkill == ActionsToEnhance.Dash)
        {
            EnhanceDash();
        }

        else if(baseSkill == ActionsToEnhance.Attack)
        {
            EnhanceHit();
        }
        ChangeEnhancedState(EnhancedState.Inactive);
        Debug.Log("Using Enhanced " + baseSkill);
    }


    public void EnhanceDash()
    { 
        enhancedDashHitbox.SetActive(true);
    }

    public void EnhanceHit()
    {
        Debug.Log("SpawningProjectile");
        Instantiate(projectilePrefab, projectileSpawn.transform.position,projectileSpawn. transform.rotation);
    }

    // Called through AnimEvent at the end of the dash
    public void DisableDashHit()
    {
        enhancedDashHitbox.SetActive(false);
    }
}
