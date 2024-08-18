using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;

public class NaveMovimiento : MonoBehaviour {

    Rigidbody rb;
    PlayerInput playerInput;
    Vector2 inputMoverse, inputRotar;

    public float cteVelocidad = 5.0f;
    public float maxVelocidad = 15.0f;
    public float fuerzaSalto = 5.0f;

    [SerializeField]
    float velCamara = 0.725f;
    
    public CinemachineVirtualCamera cam0;

    // Start is called before the first frame update
    void Start() {
        rb = GetComponent<Rigidbody>();
        playerInput = GetComponent<PlayerInput>();
        rb.freezeRotation = true;
    }

    // Update is called once per frame
    void Update() {
        inputMoverse =  playerInput.actions["Moverse"].ReadValue<Vector2>();
        inputRotar =    playerInput.actions["Rotar"].ReadValue<Vector2>();
        rb.maxLinearVelocity = maxVelocidad;
    }

    public Vector3 rotNave;

    void FixedUpdate() {
        Vector3 direccion = new Vector3(inputMoverse.y, 0.0f, -inputMoverse.x);     // Coord. y del inputMoverse (stick arriba) es avanzar en la X del juego. Idem con eje Z en el juego
        
        rb.AddRelativeForce( direccion * cteVelocidad );

        this.transform.localEulerAngles = rotNave;
    }

/*
    public void cambiaCam(float grados) {
        int aux = (int) grados;
        goCam0.SetActive(aux == 0);
        goCam60.SetActive(aux == 60);
        goCam_60.SetActive(aux == -60);

        if (goCam0.activeSelf) camPath = getCam(cam0);
        if (goCam60.activeSelf) camPath = getCam(cam60);
        if (goCam_60.activeSelf) camPath = getCam(cam_60);
    }
    */

    public void Saltar() {
        if (this.transform.position.y <= 10.0f)
            rb.AddForce(Vector3.up * fuerzaSalto);
    }

}
