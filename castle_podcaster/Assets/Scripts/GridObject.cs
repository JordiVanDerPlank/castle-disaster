using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridObject : MonoBehaviour
{
    [SerializeField] bool isKingTower;
    [SerializeField] float startHealth, health;

    private void Start()
    {
        if (DebugController.Instance.useDebugValues)
            startHealth = isKingTower ? DebugController.Instance.kingTowerHealth : DebugController.Instance.towerHealth;

        health = startHealth;
    }

    public void TakeDamage(float damage)
    {
        health -= damage;

        if (health <= 0 )
        {
            Destroy(gameObject);


            //king tower behaviour
            if (isKingTower)
                GameManager.Instance.SetGameOver();
        }
    }
}
