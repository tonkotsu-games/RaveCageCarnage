﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Character", menuName = "Characters/Character")]
public class CharacterScriptableObject : ScriptableObject
{
    [Header("---Info-------")]
    public string characterName;    

    [Tooltip("Current HP of the chracter.")]
    public int healthCurrent;

    [Header("---Prefab-------")]
    public GameObject characterPrefab;

    [Header("---Stats-------")]
    [Tooltip("Maximum HP of the character.")]
    public int healthMax;
    public float idleDuration;
    [Range(.5f, 5f)]
    public float meleeAttackRange = 3.65f;

    void Init()
    {
        healthCurrent = healthMax;
        Debug.Log("SO Init");
    }
}