using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubeBajaY : MonoBehaviour
{

    float posOriginalY;

    // Start is called before the first frame update
    void Start()
    {
        posOriginalY = this.transform.position.y;
    }

    [SerializeField]
    float longitud = 1f;

    float aRadianes = Mathf.PI * 0.5f; // 90 grados en radianes

    // Update is called once per frame
    void Update()
    {
        Vector3 nuevaPos = this.transform.position;

        nuevaPos.y = posOriginalY + Mathf.Sin(Time.time * aRadianes) * longitud;

        this.transform.position = nuevaPos;
    }
}
