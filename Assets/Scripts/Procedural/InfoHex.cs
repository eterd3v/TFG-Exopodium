using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfoHex : MonoBehaviour
{

    public TriggerPista tPista;
    public string tipo;
    public GameObject izq, der;
    public GameObject a, b, c;


    // Start is called before the first frame update
    void Start() {
        SetTipo(tipo);
    }

    public void SetTipo(string t){
        //Debug.Log("Set tipo '" + t + "' en recta");
        tipo = t;
        switch (tipo){
            case "a":
                izq.SetActive(true);
                der.SetActive(true);
                break;
            case "b":
                izq.SetActive(false);
                der.SetActive(true);
                break;
            case "c":
                izq.SetActive(true);
                der.SetActive(false);
                break;
        };
    }

    public void Eliminar() {

        if (!izq.activeInHierarchy)  Destroy(izq);     // Destruir las paredes
        if (!der.activeInHierarchy)  Destroy(der);     

        if (a != null) Destroy(a);                     // Destruir los tipos de apoyo
        if (b != null) Destroy(b);     
        if (c != null) Destroy(c);    

        Destroy(this);                                 // Destruir el componente de la vía

    }

}
