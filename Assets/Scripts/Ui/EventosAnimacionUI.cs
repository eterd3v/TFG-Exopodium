using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventosAnimacionUI : MonoBehaviour
{
    bool contador321Terminado = false;

    [SerializeField]
    NaveMovimiento nave = null;

    // Getters
    public bool IsContadorTerminado() {return contador321Terminado;}

    // Setters
    public void OppositeContadorTerminado(){
        contador321Terminado = !contador321Terminado;
        if (nave != null && contador321Terminado){
            nave.Reanudar();
        }
    }

}
