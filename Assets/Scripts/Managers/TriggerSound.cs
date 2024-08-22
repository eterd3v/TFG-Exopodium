using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerSound : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public bool terminoReproduccion = false; // Variable para esperar a que termine
    public bool porTrigger;
  
    float previousVolume;

    public float reduccionDuracionSprint = 0.65f, reduccionDuracionAndar=0.9f;

    // Update is called once per frame
    void Update() {
        terminoReproduccion = false;
        int randomIndex = Random.Range(0, sonidos.Length);
        previousVolume = SoundManager.instancia.FuenteEfectos.volume;
        SoundManager.instancia.FuenteEfectos.volume = volumenes[randomIndex]; // Entre 0 y 1 siempre
        SoundManager.instancia.PlayClip(sonidos[randomIndex]);
        StartCoroutine(esperaSonido(sonidos[randomIndex].length * reduccionDuracionAndar));
    }

    IEnumerator esperaSonido(float segundos) {
        yield return new WaitForSeconds(segundos);
        //SoundManager.instancia.PlayEfectos.volume = previousVolume; 
        terminoReproduccion = true;
    }



    void OnTriggerEnter() {
        if (porTrigger) {
            triggerSFX();
        }
    }

    public AudioClip[] sonidos;
    public float[] volumenes;


    IEnumerator SFX(){
        if (sonidos.Length == 1) {
            SoundManager.instancia.FuenteEfectos.volume = volumenes[0];
            SoundManager.instancia.PlayClip(sonidos[0]);
            yield return new WaitForSeconds(sonidos[0].length);
        } else if (sonidos.Length > 1) {
            SoundManager.instancia.FuenteEfectos.volume = 1.0f;
            SoundManager.instancia.RandomSoundEffect(sonidos);
        }
    }

    public void triggerSFX() {
        StartCoroutine(SFX());
    }

}
