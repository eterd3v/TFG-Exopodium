using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

 
public class GenerarViasPerlin : MonoBehaviour
{

    static public float uno = 1.0f, dos = 2.0f;


    // (x-a)^2 + (y-b)^2 = r^2 <- r = {1.0, 2.837, 3.5, .... }

    private float[,] circulos; // Matriz de 3 x n. Fila 1: a, Fila 2: b, Fila 3: r
    public int semilla;

    public int numCirculosPerlin = 5;
    public int divisiones = 120;

    public float minGenRadius,maxGenRadius;
    public float minSemiejeA, maxSemiejeA;
    public float minSemiejeB, maxSemiejeB;
    public float minAmplitud, maxAmplitud;

    void Start() {
        circulos = new float [4, numCirculosPerlin];
        Random.InitState(semilla);
        EcuacionesCirculos();
        GenerarPerlin();
    }

    void EcuacionesCirculos(){
        for (int i = 0; i < numCirculosPerlin; i++) {                       // Cada columna representa una función
            circulos[0,i] = Random.Range(minSemiejeA, maxSemiejeA);           //a <-> x
            circulos[1,i] = Random.Range(minSemiejeB, maxSemiejeB);           //b <-> z
            circulos[2,i] = Random.Range(minGenRadius, maxGenRadius);       //r
            circulos[3,i] = Random.Range(minAmplitud, maxAmplitud);       //amplitud perlin
        }

        float anguloSecciones = 360.0f / numCirculosPerlin;
        float alfa = 0.0f;
        for (int i = 0; i < numCirculosPerlin; i++) {                   // Arreglo para los cuadrantes

            //if (alfa <= 90.0f) No hay que cambiar nada, ya están en positivo los valores
            
            if (alfa > 270.0f){
                circulos[0,i] = -circulos[0,i];
            }else if (alfa > 90.0f && alfa <= 180.0f){
                circulos[1,i] = -circulos[1,i];
            }else if (alfa > 180.0f && alfa <= 270.0f) {
                circulos[0,i] = -circulos[0,i];
                circulos[1,i] = -circulos[1,i];
            }

            alfa += anguloSecciones;
        }
    }

    private Vector3[] posicionesFinales;

    Vector3 elipseXZ(float alfa, float semiEjeA, float semiEjeB, float radio, float gamma, float amplitud) {
        alfa *= Mathf.Deg2Rad;
        float ruido = Mathf.Sin(gamma) * amplitud;
        float x_ =  Mathf.Cos(alfa) * semiEjeA * (radio + ruido);
        float z_ = -Mathf.Sin(alfa) * semiEjeB * (radio + ruido);
        return new Vector3(x_, 0.0f, z_); // CORRECTO!
    }

    // https://www.lanshor.com/ruido-perlin/

    void GenerarPerlin(){

        float incremento = 360.0f / (float) divisiones; 

        float anguloSecciones = 360.0f / numCirculosPerlin;

        posicionesFinales = new Vector3[divisiones];

        float alfa = 0.0f; 
        for (int i = 0; i < divisiones; i++) {
            //posicionesFinales[i] = elipseXZ(alfa, uno, uno, baseRadius, 0.0f, 0.0f);
            alfa += incremento;
        }

        for (int i = 0; i < numCirculosPerlin; i++) {
            float cPow =  Mathf.Pow(2,(float)(i));
            alfa = 0.0f;
            for (int j = 0; j < divisiones; j++) {
                posicionesFinales[j] += elipseXZ(alfa, circulos[0,i], circulos[1,i], circulos[2,i], alfa , circulos[3,i] * cPow);
                alfa += incremento;
            }

            //Debug.Log(alfa + ">" + elipseXZ(alfa, uno, uno, baseRadius) + " :: " + posicionesFinales[0]);
        }

        for (int k = 1; k < divisiones; k++)
        {
            //GameObject go = Instantiate(prefab, this.transform);
            //go.transform.Translate(posicionesFinales[k].x, 0.0f, posicionesFinales[k].z);
            Debug.DrawRay(posicionesFinales[k-1], posicionesFinales[k] - posicionesFinales[k-1], Color.red, 600); // Ver el trazo de la curva en el editor. Expira en 60 segs.
        }

        DibujarCirculosPerlin();
    }

    void DibujarCirculosPerlin(){

        Color col = new Color (0.2f,1.0f,0.0f);

        for (int i = 0; i < numCirculosPerlin; i++)
        {
            Vector3 last = elipseXZ(0, circulos[0,i], circulos[1,i], circulos[2,i], 0.0f, circulos[3,i]);
            Vector3 now;

            float cPow =  Mathf.Pow(2,(float)i);
            
            for (int k = 1; k < divisiones; k++) {
                //GameObject go = Instantiate(prefab, this.transform);
                //go.transform.Translate(posicionesFinales[k].x, 0.0f, posicionesFinales[k].z);
                float alpha = (float)k * (360.0f/(float)divisiones);
                now = elipseXZ(alpha, circulos[0,i], circulos[1,i], circulos[2,i], alpha*cPow , circulos[3,i] / cPow);
                Debug.DrawRay(last, now - last, col, 600); // Ver el trazo de la curva en el editor. Expira en 60 segs.
                last = now;
            }
            float colGB = (float) (i+1) / (float) numCirculosPerlin;
            col.g -= colGB;
            col.b += colGB;
        }

    }
   
    public GameObject prefab;


}
