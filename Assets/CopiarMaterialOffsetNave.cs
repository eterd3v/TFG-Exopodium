using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CopiarMaterialOffsetNave : MonoBehaviour
{


    Material este=null;

    [SerializeField]
    NaveMovimiento nave = null;

    // Start is called before the first frame update
    void Start()
    {
        este = this.GetComponent<MeshRenderer>().material;
    }

    // Update is called once per frame
    void Update()
    {
        if (nave != null && este != null)
            este.mainTextureOffset = nave.parallax.mainTextureOffset;
    }
}
