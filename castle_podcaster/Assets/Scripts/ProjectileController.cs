using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileController : MonoBehaviour
{
    [SerializeField] Rigidbody rb;
    float damage;
    [SerializeField] GameObject originGameObject;

    public void SetProjectileData(float damage, GameObject originGameObject)
    {
        this.damage = damage;
        this.originGameObject = originGameObject;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.root.gameObject == originGameObject || originGameObject == null)
            return;

        if (other.gameObject.tag == "Building")
        {
            if (originGameObject.tag == "Building")
                return;
            other.gameObject.GetComponent<GridObject>().TakeDamage(damage);
            transform.parent = other.transform;
            rb.isKinematic = true;
        }

        if (other.transform.root.gameObject.tag == "Unit")
        {
            other.transform.root.GetComponent<UnitController>().TakeDamage(damage, null, originGameObject.GetComponent<GridObject>());
            Destroy(gameObject);
        }
    }

    public void SetTarget(Transform _target)
    {
        target = _target;
    }

    Transform target;
    [SerializeField] float projectileSpeed;

    private void Update()
    {
        try
        {
            Vector3 direction = (target.position - transform.position).normalized;
            Vector3 velocity = direction * projectileSpeed;
            rb.velocity = velocity;
            transform.LookAt(target);
        }
        catch { }
    }
}
