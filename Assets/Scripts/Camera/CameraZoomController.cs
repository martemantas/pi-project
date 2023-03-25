using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraZoomController : MonoBehaviour
{
    public Camera camera;
    public GameObject fc;
    public GameObject mc;
    private float targetZoom;
    private float zoomFactor = 4f;
    private float zoomSpeed = 10f;

    // Start is called before the first frame update
    void Start()
    {
        targetZoom = camera.orthographicSize;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Tab)) {
            fc.SetActive(true);
            mc.SetActive(false);

            float scrollData = Input.GetAxis("Mouse ScrollWheel");
            targetZoom = targetZoom - scrollData * zoomFactor;
            targetZoom = Mathf.Clamp(targetZoom, 4f, 10f);
            camera.orthographicSize = Mathf.Lerp(camera.orthographicSize, targetZoom, Time.deltaTime * zoomSpeed);

        }
        else
        {
            mc.SetActive(true);
            fc.SetActive(false);
        }

    }
}