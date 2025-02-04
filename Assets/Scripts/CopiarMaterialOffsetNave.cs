using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CopiarMaterialOffsetNave : MonoBehaviour
{


    Material este=null;

    // Start is called before the first frame update
    void Start()
    {
        este = this.GetComponent<MeshRenderer>().material;
    }

    // Mejora

    public static Vector2 offsetComun = Vector2.zero;

    void Update()
    {
        este.mainTextureOffset = offsetComun;
    }
}
