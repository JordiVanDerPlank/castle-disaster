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
    [SerializeField] Transform kingTower;

    private void Awake()
    {
        kingTower = GameObject.Find("Tower_King").transform;
    }

    private void Update()
    {
        agent.destination = kingTower.position;
        //agent.SetDestination(kingTower.position);
        //agent.Move(Vector3.zero);
        return;
        if (target == null)
        {
            FindTarget();
        }

        if (currentTime >= attackSpeed && target != null)
        {
            //target.TakeDamage(attackDamage);
            ProjectileController _newProjectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity).GetComponent<ProjectileController>();
            _newProjectile.SetProjectileData(attackDamage, gameObject, GetShootForce());
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

    void FindTarget()
    {
        List<GridObject> buildings = FindObjectsOfType<GridObject>().ToList();

        if (buildings.Count <= 0)
            return;

        target = buildings.OrderBy(building => Vector3.Distance(transform.position, building.transform.position)).First();
    }

    float GetShootForce()
    {
        return Vector3.Distance(transform.position, target.transform.position);
    }
}
