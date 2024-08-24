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
    private LensSettings lenteMax,lenteMin;
    private float fovOrtoMax,fovOrtoMin;

    // Start is called before the first frame update
    void Start() {
        rb = GetComponent<Rigidbody>();
        playerInput = GetComponent<PlayerInput>();
        rb.freezeRotation = true;

        lenteMax = lenteMin = (LensSettings)cam0.m_Lens;
        lenteMin.OrthographicSize = lenteMax.OrthographicSize * 0.85f;
    }

    // Update is called once per frame
    void Update() {
        inputMoverse =  playerInput.actions["Moverse"].ReadValue<Vector2>();
        inputRotar =    playerInput.actions["Rotar"].ReadValue<Vector2>();
        rb.maxLinearVelocity = maxVelocidad;
        InterpolarCamara();
    }

    [SerializeField]
    float segsAcelera = 2.5f;
    [SerializeField]
    float segsDecelera = 3.75f;
    [SerializeField]
    float qInputPercentage = 0.8f;


    float lastQInput=0f;
    float pBlend=0f;
    void InterpolarCamara() {   // se ejecuta dentro del frame, es decir, del update
        // Cambios en la cámara
        float qInput = (Mathf.Abs(inputMoverse.x)+Mathf.Abs(inputMoverse.y))*qInputPercentage; 
        // Está algo reducido, porque si no da mucho más de 1
        // En realidad, es la 'velocidad' de la nave

        float pBlendDeceleration = (segsAcelera+0.0001f)/(segsAcelera < segsDecelera ? segsDecelera : segsAcelera);
        // Si acelera (asegurado con Epsilon), lo esperado. Si no tarda un poco más
        // Es decir, o segsAcelera o segsDecelera
        bool isAcelera = lastQInput + Mathf.Epsilon <= qInput;
        pBlend += Time.deltaTime / (isAcelera ? segsAcelera : -segsDecelera);

        pBlend = Mathf.Clamp01(pBlend);
        cam0.m_Lens = LensSettings.Lerp(lenteMin, lenteMax, pBlend); // Interpola entre la configuración de la lente

        // Actualizar variables
        lastQInput = qInput;
    }

    public Vector3 rotNave;

    void FixedUpdate() {    // Puede ejecutarse más de una vez por frame
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
