using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

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

    [SerializeField] NavMeshAgent agent;
    [SerializeField] GridObject kingTower;
    [SerializeField] float distanceToTarget;

    private void Awake()
    {
        kingTower = GameObject.Find("Tower_King").GetComponent<GridObject>();
    }

    private void Update()
    {
        agent.destination = kingTower.transform.position;

        if (target == null)
        {
            FindTarget();
        }

        if (agent.remainingDistance <= agent.stoppingDistance)
        {
            target = kingTower;
        }

        distanceToTarget = Vector3.Distance(transform.position, target.transform.position);

        if (currentTime >= attackSpeed && target != null)
        {
            if (Vector3.Distance(transform.position, target.transform.position) > attackDistance)
            {
                currentTime = 0;
                return;
            }

            switch (enemyType)
            {
                case UnitType.swordsman:
                    target.TakeDamage(attackDamage);
                    break;
                case UnitType.archer:
                    ProjectileController _newProjectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity).GetComponent<ProjectileController>();
                    _newProjectile.SetProjectileData(attackDamage, gameObject);
                    _newProjectile.SetTarget(target.transform);
                    break;
            }
            
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

    void FindTarget()
    {
        List<GridObject> buildings = FindObjectsOfType<GridObject>().ToList();

        if (buildings.Count <= 0)
            return;

        target = buildings.OrderBy(building => Vector3.Distance(transform.position, building.transform.position)).First();
    }
}
