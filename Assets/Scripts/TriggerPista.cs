using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerPista : MonoBehaviour {

    static public float segundosCambio = 0.74f; // Segundos
    public NaveMovimiento naveMovimiento = null;    // Se asigna durante la generación procedural
    Vector3 rotacion = Vector3.zero;

    public void setRotacion(Vector3 rot) {
        rotacion = rot;
    }

    public string tagFiltro = "Jugador";
    Vector3 rotOriginal;
    void OnTriggerEnter(Collider other) {
        if (other.tag == tagFiltro && naveMovimiento != null && !lerpRotar ) {
            rotOriginal = naveMovimiento.rotNave;
            lerpRotar = true;
        }
    }

    void OnTriggerExit(Collider other) {
        if (other.tag == tagFiltro && naveMovimiento != null && lerpRotar) {
            lerpRotar = false;
        }
    }


    bool lerpRotar = false;
    float t = 0.0f;
    void FixedUpdate() {
        if (lerpRotar && t < 1.0f) {
            naveMovimiento.rotNave.y = Mathf.LerpAngle(rotOriginal.y, rotacion.y, Mathf.Sin(t)); // Interpolación con un Ease senoidal
            t += Time.fixedDeltaTime / Mathf.Abs(segundosCambio) + 0.01f;;            
        }

        if ( t >= 1.0f ) {
            naveMovimiento.rotNave.y = rotacion.y;
            lerpRotar = false;
            t = 0;
        }
    }

}
