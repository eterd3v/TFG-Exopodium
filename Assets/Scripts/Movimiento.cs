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
    float velCambioCam = 0.4f;
    
    public CinemachineVirtualCamera cam;
    CinemachineTrackedDolly camPath;

    [SerializeField]
    GenerarCircuitoHex gch;
    
    // Start is called before the first frame update
    void Start() {
        rb = GetComponent<Rigidbody>();
        playerInput = GetComponent<PlayerInput>();
        rb.freezeRotation = true;
        camPath = (CinemachineTrackedDolly) cam.GetCinemachineComponent(CinemachineCore.Stage.Body); // Hereda del componente;
    }
    
    // Update is called once per frame
    void Update() {
        inputMoverse =  playerInput.actions["Moverse"].ReadValue<Vector2>();
        inputRotar =    playerInput.actions["Rotar"].ReadValue<Vector2>();
        rb.maxLinearVelocity = maxVelocidad;
    }

    void FixedUpdate() {
        Vector3 direccion = new Vector3(inputMoverse.y, 0.0f, -inputMoverse.x);     // Coord. y del inputMoverse (stick arriba) es avanzar en la X del juego. Idem con eje Z en el juego
        
        camPath.m_PathPosition += Time.deltaTime * -inputMoverse.x * velCambioCam;
        camPath.m_PathPosition = Mathf.Clamp01(camPath.m_PathPosition);             // Entre 0 y el número máximo de posiciones del raíl de la cámara
        
        rb.AddRelativeForce( direccion * cteVelocidad );

        Vector3 rotOrig = this.transform.localEulerAngles;
        Vector3 rotPista = rotacionPistaActual();
        if (rotOrig.y != rotPista.y) {
            rotOrig.y += 1.0f;
            this.transform.localEulerAngles = rotOrig;
        }
    }

    public void Saltar() {
        if (this.transform.position.y <= 10.0f)
            rb.AddForce(Vector3.up * fuerzaSalto);
    }

    Vector3 rotacionPistaActual() {                 // PENDIENTE
        return this.transform.localEulerAngles;
    } 

}
