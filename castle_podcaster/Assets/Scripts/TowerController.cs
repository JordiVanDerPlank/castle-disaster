using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TowerController : MonoBehaviour
{
    [SerializeField] float attackSpeed, attackDistance, attackDamage;
    [SerializeField] UnitController target;
    float currentTime;

    [SerializeField] GameObject projectilePrefab;
    [SerializeField] Transform projectileSpawnPosition;

    private void Start()
    {
        if (DebugController.Instance.useDebugValues)
        {
            attackSpeed = DebugController.Instance.towerAttackSpeed;
            attackDistance = DebugController.Instance.towerReach;
            attackDamage = DebugController.Instance.towerDamage;
        }
    }

    private void Update()
    {
        if (target == null)
        {
            FindTarget();
            return;
        }

        if (currentTime >= attackSpeed && target != null)
        {
            //target.TakeDamage(attackDamage);
            ProjectileController _newProjectile = Instantiate(projectilePrefab, projectileSpawnPosition.position, Quaternion.identity).GetComponent<ProjectileController>();
            _newProjectile.SetProjectileData(attackDamage, gameObject);
            //_newProjectile.SetTargetPosition(target.transform.position);
            _newProjectile.SetTarget(target.transform);
            currentTime = 0;
        }

        currentTime += Time.deltaTime;
    }

    void FindTarget()
    {
        List<UnitController> enemies = FindObjectsOfType<UnitController>().ToList();

        if (enemies.Count <= 0)
            return;

        target = enemies.OrderBy(enemy => Vector3.Distance(transform.position, enemy.transform.position)).First();
        if (Vector3.Distance(transform.position, target.transform.position) > attackDistance)
            target = null;
    }
}
