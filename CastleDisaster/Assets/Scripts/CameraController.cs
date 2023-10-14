using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] float minOrtographicSize, maxOrtographicSize;
    [SerializeField] float startZoomValue, zoomValue, zoomSpeed;

    private void Start()
    {
        Camera.main.orthographicSize = startZoomValue;
        zoomValue = Camera.main.orthographicSize;
    }

    float h, v;
    [SerializeField] float moveSpeed;

    // Update is called once per frame
    void Update()
    {
        HandleZoom();

        GetInput();
        Move();
    }
    void HandleZoom()
    {
        if (zoomValue < minOrtographicSize)
            zoomValue = minOrtographicSize;
        if (zoomValue > maxOrtographicSize)
            zoomValue = maxOrtographicSize;
        Camera.main.orthographicSize = zoomValue;

        if (Input.GetAxis("Mouse ScrollWheel") > 0f) // backwards
        {
            zoomValue -= zoomSpeed;
        }
        else if (Input.GetAxis("Mouse ScrollWheel") < 0f) // forwards
        {
            zoomValue += zoomSpeed;
        }
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
