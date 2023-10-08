using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridObject : MonoBehaviour
{
    [SerializeField] float startHealth, health;

    private void Start()
    {
        health = startHealth;
    }

    public void TakeDamage(float damage)
    {
        health -= damage;

        if (health <= 0 )
        {
            Destroy(gameObject);
        }
    }
}
