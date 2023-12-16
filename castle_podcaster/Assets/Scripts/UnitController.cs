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
    [SerializeField] float attackSpeed, attackDistance, attackDamage;
    [SerializeField] float viewDistance;
    [SerializeField] GridObject target;
    [SerializeField] UnitController unitTarget;
    float currentTime;

    [SerializeField] GameObject projectilePrefab;
    [SerializeField] NavMeshAgent agent;
    [SerializeField] float distanceToTarget;

    [SerializeField] bool isEnemyUnit;
    [SerializeField] UnitType enemyType;

    [SerializeField] GridObject kingTower;

    private void Awake()
    {
        kingTower = GameObject.Find("Tower_King").GetComponent<GridObject>();
    }

    public bool IsEnemyUnit()
    {
        return isEnemyUnit;
    }

    private void Start()
    {
        if (DebugController.Instance.useDebugValues)
        {
            if (enemyType == UnitType.swordsman) {
                attackSpeed = DebugController.Instance.swordsmanAttackSpeed;
                attackDistance = DebugController.Instance.swordsmanRange;
                attackDamage = DebugController.Instance.swordsmanDamage;
                health = DebugController.Instance.swordsmanHealth;
            }

            if (enemyType == UnitType.archer)
            {
                attackSpeed = DebugController.Instance.archerAttackSpeed;
                attackDistance = DebugController.Instance.archerRange;
                attackDamage = DebugController.Instance.archerDamage;
                health = DebugController.Instance.archerHealth;
            }
        }
    }

    private void Update()
    {
        if (isEnemyUnit)
        {
            if (unitTarget != null)
            {
                agent.destination = unitTarget.transform.position;
            }
            else
            {
                agent.destination = kingTower.transform.position;

                if (agent.remainingDistance <= agent.stoppingDistance)
                {
                    target = kingTower;
                }
            }
        }

        else
        {
            if (unitTarget != null)
                agent.destination = unitTarget.transform.position;
        }

        if (isEnemyUnit && target == null)
        {
            FindTarget();
        }

        if (!isEnemyUnit && unitTarget == null)
            FindTarget();

        
        if (target != null)
            distanceToTarget = Vector3.Distance(transform.position, target.transform.position);

        if (currentTime >= attackSpeed)
        {
            if (target != null && unitTarget == null)
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
            }

            if (unitTarget != null)
            {
                if (Vector3.Distance(transform.position, unitTarget.transform.position) > attackDistance)
                {
                    print("want to attack, but can't!");
                    currentTime = 0;
                    return;
                }

                switch (enemyType)
                {
                    case UnitType.swordsman:
                        unitTarget.TakeDamage(attackDamage, this, null);
                        break;
                    case UnitType.archer:
                        ProjectileController _newProjectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity).GetComponent<ProjectileController>();
                        _newProjectile.SetProjectileData(attackDamage, gameObject);
                        _newProjectile.SetTarget(unitTarget.transform);
                        break;
                }
            }
            
            currentTime = 0;
        }

        currentTime += Time.deltaTime;
    }

    [SerializeField] float health;

    public void TakeDamage(float damage, UnitController damagedByUnit, GridObject damagedByBuilding)
    {
        health -= damage;
        if (health <= 0)
        {
            Destroy(gameObject);
        }

        else
        {
            print("got hit");
            if (enemyType == UnitType.swordsman)
            {
                if (damagedByBuilding != null)
                {
                    print("by building");
                    target = damagedByBuilding;
                }

                else
                {
                    print("by unit");
                    unitTarget = damagedByUnit;
                }
            }
        }
    }

    void FindTarget()
    {
        if (isEnemyUnit)
        {
            List<GridObject> _buildings = FindObjectsOfType<GridObject>().ToList();

            if (_buildings.Count <= 0)
                return;

            target = _buildings.OrderBy(building => Vector3.Distance(transform.position, building.transform.position)).First();
        }

        else
        {
            List<UnitController> _enemies = FindObjectsOfType<UnitController>().Where(x => x.IsEnemyUnit()).ToList();

            if (_enemies.Count <= 0)
                return;

            unitTarget = _enemies.OrderBy(enemy => Vector3.Distance(transform.position, enemy.transform.position)).First();
            if (Vector3.Distance(transform.position, unitTarget.transform.position) > viewDistance)
            {
                unitTarget = null;
                return;
            }
        }
    }
}
