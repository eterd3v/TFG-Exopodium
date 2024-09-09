using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

public class tileAnim : MonoBehaviour {

    private Material mat = null;

    [SerializeField]
    Texture[] arrayTexturas;
    [SerializeField]
    float segundosAnimacion=7f;
    [SerializeField]
    float velocity = 0.85f;

    void Start() {
        MeshRenderer mesh = this.GetComponent<MeshRenderer>();
        if (mesh != null){
            mat = mesh.material;
            if (mat != null){
                mat.mainTextureOffset = Vector2.zero;
                if (arrayTexturas.Length > 0){
                    Random rand = new Random();
                    mat.mainTexture = arrayTexturas[rand.Next()%arrayTexturas.Length];
                }
            }
        }
        arrayTexturas = null;
    }

    void Update() {
        if (mat != null) {
            Vector2 nuevo = mat.mainTextureOffset;
            float deltaTiempo = Time.deltaTime / segundosAnimacion;
            nuevo.x += deltaTiempo;
            nuevo.y += deltaTiempo;
            mat.mainTextureOffset = nuevo * velocity; 
        }
    }
}
