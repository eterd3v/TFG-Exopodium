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

    public void cambiarPor(int i) {
        // Debug.Log("Activo: " + panelInicialActivo + ", i: " + i);
        paneles[panelInicialActivo].SetActive(false);
        paneles[i].SetActive(true);
        panelInicialActivo = i;
    }

    public void btnJugar() {
        paneles[panelInicialActivo].SetActive(false);
        // MainManager.instance.SwitchPausa();
        fondoUI.SetActive(false);
    }
}
