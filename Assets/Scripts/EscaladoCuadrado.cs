using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EscaladoCuadrado : MonoBehaviour
{
    
    [SerializeField]
    bool porMaterial = false;

    [SerializeField]
    float pixelsPorUnidad = 100f;
    
    // Start is called before the first frame update
    void Start()
    {
        if (porMaterial){
            Material myMaterial = this.GetComponent<MeshRenderer>().material;
            Texture textura = myMaterial.mainTexture;
            if (textura != null){
                Vector3 escalado = Vector3.one * pixelsPorUnidad;
                escalado.x /= textura.width;
                escalado.y /= textura.height;
                this.transform.localScale = escalado;
            } 
        }else{
            Sprite mySprite = this.GetComponent<SpriteRenderer>().sprite;
            Texture2D textura = mySprite.texture;
            if (textura != null){
                Vector3 escalado = Vector3.one * mySprite.pixelsPerUnit;
                escalado.x /= textura.width;
                escalado.y /= textura.height;
                this.transform.localScale = escalado;
            } 
        }

        Destroy(this);
    }

}
