using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

public class rotarCocheZ : MonoBehaviour
{

    [SerializeField]
    float segundosAnimacion=7f;

    [SerializeField]
    bool sentidoHorario = true;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 nuevo = this.transform.localEulerAngles;
        nuevo.z += (sentidoHorario ? 360f : -360f) * Time.deltaTime / segundosAnimacion;
        this.transform.localEulerAngles = nuevo; 
    }
}
