using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movimiento : MonoBehaviour {

    // public float speed;
    // public float acceleration;
    // public float maxSpeed;

    public Camera izq;
    public Camera der;
    
    // Start is called before the first frame update
    void Start() {
    }
    
    // Update is called once per frame
    void Update() {
        // float distance;
        // distance = speed * Time.deltaTime;
        // distance += 0.5f * acceleration * Time.deltaTime * Time.deltaTime;
        // // distance = distance > maxSpeed ? maxSpeed : distance;
        // float movementInZ = Input.GetAxis("Zmovement");
        // float movementInX = Input.GetAxis("Xmovement");
        // float movementInY = Input.GetAxis("Ymovement");
        // transform.Translate(Vector3.right * movementInZ * distance);
        // transform.Translate(Vector3.back * movementInX * distance);
        // transform.Translate(Vector3.up * movementInY * distance);
        transform.Translate(Vector3.right * (Input.GetKey(KeyCode.W) ? 1.0f : 0.0f));
        transform.Translate(Vector3.left * (Input.GetKey(KeyCode.S) ? 1.0f : 0.0f));
        transform.Translate(Vector3.forward * (Input.GetKeyDown(KeyCode.A) ? 1.0f : 0.0f));
        transform.Translate(Vector3.back * (Input.GetKeyDown(KeyCode.D) ? 1.0f : 0.0f));
        transform.Translate(Vector3.up * (Input.GetKeyDown(KeyCode.Space) ? 1.0f : 0.0f));
        transform.Translate(Vector3.down * (Input.GetKeyDown(KeyCode.LeftShift) ? 1.0f : 0.0f));

        // if ( transform.position.z < 0 ) {
        //         izq.enabled = true;
        //         der.enabled = false;
        // } else {
        //         izq.enabled = false;
        //         der.enabled = true;
        // }
    }
}
