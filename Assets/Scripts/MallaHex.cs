using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MallaHex : MonoBehaviour {

    public Material material;
    public bool esTecho;
    public float l = -1;    // Campo que determina la longitud de los triangulos que forman la pieza final
    float h;
    public float _2h = -1;

    float sqrt3 = Mathf.Sqrt(3.0f);

    int numVertices = 6, numTriangulos = 4;
    Vector3[] newVertices;
    int[] newTriangles;

    // Start is called before the first frame update
    void Start() {

        // Cálculos
        if (_2h > 0)          l = _2h / sqrt3;
        else                _2h = sqrt3*l;
        h = _2h * 0.5f;
        
        Mesh malla = new Mesh();

        newVertices = new Vector3[numVertices];
        Vector2[] newUV = new Vector2[numVertices];
        newTriangles = new int [numTriangulos*3]; // No tiene por qué coincidir la topología con numVertices

        // Vertices
        newVertices[1] = new Vector3(3*l,0,0);
        newVertices[2] = new Vector3(3.5f*l,0,h);
        newVertices[3] = new Vector3(3*l,0,_2h);
        newVertices[4] = new Vector3( 0,0,_2h);
        newVertices[5] = new Vector3( l*0.5f,0,h);
        newVertices[0] = new Vector3( 0,0,0 );

        // Textura
        float aux1 = 3.0f/3.5f, aux2 = 0.5f/3.5f;

        newUV[1] = new Vector2(aux1, 0.0f);
        newUV[2] = new Vector2(1.0f, 0.5f);
        newUV[3] = new Vector2(aux1, 1.0f);
        newUV[4] = new Vector2(0.0f, 1.0f);
        newUV[5] = new Vector2(aux2, 0.5f);
        newUV[0] = new Vector2(0.0f, 0.0f);

        // Topología. Confg 1
        newTriangles[0] = 0;
        newTriangles[1] = 1;
        newTriangles[2] = 5;

        newTriangles[3] = 1;
        newTriangles[4] = 2;
        newTriangles[5] = 5;

        newTriangles[6] = 2;
        newTriangles[7] = 3;
        newTriangles[8] = 5;

        newTriangles[9] = 3;
        newTriangles[10] = 4;
        newTriangles[11] = 5;

        malla.vertices = newVertices;
        malla.uv = newUV;
        malla.triangles = newTriangles;

        GetComponent<MeshFilter>().mesh = malla;
        MeshRenderer mr = GetComponent<MeshRenderer>();
        mr.material = material;
        mr.enabled = !esTecho;
        // MeshCollider mc = GetComponent<MeshCollider>(); <-- No hace falta, se ha sustituido por un plano que sigue al objetivo todo el rato
        // mc.sharedMesh = malla;

        transform.Rotate(180,0,0);
        transform.Translate(-1.5f*l,0,-5);  // Ajuste específico
        // En z es -5 por la separación de la mitad del ancho de la pista -> (-h)
        // En x lo suficiencte para situar la pieza en el centro, y como 
        // la longitud total son 3*l, pues el centro será (3*l)/2 
    }

    public bool estaDentro(Vector3 punto) {
        bool aux = false;
        for (int i = 0; i < numTriangulos; i++) {
            aux |= dentroTri(punto, newVertices[newTriangles[0 + (i*3)]], newVertices[newTriangles[1 + (i*3)]], newVertices[newTriangles[2 + (i*3)]]);
        }
        return aux;
    }

    // http://totologic.blogspot.com/2014/01/accurate-point-in-triangle-test.html
    float dotDentro(Vector3 uno, Vector3 dos, Vector3 d) {
        return (dos.z - uno.z)*(d.x - uno.x) + (-dos.x + uno.x)*(d.z - uno.z);
    }

    public bool dentroTri(Vector3 P, Vector3 A, Vector3 B, Vector3 C) {
        // Un punto N dentro de un triángulo de puntos (P1, P2, P3), cada punto con x,y,z
        bool check = dotDentro(A,B,P) >= 0;
        check &= dotDentro(B,C,P) >= 0;
        check &= dotDentro(C,A,P) >= 0;
        return check;
    }



    // Update is called once per frame
    void Update()
    {
        
    }


}
