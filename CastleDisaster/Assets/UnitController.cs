using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum UnitType
{
    swordsman,
    archer
}

public class UnitController : MonoBehaviour
{
    [SerializeField] UnitType enemyType;
    [SerializeField] float attackSpeed, attackDistance, attackDamage;
    [SerializeField] GridObject target;
    float currentTime;

    [SerializeField] GameObject projectilePrefab;

    private void Update()
    {
        if (currentTime >= attackSpeed && target != null)
        {
            //target.TakeDamage(attackDamage);
            ProjectileController _newProjectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity).GetComponent<ProjectileController>();
            _newProjectile.SetProjectileData(attackDamage, gameObject);
            _newProjectile.SetTargetPosition(target.transform.position);
            currentTime = 0;
        }

        currentTime += Time.deltaTime;
    }

    [SerializeField] float health;

    public void TakeDamage(float damage)
    {
        health -= damage;
        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }
}
