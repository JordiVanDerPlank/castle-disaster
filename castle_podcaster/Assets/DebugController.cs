using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugController : MonoBehaviour
{
    public static DebugController Instance;
    private void Awake()
    {
        Instance = this;
    }
    public bool useDebugValues;

    [Space]

    [Header("Unit Spawning")]
    public int amountOfUnitsToSpawn;

    [Header("King Tower Variables")]
    public float kingTowerHealth;

    [Header("Tower Variables")]
    public float towerHealth;
    public float towerReach;
    public float towerDamage;
    [Tooltip("How much time (s) between attacks")] public float towerAttackSpeed;

    [Header("Unit Variables")]
    public float swordsmanHealth;
    public float swordsmanDamage;
    public float swordsmanRange;
    public float swordsmanAttackSpeed;

    public float archerHealth;
    public float archerDamage;
    public float archerRange;
    public float archerAttackSpeed;
}
