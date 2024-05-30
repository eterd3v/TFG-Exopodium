using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class NaveMovimiento : MonoBehaviour {

    // public float speed;
    // public float acceleration;
    // public float maxSpeed;

    Rigidbody rb;
    PlayerInput playerInput;
    Vector2 inputMoverse, inputRotar;

    [SerializeField]
    Vector3 v3Camara;

    [SerializeField]
    float fuerza = 5.0f;
    
    // Start is called before the first frame update
    void Start() {
        rb = GetComponent<Rigidbody>();
        playerInput = GetComponent<PlayerInput>();
    }
    
    // Update is called once per frame
    void Update() {
        inputMoverse =  playerInput.actions["Moverse"].ReadValue<Vector2>();
        inputRotar =    playerInput.actions["Rotar"].ReadValue<Vector2>();
    }

    void FixedUpdate(){
        Vector3 direccion = new Vector3(inputMoverse.y, 0.0f, -inputMoverse.x); // Coord. y del inputMoverse (stick arriba) es avanzar en la X del juego. Idem con eje Z en el juego
        direccion.x *= fuerza;
        direccion.z *= fuerza;
        rb.AddRelativeForce( direccion );

        Vector3 rotacion = new Vector3(inputRotar.x, 0.0f, inputRotar.y);
        //rb.AddRelativeTorque( rotacion * fuerza );

    }

    public void Saltar(){
        rb.AddForce(Vector3.up * fuerza);
    }
}
