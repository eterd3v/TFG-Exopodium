using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class UIManager : MonoBehaviour
{

    public GameObject[] paneles;
    public GameObject fondoUI;
    public int panelInicialActivo;
    public int panelOpcionEnJuego;

    
    // Start is called before the first frame update
    void Start() {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            // TODO: Se pausa el juego...
            // y ademas...
            cambiarPor(panelOpcionEnJuego);
        }
    }

    public void cambiarPor(int i) {
        // Debug.Log("Activo: " + panelInicialActivo + ", i: " + i);
        paneles[panelInicialActivo].SetActive(false);
        paneles[i].SetActive(true);
        panelInicialActivo = i;
    }

    public void btnJugar() {
        paneles[panelInicialActivo].SetActive(false);
        fondoUI.SetActive(false);
    }
}
