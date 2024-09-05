using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour {

	// Componentes de reproductores de audio
	public AudioSource FuenteEfectos;
	public AudioSource FuenteMusica;

	// Rango para ajustar el pitch
	public float LowPitchRange = 0.95f;
	public float HighPitchRange = 1.05f;

    public static SoundManager instancia = null;

    void Awake() {
		inicializar();
    }

    public void inicializar() {
		if (instancia != null) {    // Singleton
			SoundManager.instancia.FuenteMusica = this.FuenteMusica;
			SoundManager.instancia.FuenteEfectos = this.FuenteEfectos;
            Destroy(gameObject);
            return;
        }

        instancia = this;
        DontDestroyOnLoad(gameObject);
        inicializar();
    }

	public void PlaySource(AudioSource source){
		source.Play();
	}

	// Reproducir a través de la fuente de efectos.
	public void PlayClip(AudioClip clip) {
		FuenteEfectos.clip = clip;
		FuenteEfectos.Play();
	}

		// Reproducir a través de la fuente de efectos.
	public void PlayEfectoSource(AudioClip clip, AudioSource source) {
		source.clip = clip;
		source.Play();
	}

	// Reproducir a través de la fuente de música.
	public void PlayMusica(AudioClip clip) {
		FuenteMusica.clip = clip;
		FuenteMusica.Play();
	}

    public void StopEfectos(){
        FuenteEfectos.Stop();
    }

    public void StopMusica(){
        FuenteMusica.Stop();
    }

	public void RandomSoundEffect(params AudioClip[] clips) {	// Con FuenteEfectos
		int randomIndex = Random.Range(0, clips.Length);
		float randomPitch = Random.Range(LowPitchRange, HighPitchRange);

		FuenteEfectos.pitch = randomPitch == null ? 1.0f : randomPitch;
		FuenteEfectos.clip = clips[randomIndex];
		FuenteEfectos.Play();
	}


}
