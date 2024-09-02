using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine; // Versión 2.9.7 !

public class NaveMovimiento : MonoBehaviour {

    Rigidbody rb;
    PlayerInput playerInput;
    Vector2 inputMoverse, inputRotar;

    public float cteVelocidad = 5.0f;
    public float maxVelocidad = 15.0f;
    public float fuerzaSalto = 5.0f;

    public CinemachineVirtualCamera cam0;
    private CinemachineOrbitalTransposer tipoCam0;
    private LensSettings lenteMax,lenteMin, lenteActual;
    private float fovOrtoMax,fovOrtoMin;

    InputAction ioMoverse, ioRotar, ioElevar, ioDescender;

    [SerializeField]
    Transform fondo = null;

    public Material parallax = null;

    [SerializeField]
    float parallaxVelocity = 0.2f;

    [SerializeField]
    Transform parallaxIzq = null, parallaxDer = null, parallaxAuxIzq = null, parallaxAuxDer = null;

    [SerializeField]
    Vector2 rotacionesParallax = new Vector2(15f,-15f);
    
    [SerializeField]
    Vector2 escaladosParallax = new Vector2(1.125f,1.125f);


    Vector3 puntoOrigen;

    void Start() {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;

        playerInput = GetComponent<PlayerInput>();
        ioMoverse = playerInput.actions["Moverse"];
        ioRotar = playerInput.actions["Rotar"];
        ioElevar = playerInput.actions["Elevar"];
        ioDescender = playerInput.actions["Descender"];


        tipoCam0 = (CinemachineOrbitalTransposer) cam0.GetCinemachineComponent(CinemachineCore.Stage.Body);
        lenteMax = lenteMin = (LensSettings)cam0.m_Lens;
        lenteMin.OrthographicSize = lenteMax.OrthographicSize * percentageLensLerp;

        if (parallax != null) {
            puntoOrigen = this.transform.position;
            parallax.mainTextureOffset = Vector2.zero;
        }

        if (vehiculo != null) {
            angulosVehiculoInicial = vehiculo.localEulerAngles;
        }

    }

    // Update is called once per frame
    void Update() {
        inputMoverse =  ioMoverse.ReadValue<Vector2>();
        inputRotar =    ioRotar.ReadValue<Vector2>();
        rb.maxLinearVelocity = maxVelocidad;
        InterpolarCamara();
        if (fondo != null) {
            Vector3 escalado = Vector3.one * 2f * lenteActual.OrthographicSize;
            escalado.x *= lenteActual.Aspect;
            fondo.localScale = escalado;
        }
        if (parallax != null) {
            Vector3 distancias = this.transform.position - puntoOrigen;
            Vector3 pesos = new Vector3(0.65f,0f,0.35f) * parallaxVelocity;
            distancias.x *= pesos.x;
            distancias.z *= pesos.z;
            parallax.mainTextureOffset = new Vector2(puntoOrigen.x + distancias.x + distancias.z, 0f);
        }
    }

    [SerializeField]
    float percentageLensLerp = 0.85f;
    [SerializeField]
    float segsAcelera = 2.5f;
    [SerializeField]
    float segsDecelera = 3.75f;
    [SerializeField]
    float qInputPercentage = 0.8f;

    [SerializeField]
    ParticleSystem particulas = null;

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
        lenteActual = LensSettings.Lerp(lenteMin, lenteMax, pBlend); // Interpola entre la configuración de la lente
        cam0.m_Lens = lenteActual;

        if (particulas != null) {
            if (pBlend > 0.66f && !particulas.isPlaying)
                particulas.Play();
            else if (pBlend < 0.66f && particulas.isPlaying){
                particulas.Stop();
            }
        }

        if (parallaxIzq != null && parallaxDer != null && parallaxAuxIzq != null & parallaxAuxDer != null){
            parallaxIzq.localEulerAngles = new Vector3(pBlend * rotacionesParallax.x,0f,0f);
            parallaxDer.localEulerAngles = new Vector3(pBlend * rotacionesParallax.y,0f,0f);
            parallaxAuxIzq.localScale = new Vector3(1f,Mathf.Lerp(1f,escaladosParallax.x,pBlend),1f);
            parallaxAuxDer.localScale = new Vector3(1f,Mathf.Lerp(1f,escaladosParallax.y,pBlend),1f);
        }

        // Actualizar variables
        lastQInput = qInput;
    }

    public Vector3 rotNave;
    [SerializeField]
    float umbralMovimientoVehiculo=0.3333f;
    [SerializeField]
    float segundosLerpMovimientoVehiculo=0.75f;
    [SerializeField]
    Transform vehiculo = null;
    [SerializeField]
    Vector3 minAngulosVehiculo = Vector3.zero, maxAngulosVehiculo = Vector3.one;

    Vector3 lerpAngulos = Vector3.zero;
    Vector3 angulosVehiculoInicial;
    static float grados = Mathf.PI * 0.5f;
    void FixedUpdate() {    // Puede ejecutarse más de una vez por frame
        
        float vertical = ioElevar.ReadValue<float>() - ioDescender.ReadValue<float>();
        Vector3 direccion = new Vector3(inputMoverse.y, vertical, -inputMoverse.x);     // Coord. y del inputMoverse (stick arriba) es avanzar en la X del juego. Idem con eje Z en el juego
        rb.AddRelativeForce( direccion * cteVelocidad );
        this.transform.localEulerAngles = rotNave;

        if (vehiculo != null){

            float fixedSegundos = Time.fixedDeltaTime/segundosLerpMovimientoVehiculo;

            if (inputMoverse.x < -umbralMovimientoVehiculo){
                lerpAngulos.x += fixedSegundos;
            }else if (inputMoverse.x > umbralMovimientoVehiculo) {
                lerpAngulos.x -= fixedSegundos;
            }else if (!Mathf.Approximately(lerpAngulos.x,0.5f)) {
                lerpAngulos.x += lerpAngulos.x > 0.5f ? -fixedSegundos : fixedSegundos;
            }

            //if (inputMoverse.y < -umbralMovimientoVehiculo){
            //}else if (inputMoverse.y > umbralMovimientoVehiculo) {
            //}

            lerpAngulos.y = 0.5f;// + rotNave.y AVERIGUAR MAÑANA!!!

            if (vertical > umbralMovimientoVehiculo){
                lerpAngulos.z += fixedSegundos*1.25f;
            }else if (vertical < -umbralMovimientoVehiculo){
                lerpAngulos.z -= fixedSegundos*1.25f;
            }else if (!Mathf.Approximately(lerpAngulos.z,0.5f)) {
                lerpAngulos.z += lerpAngulos.z > 0.5f ? -fixedSegundos : fixedSegundos;
            }
            
            lerpAngulos.x = Mathf.Clamp01(lerpAngulos.x);
            lerpAngulos.y = Mathf.Clamp01(lerpAngulos.y);
            lerpAngulos.z = Mathf.Clamp01(lerpAngulos.z);

            Vector3 rotacion = Vector3.zero;

            rotacion.x = Mathf.Lerp(minAngulosVehiculo.x,maxAngulosVehiculo.x,lerpAngulos.x);  // Aquí siempre se recorta el valor entre 0 y 1 !
            rotacion.y = Mathf.Lerp(minAngulosVehiculo.y,maxAngulosVehiculo.y,lerpAngulos.y);
            rotacion.z = Mathf.Lerp(minAngulosVehiculo.z,maxAngulosVehiculo.z,lerpAngulos.z);

            vehiculo.localEulerAngles = rotacion + angulosVehiculoInicial;
        }

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

    public void Saltarr() {
        if (this.transform.position.y <= 10.0f)
            rb.AddForce(Vector3.up * fuerzaSalto);
    }

}
