using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerPista : MonoBehaviour
{

    float duracionFixed;

    // Start is called before the first frame update
    void Start()
    {
        
    }


    static public float segundosCambio = 0.74f; // Segundos
    public NaveMovimiento nm = null; 
    Vector3 rotacion = Vector3.zero;

    public void setRotacion(Vector3 rot) {
        rotacion = rot;
    }

    public string tagFiltro = "Jugador";
    Vector3 rotOriginal;
    void OnTriggerEnter(Collider other) {
        if (other.tag == tagFiltro && nm != null && !lerpRotar ) {
            //nm.cambiaCam(rotacion.y);
            rotOriginal = nm.rotNave;
            lerpRotar = true;
        }
    }

    void OnTriggerExit(Collider other) {
        if (other.tag == tagFiltro && nm != null && lerpRotar) {
            lerpRotar = false;
        }
    }

    void Update() {
        duracionFixed = Time.fixedDeltaTime / Mathf.Abs(segundosCambio) + 0.01f;
    }

    bool lerpRotar = false;
    float t = 0.0f;
    void FixedUpdate() {
        if (lerpRotar && t < 1.0f) {

            Debug.Log("e que paza, yo interpolo tio");
            
            nm.rotNave.y = Mathf.LerpAngle(rotOriginal.y, rotacion.y, Mathf.Sin(t)); // Interpolación con un Ease senoidal
            
            t += duracionFixed;            
        
        }

        if ( t >= 1.0f ) {
            nm.rotNave.y = rotacion.y;
            lerpRotar = false;
            t = 0;
        }
    }
}
