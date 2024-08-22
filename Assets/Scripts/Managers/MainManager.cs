using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;

public class MainManager : MonoBehaviour {

    public bool completado = false;

    public static MainManager instance;

    public PlayerLogic playerLogic;

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

    public void LoadCurrentLevelNum()
    {
        LoadSceneIndex(currentLevel);
    }

    public void LoadCurrentScene(){
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void LoadReset() {
        currentLevel = 0;
        LoadSceneIndex(currentLevel);
    }

    public void LoadSceneString(string sceneName) {
        SceneManager.LoadScene(sceneName);
    }

    public void ExitGame(){
        Application.Quit();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void restaSalud(float damage) {
        playerLogic.applyIncrease(0, damage);
    }

    public void restaCordura(float damage) {
        playerLogic.applyIncrease(2, damage);
    }

    public float getCordura() {
        return playerLogic.getReal(2);
    }

    public bool canRun() {
        if (playerLogic.isZero(1))
        {
            return false;
        }
        else {
            return true;
        }
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
        if (this.playerLogic != null && MainManager.instance.playerLogic!= null){
            MainManager.instance.playerLogic.CopyValues(this.playerLogic);
        }
        MainManager.instance.playerLogic = this.playerLogic;
    }
}
