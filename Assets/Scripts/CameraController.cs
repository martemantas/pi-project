using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public static CameraController instance;

    public Room currRoom;
    public float moveSpeed;

    void Awake()
    {
        instance = this;
    }

    void Update()
    {
        UpdatePosition();
    }

    void UpdatePosition()
    {
        if(currRoom == null)
        {
            return;
        }

        Vector3 targetPos = GetCameraTargetPosition();

        transform.position = Vector3.MoveTowards(transform.position, targetPos, Time.deltaTime * moveSpeed);
    }

    Vector3 GetCameraTargetPosition()
    {
        if(currRoom == null)
        {
            return Vector3.zero;
        }

        Vector3 targetPos = currRoom.GetRoomCenter();
        targetPos.z = transform.position.z;

            return targetPos;
    }

    public bool IsSwichingScene()
    {
        return transform.position.Equals(GetCameraTargetPosition()) == false;
    }

    /*public Transform target;
    public float smoothing;
    public Vector2 maxBounds;
    public Vector2 minBounds;

    private void LateUpdate()
    {
        if (transform.position != target.position)
        {
            Vector3 targetPosition = new Vector3(target.position.x, target.position.y, transform.position.z);
            targetPosition.x = Mathf.Clamp(targetPosition.x, minBounds.x, maxBounds.x);
            targetPosition.y = Mathf.Clamp(targetPosition.y, minBounds.y, maxBounds.y);
            transform.position = Vector3.Lerp(transform.position, targetPosition, smoothing);
        }
    }*/
}