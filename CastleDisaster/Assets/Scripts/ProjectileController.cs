using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.Windows;
using static UnityEngine.GraphicsBuffer;

public class ProjectileController : MonoBehaviour
{
    [SerializeField] Rigidbody rb;
    [SerializeField] Vector3 targetPosition;
    Vector3 dir;
    [SerializeField] float shootForce = 30;
    float damage;
    [SerializeField] GameObject originGameObject;

    public void SetProjectileData(float damage, GameObject originGameObject, float shootForce)
    {
        this.damage = damage;
        this.originGameObject = originGameObject;
        this.shootForce = shootForce;
    }

    public void SetTargetPosition(Vector3 position)
    {
        int yPosOffset = (int)Vector3.Distance(originGameObject.transform.position, targetPosition);
        targetPosition = new Vector3(position.x, position.y + 7.5f, position.z);
    }

    private void Start()
    {
        dir = (targetPosition - transform.position).normalized;
        rb.velocity = dir * shootForce;
    }


    private void OnTriggerEnter(Collider other)
    {
        print(other.transform.root.gameObject.name);

        if (other.transform.root.gameObject == originGameObject || originGameObject == null)
            return;

        if (other.gameObject.tag == "Building")
        {
            other.gameObject.GetComponent<GridObject>().TakeDamage(damage);
            transform.parent = other.transform;
            rb.isKinematic = true;
        }

        if (other.transform.root.gameObject.tag == "Unit")
        {
            print("hit unit!");
            other.transform.root.GetComponent<UnitController>().TakeDamage(damage);
            Destroy(gameObject);
        }        
    }
}
