using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InicioPartida : MonoBehaviour
{

    public GameObject jugadorConCamara;

    public GameObject primeraVia;

    // Start is called before the first frame update
    void Start()
    {
        jugadorConCamara.transform.position = primeraVia.transform.position;
        jugadorConCamara.transform.Translate(0,1,0);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
