using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class UIManager : MonoBehaviour
{

    public GameObject[] paneles;
    public GameObject fondoUI;
    public int panelInicialActivo;

    public void cambiarPor(int i) {
        // Debug.Log("Activo: " + panelInicialActivo + ", i: " + i);
        paneles[panelInicialActivo].SetActive(false);
        paneles[i].SetActive(true);
        panelInicialActivo = i;
    }

    public void DesactivarTodo() {
        for (int i = 0; i < paneles.Length; i++)
        {
            paneles[i].SetActive(false);
        }
    }

    public void ActivarInicial(){
        paneles[0].SetActive(true);
    }

    

}
