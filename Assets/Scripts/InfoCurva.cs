using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfoCurva : MonoBehaviour
{

    public string tipo;
    public GameObject izq, der;
    public GameObject A, B, C;

    // Start is called before the first frame update
    void Start() {
        SetTipo(tipo);
    }

    public void SetTipo(string t){
        tipo = t;
        switch (tipo){
            case "A":
                izq.SetActive(false);
                der.SetActive(false);
                break;
            case "B":
                izq.SetActive(false);
                der.SetActive(true);
                break;
            case "C":
                izq.SetActive(true);
                der.SetActive(false);
                break;
        };
    }

    public void Eliminar() {

        if (!izq.activeInHierarchy)  Destroy(izq);     // Destruir las paredes
        if (!der.activeInHierarchy)  Destroy(der);     

        if (A != null) Destroy(A);                     // Destruir los tipos de apoyo
        if (B != null) Destroy(B);     
        if (C != null) Destroy(C);    

        Destroy(this);                                 // Destruir el componente de la vía

    }
}
