using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
 
public class Perlin : MonoBehaviour {

    public int semilla;
    public int divisiones = 120;

    void Start() {
        Random.InitState(semilla);
        //float[] aux = GenerarPerlin(0.0f,0.0f,divisiones);

        Vector3[] aux = GenerarPosiciones(10.0f,0.0f,divisiones);
        Vector3 now, last = aux[0];
        for (int i = 1; i < divisiones; i++) {
            now = aux[i];
            Debug.DrawRay(last,now-last, Color.red, 600);
            last = now;
        }
    }

    Vector3[] GenerarPosiciones(float amplitud, float frecuencia, int longitud){
        Vector3[] posiciones = new Vector3 [longitud];

        float offSetRand = Random.value;

        float[] z_ = new float[longitud];
        int i;
        for (i = 0; i < longitud; i++) { // Seno
            z_[i] = Mathf.Abs(Mathf.PerlinNoise1D( offSetRand + (float)i));
        }

        float[] x_ = new float[longitud];
        int start = Mathf.FloorToInt(((float)longitud) * 0.25f);
        i = (start + 1) % longitud;
        while (start != i) { // coseno
            x_[i] = z_[i];
            i = ++i % longitud;
        }


        float alfa = Mathf.Deg2Rad * (360.0f/(float)longitud);
        for (i = 0; i < longitud; i++) {
            posiciones[i] = new Vector3(x_[i], 0.0f, z_[i]);
            posiciones[i] *= amplitud;
        }

        return posiciones;
    }

}
