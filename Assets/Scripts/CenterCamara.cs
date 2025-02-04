using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FocusCamara : MonoBehaviour
{

    public bool focusCamara;
    public Transform punto;

    // Start is called before the first frame update
    void Start() {
        focusCamara = true;
    }

    public void switchCenterCamera(){
        focusCamara = !focusCamara;
    } 

    // Update is called once per frame
    void Update() {
        if ( focusCamara && punto != null ) {
            Vector3 nuevoVector = punto.position - transform.position;
            transform.rotation = Quaternion.LookRotation( nuevoVector );
        }
    }
}
