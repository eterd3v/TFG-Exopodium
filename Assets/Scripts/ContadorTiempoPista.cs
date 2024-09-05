using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ContadorTiempoPista : MonoBehaviour {

    [SerializeField]
    EventosAnimacionUI listosYa = null;

    TMP_Text contadorInterfaz = null;

    void Start() {
        contadorInterfaz = this.GetComponent<TMP_Text>();
    }

    bool tiempoEnMarcha = false;
    public bool enPlay=true;
    float tiempoTranscurrido = 0f;

    // Update is called once per frame
    void Update() {
        if (listosYa != null){
            tiempoEnMarcha = listosYa.IsContadorTerminado();
        }

        if (tiempoEnMarcha && enPlay){
            tiempoTranscurrido += Time.deltaTime;
            contadorInterfaz.text = sacarTiempoTranscurrido(tiempoTranscurrido);
        } 

    }

    int iMinutos=0, iSegundos=0;
    float fMilisegundos=0f;

    public string sacarTiempoTranscurrido(float t) {
        string espaciador = " : ";
        iMinutos = Mathf.FloorToInt(t/60f);
        iSegundos = Mathf.FloorToInt(t%60f);
        fMilisegundos = 100f * (t - (float)iMinutos*60f - (float)iSegundos);
        return iMinutos.ToString() + espaciador + iSegundos.ToString() + espaciador + Mathf.CeilToInt(fMilisegundos).ToString();
    }
}
