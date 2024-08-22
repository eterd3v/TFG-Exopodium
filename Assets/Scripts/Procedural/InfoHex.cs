using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfoHex : MonoBehaviour
{

    public TriggerPista tPista;
    public string tipo;
    public GameObject izq, der, pared;
    public GameObject bordeIzq, bordeDer;
    public GameObject a, b, c;
    public MallaHex suelo;
    public GameObject[] obstaculos;

    void TrasladarX(GameObject obj, float x_) {
        Vector3 aux = obj.transform.localPosition;
        aux.x = x_;
        obj.transform.localPosition = aux;
    }

    void EscalarX(GameObject obj, float x_) {
        Vector3 aux = obj.transform.localScale;
        aux.x = x_;
        obj.transform.localScale = aux;
    }

    // Start is called before the first frame update
    void Start() {
        TrasladarX(pared,-suelo.l/2f);
        EscalarX(pared,suelo.l * 2f);

        TrasladarX(izq,suelo.l);
        EscalarX(izq,suelo.l);

        TrasladarX(der,suelo.l);
        EscalarX(der,suelo.l);

        float zAux = bordeIzq.transform.localPosition.z; 
        bordeIzq.transform.localPosition = new Vector3(suelo.l * 2f,suelo.h, zAux -suelo.h/2);
        bordeIzq.transform.localEulerAngles = new Vector3(0,60,0);
        
        zAux = bordeDer.transform.localPosition.z; 
        bordeDer.transform.localPosition = new Vector3(suelo.l * 2f,suelo.h, zAux +suelo.h/2);
        bordeDer.transform.localEulerAngles = new Vector3(0,-60,0);
        
        EscalarX(bordeIzq,suelo.l);
        EscalarX(bordeDer,suelo.l);

        SetTipo(tipo);
    }

    public void EscogerObstaculoRandom(int aleatorio) {
        int cantidad = obstaculos.Length; 
        if (cantidad > 0) {
            aleatorio = aleatorio%cantidad;
            int i=0;
            while (i < aleatorio)
                obstaculos[i++].SetActive(false);
            obstaculos[i++].SetActive(true);  // aquí i es igual a aleatorio
            while (i < cantidad)
                obstaculos[i++].SetActive(false);
        }
    }

    public void SetTipo(string t){
        //Debug.Log("Set tipo '" + t + "' en recta");

        tipo = t;
        switch (tipo){
            case "a":
                izq.SetActive(true);
                bordeIzq.SetActive(false);
                der.SetActive(true);
                bordeDer.SetActive(false);
                break;
            case "c":
                izq.SetActive(true);
                bordeIzq.SetActive(true);
                der.SetActive(false);
                bordeDer.SetActive(false);
                break;
            case "b":
                izq.SetActive(false);
                bordeIzq.SetActive(false);
                der.SetActive(true);
                bordeDer.SetActive(true);
                break;
        };
    }

    public void Eliminar() {

        if (!izq.activeInHierarchy)         Destroy(izq);           // Destruir las paredes
        if (!der.activeInHierarchy)         Destroy(der);     
        if (!bordeIzq.activeInHierarchy)    Destroy(bordeIzq);      // Destruir las paredes
        if (!bordeDer.activeInHierarchy)    Destroy(bordeDer);     

        if (a != null) Destroy(a);                     // Destruir los tipos de apoyo
        if (b != null) Destroy(b);     
        if (c != null) Destroy(c);    

        for (int i = 0; i < obstaculos.Length; ++i) {
            if (!obstaculos[i].activeSelf)
                Object.Destroy(obstaculos[i]);
        }

        Destroy(this);                                 // Destruir el componente de la vía

    }

}
