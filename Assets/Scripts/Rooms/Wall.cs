using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : MonoBehaviour
{
    public enum WallType
    {
        left, right, top, bottom
    }

    public WallType wallType;
    public GameObject wallCollider;
}
