using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventosAnimacionUI : MonoBehaviour
{
    Animator animator = null;

    void Start(){
        animator = this.GetComponent<Animator>();
    }

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

    public void IrMenuPrincipal() {
        MainManager.instance.LoadSceneIndex(0);
        animator.SetBool("salidaContador",false);
        animator.SetBool("escenaOut",false);
        animator.SetBool("escenaOutAlter",false);
    }

}
