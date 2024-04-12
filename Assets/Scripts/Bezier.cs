using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bezier : MonoBehaviour {
/*
    // PUNTOS DE CONTROL
    public GameObject   p1, p2, p3, p4;
    private Vector3     v1, v2, v3, v4;

    //  CONTROL DEL TIEMPO (Ya no se usa deltaT aquí. Se usa un atributo de esta clase)
    public ControlScript controlTiempo;

    // OBJETO/PUNTO ACTUAL DE LA CURVA
    public GameObject punto;
    private BezierPoint bp;

    //  CONTROL DE LA VELOCIDAD
    private float[,] desplazamientos;
    public int numeroArcos;   // mayor que 1
    private int lastNumeroArcos;
    private float longitudTotal;
    public int tipoDeEase;

    private LineRenderer lineRender;

    // Start is called before the first frame update
    void Start() {


        // Parámetros adicionales
        bp = punto.GetComponent<BezierPoint>(); 
        lineRender = GetComponent<LineRenderer>();
        if (lineRender != null){
            float alpha = 1.0f;
            Gradient gradient = new Gradient();
            gradient.SetKeys(
                new GradientColorKey[] { new GradientColorKey(Color.blue, 0.0f), new GradientColorKey(Color.green, 0.5f), new GradientColorKey(Color.red, 1.0f) },
                new GradientAlphaKey[] { new GradientAlphaKey(alpha, 0.0f), new GradientAlphaKey(alpha, 0.5f), new GradientAlphaKey(alpha, 1.0f) }
            );
            lineRender.colorGradient = gradient;
            lineRender.startWidth = 0.1f;
            //lineRender.endWidth = 0.3f;
        }


        asignarPosiciones();
        lastNumeroArcos = numeroArcos;
        generarTablaDesplazamientos();
    }

    void Update() {

        if (posicionesCambiadas()) {
            asignarPosiciones();
            generarTablaDesplazamientos();
        } else if (numeroArcos != lastNumeroArcos) {
            if (numeroArcos < 2)
                numeroArcos = 2;
            lastNumeroArcos = numeroArcos;
            generarTablaDesplazamientos();
        }

        if (controlTiempo.play && punto != null) {
            punto.transform.position = aproximarConTabla(controlTiempo.t);   // p = P(U(S(t)))
            if (bp != null) {
                bp.NuevaLuz(controlTiempo.t);
                bp.AplicarMaterial();
            }
        }
        
    }

    void asignarPosiciones(){
        v1 = p1.transform.position;
        v2 = p2.transform.position;
        v3 = p3.transform.position;
        v4 = p4.transform.position;
    }

    bool posicionesCambiadas(){
        return 
            v1 != p1.transform.position | 
            v2 != p2.transform.position | 
            v3 != p3.transform.position | 
            v4 != p4.transform.position;
    }

    void generarTablaDesplazamientos() {

        desplazamientos = new float[2, numeroArcos -1];     // 2 filas (longitud_i, u_i) y tantas divisiones como columnas.  
                                                            // (num. Arcos - 1 = num. Divisiones)

        float deltaU = 1/((float)numeroArcos);              // delta de U, para los incrementos (entre 0 y 1)
        float u = deltaU;                                   // incremento acumulado, u
        Vector3 lastP = v1;                                 // Punto anterior   para hallar la longitud del arco
        Vector3 nowP;                                       // Punto actual     para hallar la longitud del arco

        bool hayLineRenderer = lineRender != null; 
        if (hayLineRenderer){
            lineRender.positionCount = numeroArcos;
            lineRender.SetPosition(0,v1);
        }

        for (int i = 0; i < numeroArcos -1; ++i) {          // 100 arcos -> 99 cortes, del 0 al 98

            nowP = bezier(u);

            if (hayLineRenderer){
                lineRender.SetPosition(1+i,nowP);
                //Debug.DrawRay(lastP, nowP - lastP, Color.red, 40); // Ver el trazo de la curva en el editor. Expira en 60 segs.
            }

            desplazamientos[0,i]  = Vector3.Distance(nowP, lastP);
            desplazamientos[0,i] += i != 0 ? desplazamientos[0, i-1] : 0.0f; // suma acumulada de longitudes
            desplazamientos[1,i]  = u;

            u = (i+1) * deltaU;     // u se considera para el siguiente punto de corte
            lastP = nowP;           // se prepara el ultimo punto para el siguiente arco
            
        }

        longitudTotal = desplazamientos[0, numeroArcos-2]; // punto 99 al punto 100, es el arco 98
        //Debug.Log("Suma de longitudes " + longitudTotal);

    }


    Vector3 aproximarConTabla(float t) {

        if ( t <= 0.0f || t >= 1.0f ) {
            return t <= 0.0f ? v1 : v4; 
        }

        float LENGHT = ease(t) * longitudTotal;

        float min = longitudTotal;          // Buscamos al que más se parezca a LENGHT
        float dif = 0;
        float lastDif = longitudTotal + 1;  // Para cortar cuanto antes el bucle

        int idx = 0;                        // Índice

        for (int i = 0; i < numeroArcos-1 && lastDif >= dif ; ++i) {

            dif = LENGHT - desplazamientos[0,i];
            dif = dif < 0 ? -dif : dif;  // Valor absoluto

            if ( dif < min ) {
                min = dif;
                idx = i;
            }

            lastDif = dif;
            ++i;
        }

        return bezier(desplazamientos[1,idx]);      // Se devuelve el u_i de la tabla (entre 0 y 1)
    }

    // t es un flotante entre 0 y 1
    Vector3 bezier(float t){

        float uMt = 1.0f - t;   // uno Menos t
        float aux = 3 * t * uMt;

        Vector3 r1 =  uMt*uMt*uMt*v1;
        Vector3 r2 =  aux*uMt    *v2;
        Vector3 r3 =  aux*t      *v3;
        Vector3 r4 =  t*t*t      *v4;
        
        return r1+r2+r3+r4;    // punto final
    }

    float ease(float s){
        switch (tipoDeEase) {
            case 0: 
                return (Mathf.Sin(Mathf.PI * (s - 0.5f)) + 1.0f) * 0.5f; // Ángulo es lo mismo que: PI*s - (PI/2)

            case 1: // https://gizma.com/easing/#easeInOutCubic
                return s < 0.5f ? 4.0f * s * s * s : 1.0f - Mathf.Pow(-2.0f * s + 2.0f, 3.0f) / 2.0f;

            case 2: // https://gizma.com/easing/#easeOutBounce
                float n1 = 7.5625f;
                float d1 = 2.75f;
                if (s < 1.0f / d1) {
                    return n1 * s * s;
                } else if (s < 2.0f / d1) {
                    return n1 * (s -= 1.5f / d1) * s + 0.75f;
                } else if (s < 2.5f / d1) {
                    return n1 * (s -= 2.25f / d1) * s + 0.9375f;
                } else {
                    return n1 * (s -= 2.625f / d1) * s + 0.984375f;
                }

            case 3: // https://gizma.com/easing/#easeInQuint
                return s * s * s * s * s; 

            default:
                return s;
        }
    }

    public void setTipoEase(int tipo){
        tipoDeEase = tipo;
    }

*/
}
