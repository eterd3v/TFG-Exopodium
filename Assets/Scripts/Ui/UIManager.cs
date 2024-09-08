using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Slider = UnityEngine.UI.Slider;
using UnityEngine.UIElements;

public class UIManager : MonoBehaviour
{

    public GameObject[] paneles;
    public GameObject fondoUI;
    public int panelInicialActivo;

    public Slider sliderSFX, sliderMusica;

    void Start() {
        sliderSFX.value = MainManager.instance.playerData.volumes.x;
        sliderMusica.value = MainManager.instance.playerData.volumes.y;
    }

    public void SetSFXVolume() {
        MainManager.instance.playerData.volumes.x = sliderSFX.value;
    }

    public void SetMusicVolume() {
        MainManager.instance.playerData.volumes.y = sliderMusica.value;
    }

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

    
    public void ButtonIdioma(string lang){
        MainManager.instance.SeleccionarIdioma(lang);
    }

    public void ButtonSalir(){
        MainManager.instance.ExitGame();
    }

    public void CargarEscena(int escenaIndex){
        MainManager.instance.LoadSceneIndex(escenaIndex);
    }

    public void SeedAleatoria(bool estado){
        MainManager.instance.SeedAleatoria(estado);
    }

    public void CargarSeed(string semilla){
        MainManager.instance.CargarSemilla(semilla);
    }
    

}
