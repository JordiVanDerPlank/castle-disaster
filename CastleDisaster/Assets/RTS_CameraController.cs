using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class RTS_CameraController : MonoBehaviour
{

    [SerializeField] float minFieldOfView, maxFieldOfView;
    [SerializeField] float zoomValue, zoomSpeed;

    private void Start()
    {
        Camera.main.fieldOfView = zoomValue;
        rotY = transform.rotation.y;
    }

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(mainCameraCenterPoint);

        zoomValue = Camera.main.fieldOfView;

        float value = Input.GetAxis("Mouse ScrollWheel");

        if (value > 0)  //zoom out
            zoomValue -= zoomSpeed;
        if (value < 0)  //zoom in
            zoomValue += zoomSpeed;

        if (zoomValue < minFieldOfView)
            zoomValue = minFieldOfView;
        if (zoomValue > maxFieldOfView)
            zoomValue = maxFieldOfView;

        Camera.main.fieldOfView = zoomValue;

        AllowRotate();
        Rotate();

        forward = Camera.main.transform.forward;
        forward.y = 0;
        forward = Vector3.Normalize(forward);

        right = Quaternion.Euler(0, 90, 0) * forward;

        GetInput();
        Move();
    }

    [SerializeField] Transform mainCameraCenterPoint;
    [SerializeField] float camRotationSpeed;
    [SerializeField] bool canRotate;

    [SerializeField] Vector3 currentPos, deltaPos, lastPos;

    void AllowRotate()
    {
        if (Input.GetMouseButtonDown(1))
        {
            lastPos = currentPos;
            canRotate = true;
        }

        if (Input.GetMouseButtonUp(1))
            canRotate = false;
    }

    float rotY;
    void Rotate()
    {
        if (!canRotate)
            return;

        currentPos = Input.mousePosition;
        deltaPos = currentPos - lastPos;
        lastPos = currentPos;

        rotY += deltaPos.x / camRotationSpeed;

        mainCameraCenterPoint.transform.localRotation = Quaternion.Euler(transform.rotation.x, rotY, transform.rotation.z);

    }

    Vector3 forward, right;

    [Header("Player Variables")]
    [SerializeField] float moveSpeed;

    void GetInput()
    {
        h = Input.GetAxis("Horizontal");
        v = Input.GetAxis("Vertical");
    }

    float h;
    float v;

    Vector3 movePos;
    float distance;

    void Move()
    {
        Vector3 direction = new Vector3(h, 0, v);

        Vector3 rightMovement = right * moveSpeed * Time.deltaTime * h;
        Vector3 upMovement = forward * moveSpeed * 1.5f * Time.deltaTime * v;

        mainCameraCenterPoint.transform.position += rightMovement;
        mainCameraCenterPoint.transform.position += upMovement;
    }
}
