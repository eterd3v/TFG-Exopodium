using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_intro_juego : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    [SerializeField]
    GameObject signal = null, activar=null;

    // Update is called once per frame
    void Update()
    {
        if (signal != null && activar != null) {
            if (!signal.activeSelf){
                activar.SetActive(true);
                Destroy(this.gameObject);
            }
        }
    }
}
