using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    float h, v;
    [SerializeField] float moveSpeed;

    // Update is called once per frame
    void Update()
    {
        GetInput();
        Move();
    }

    void GetInput()
    {
        h = Input.GetAxisRaw("Horizontal");
        v = Input.GetAxisRaw("Vertical");
    }

    void Move()
    {
        transform.position += new Vector3(h, v, -h).normalized * moveSpeed * Time.deltaTime;
    }
}
