using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_dificultad : MonoBehaviour
{

    [SerializeField]
    GameObject[] dificultades;  // De Tamaño 3

    int posActual = 1;

    public void izq() {
        dificultades[posActual--].SetActive(false);
        if (posActual < 0)
            posActual = 2;
        dificultades[posActual].SetActive(true);
        MainManager.instance.SetDificultad(posActual);
    }

    public void der() {
        dificultades[posActual++].SetActive(false);
        if (posActual > 2)
            posActual = 0;
        dificultades[posActual].SetActive(true);
        MainManager.instance.SetDificultad(posActual);
    }
}
