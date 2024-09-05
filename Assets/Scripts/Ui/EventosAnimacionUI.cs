using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventosAnimacionUI : MonoBehaviour
{
    bool contador321Terminado = false;

    // Getters
    public bool IsContadorTerminado() {return contador321Terminado;}

    // Setters
    public void OppositeContadorTerminado(){
        contador321Terminado = !contador321Terminado;
    }

}
