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

    public float baseRadius;
    public float minGenRadius,maxGenRadius;
    public float minGenPosA, maxGenPosA;
    public float minGenPosB, maxGenPosB;

    void Start() {
        circulos = new float [3, numCirculosPerlin];
        Random.InitState(semilla);
        EcuacionesCirculos();
        GenerarPerlin();
    }

    void EcuacionesCirculos(){
        for (int i = 0; i < numCirculosPerlin; i++) {                       // Cada columna representa una función
            circulos[0,i] = Random.Range(minGenPosA, maxGenPosA);           //a <-> x
            circulos[1,i] = Random.Range(minGenPosB, maxGenPosB);           //b <-> z
            circulos[2,i] = Random.Range(minGenRadius, maxGenRadius);       //r
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

    Vector3 elipseXZ(float gamma, float posA, float posB, float radio) {
        gamma *= Mathf.Deg2Rad;
        float x_ =  Mathf.Cos(gamma) * radio; // PARA QUE SEA ELIPSE TENDRÍA QUE MULTIPLICARSE POR OTRO VALOR (SEMIEJE)
        float z_ = -Mathf.Sin(gamma) * radio;
        return new Vector3(x_ + posA, 0.0f, z_ + posB); // CORRECTO!
    }

    // https://www.lanshor.com/ruido-perlin/

    void GenerarPerlin(){

        float incremento = 360.0f / (float) divisiones; 

        float anguloSecciones = 360.0f / numCirculosPerlin;

        posicionesFinales = new Vector3[divisiones];

        float alfa = 0.0f; 
        for (int i = 0; i < divisiones; i++) {
            posicionesFinales[i] = elipseXZ(alfa, uno, uno, baseRadius);
            alfa += incremento;
        }

        for (int i = 0; i < numCirculosPerlin; i++) {
            float cPow =  Mathf.Pow(2,(float)i);
            alfa = 0.0f;
            for (int j = 0; j < divisiones; j++) {
                //if ((float)i * anguloSecciones < alfa && alfa <= (float)(i+1) * anguloSecciones)
                posicionesFinales[j] += elipseXZ(alfa, circulos[0,i], circulos[1,i], circulos[2,i]);
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
            Vector3 last = elipseXZ(0, circulos[0,i], circulos[1,i], circulos[2,i]);
            Vector3 now;
            for (int k = 1; k < divisiones; k++) {
                //GameObject go = Instantiate(prefab, this.transform);
                //go.transform.Translate(posicionesFinales[k].x, 0.0f, posicionesFinales[k].z);
                float alpha = (float)k * (360.0f/(float)divisiones);
                now = elipseXZ(alpha, circulos[0,i], circulos[1,i], circulos[2,i]);
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
