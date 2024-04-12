using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BezierPoint : MonoBehaviour
{

    public Material material;
    public Light luz;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void NuevaLuz(float t){

        float uMt = 1.0f - t;
        float dgs90 = Mathf.PI / 2.0f;
        float bC = Mathf.Sin(dgs90*(t+1.0f)); bC *= bC * bC * uMt * uMt;
        float gC = Mathf.Sin(dgs90*(t+0.5f)); gC *= gC *      (1-bC) * uMt;
        float rC = Mathf.Sin(dgs90*t)       ; rC *= rC * rC * (1-bC) * (1-gC);

        Vector3 rgb = new Vector3(rC,gC,bC);

        if (rgb.x < 0) rgb.x = 0.0f;
        if (rgb.y < 0) rgb.y = 0.0f;
        if (rgb.z < 0) rgb.z = 0.0f;

        if (rgb.x > 1) rgb.x = 1.0f/rgb.x;
        if (rgb.y > 1) rgb.y = 1.0f/rgb.y;
        if (rgb.z > 1) rgb.z = 1.0f/rgb.z;

        if (luz != null) {
            luz.color = new Color(rgb.x,rgb.y,rgb.z,1.0f);
        }
    }

    public void AplicarMaterial() {
        if (material != null && luz != null) {
            material.SetColor("_Color",luz.color);              // https://docs.unity3d.com/ScriptReference/Material.SetColor.html
            material.SetColor("_EmissionColor",luz.color);
        }
    }
}
