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

    private void Update()
    {
        if (target == null)
        {
            FindTarget();
        }

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

    void FindTarget()
    {
        List<UnitController> enemies = FindObjectsOfType<UnitController>().ToList();

        if (enemies.Count <= 0)
            return;

        target = enemies.OrderBy(enemy => Vector3.Distance(transform.position, enemy.transform.position)).First();
    }
}
