using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;

public class MainManager : MonoBehaviour {

    public static MainManager instance;

    public PlayerData playerData;

    public LocalizationSettings traducciones;

    public int currentLevel = 0;

    private void Awake()
    {
        // start of new code
        if (instance != null) {
            Destroy(gameObject);
            return;
        }
        // end of new code
        instance = this;
        playerData = new PlayerData();
        DontDestroyOnLoad(gameObject);
    }

    public void LoadSceneIndex(int sceneIndex) {
        currentLevel = sceneIndex;
        SceneManager.LoadScene(sceneIndex);
    }

    public void LoadNextLevelNum() {
        currentLevel = 1 + currentLevel;
        LoadSceneIndex(currentLevel);
    }

    public void LoadReset() {
        LoadSceneIndex(currentLevel);
    }

    public void ExitGame(){
        Application.Quit();
    }

    public void SetDificultad(int nuevaDificultad){
        playerData.dificultadActual = nuevaDificultad;
    }

    public void SeedAleatoria(bool estado) {
        playerData.semillaAleatoria = estado;
    }

    public void CargarSemilla(string seed){
        Debug.Log(seed);
        playerData.semillaCargar = int.Parse(seed);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SeleccionarIdioma(string str) {
        List<Locale> ilp = traducciones.GetAvailableLocales().Locales;
        if (ilp != null)
            switch (str) {
                case "en": {
                    traducciones.SetSelectedLocale(ilp[0]);
                    break;
                }
                case "es": {
                    traducciones.SetSelectedLocale(ilp[1]);
                    break;
                }
                default: {
                    break;
                }
            }
    }

    void OnDestroy() {
        Debug.Log("Se ha destruido el objeto " + this.name + " porque ya existe " + MainManager.instance.name);
        if (this.playerData != null && MainManager.instance.playerData!= null){
            MainManager.instance.playerData.CopyValues(this.playerData);
        }
        MainManager.instance.playerData = this.playerData;
    }
}
