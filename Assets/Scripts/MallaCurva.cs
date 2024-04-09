using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MallaCurva : MonoBehaviour
{

    public Material material;

    public bool esTecho;

    // Start is called before the first frame update
    void Start()
    {
        Mesh malla = new Mesh();

        int numVertices = 6;
        Vector3[] newVertices = new Vector3[numVertices];
        Vector2[] newUV = new Vector2[numVertices];
        int[] newTriangles = new int [4*3]; // No tiene por qué coincidir la topología con numVertices

        // Vertices
        newVertices[0] = new Vector3(10,0,0);
        newVertices[1] = new Vector3(20,0,10);
        newVertices[2] = new Vector3(20,0,20);
        newVertices[3] = new Vector3(10,0,20);
        newVertices[4] = new Vector3( 0,0,10);
        newVertices[5] = new Vector3(10,0,10);

        // Textura
        newUV[0] = new Vector2(0.5f,0.0f);
        newUV[1] = new Vector2(1.0f,0.5f);
        newUV[2] = new Vector2(1.0f,1.0f);
        newUV[3] = new Vector2(0.5f,1.0f);
        newUV[4] = new Vector2(0.0f,0.5f);
        newUV[5] = new Vector2(0.5f,0.5f);


        // Topología
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
        GetComponent<MeshRenderer>().material = material;

        GetComponent<MeshRenderer>().enabled = !esTecho;

        transform.Rotate(180,0,0);
        transform.Translate(-10,0,-10);
    }

    // Update is called once per frame
    void Update()
    {
        
    }


}
