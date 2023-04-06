using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public static CameraController instance;

    public Room currRoom;
    public float moveSpeed;
    public bool roomCenter = false;

    public Transform target;
    public float smoothing;
    public Vector2 maxBoundsIndex;
    public Vector2 minBoundsIndex;
    private Vector2 maxBounds;
    private Vector2 minBounds;

    void Awake()
    {
        instance = this;
        maxBounds = maxBoundsIndex;
        minBounds = minBoundsIndex;
    }

    void Update()
    {
        if (!roomCenter)
            UpdatePosition();
        else
        {
            if (target == null)
                target = GameObject.FindWithTag("Player").transform;
            FollowPlayer();
        }
    }

    public void UpdatePosition()
    {
        if (currRoom == null)
        {
            return;
        }

        Vector3 targetPos = GetCameraTargetPosition();

        transform.position = Vector3.MoveTowards(transform.position, targetPos, Time.deltaTime * moveSpeed);

        if (transform.position == targetPos)
            roomCenter = true;
    }

    Vector3 GetCameraTargetPosition()
    {
        if (currRoom == null)
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

    private void FollowPlayer()
    {
        if (transform.position != target.position)
        {
            Vector3 targetPosition = new Vector3(target.position.x, target.position.y, transform.position.z);
            targetPosition.x = Mathf.Clamp(targetPosition.x, minBounds.x, maxBounds.x);
            targetPosition.y = Mathf.Clamp(targetPosition.y, minBounds.y, maxBounds.y);
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, Time.deltaTime * moveSpeed);
        }
    }

    public void UpdateCameraData(Room room)
    {
        currRoom = room;
        roomCenter = false;
        Vector3 center = currRoom.GetRoomCenter();
        minBounds = new Vector2(minBoundsIndex.x + center.x, minBoundsIndex.y + center.y);
        maxBounds = new Vector2(maxBoundsIndex.x + center.x, maxBoundsIndex.y + center.y);
    }
}