using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeguirXZ : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        este = this.transform;
    }
    private Transform este;
    public Transform objetivo;

    [SerializeField]
    bool seguirRotacion = false;

    // Update is called once per frame
    void FixedUpdate()
    {
        if (objetivo != null) {
            Vector3 pos = este.position;
            Vector3 objPos = objetivo.position;
            pos.x = objPos.x;
            pos.z = objPos.z;
            este.position = pos;
            if (seguirRotacion) {
                este.localRotation = objetivo.localRotation;
            }
        }
    }
}
