using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleDestruct : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        ParticleSystem ps = GetComponent<ParticleSystem>();
        Destroy(gameObject, ps.main.duration);
    }

}
