using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MallaHex : MonoBehaviour
{

    public Material material;

    public bool esTecho;

    [SerializeField]
    float l;    // Campo que determina la longitud de los triangulos que forman la pieza final

    // Start is called before the first frame update
    void Start() {
        
        Mesh malla = new Mesh();

        int numVertices = 6;
        Vector3[] newVertices = new Vector3[numVertices];
        Vector2[] newUV = new Vector2[numVertices];
        int[] newTriangles = new int [4*3]; // No tiene por qué coincidir la topología con numVertices

        float s3 = Mathf.Sqrt(3.0f);
        float _h = s3*l*0.5f;
        float _2h = s3*l;

        // Vertices
        newVertices[1] = new Vector3(3*l,0,0);
        newVertices[2] = new Vector3(3.5f*l,0,_h);
        newVertices[3] = new Vector3(3*l,0,_2h);
        newVertices[4] = new Vector3( 0,0,_2h);
        newVertices[5] = new Vector3( l*0.5f,0,_h);
        newVertices[0] = new Vector3( 0,0,0 );

        // Textura
        newUV[0] = new Vector2(0.5f,0.0f);
        newUV[1] = new Vector2(0.0f,0.5f);
        newUV[2] = new Vector2(0.0f,1.0f);
        newUV[3] = new Vector2(0.5f,1.0f);
        newUV[4] = new Vector2(1.0f,0.5f);
        newUV[5] = new Vector2(0.5f,0.5f);

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
        MeshCollider mc = GetComponent<MeshCollider>();
        mc.sharedMesh = malla;

        transform.Rotate(180,0,0);
        transform.Translate(-1.5f*l,0,-5);  // Ajuste específico
        // En z es -5 por la separación de la mitad del ancho de la pista
        // En x lo suficiencte para situar la pieza en el centro, y como 
        // la longitud total son 3*l, pues el centro será (3*l)/2 
    }

    // Update is called once per frame
    void Update()
    {
        
    }


}
