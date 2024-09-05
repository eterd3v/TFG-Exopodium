using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalTrigger : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    [SerializeField]
    ContadorTiempoPista contador = null;

    [SerializeField]
    AnimationClip animacionSalidaAlter = null;

    [SerializeField]
    Animator animacionesUI = null;

    [SerializeField]
    AudioSource musica = null;

    bool once = true;
    /// <summary>
    /// OnTriggerEnter is called when the Collider other enters the trigger.
    /// </summary>
    /// <param name="other">The other Collider involved in this collision.</param>
    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" && once) {    // solo para filtrar lo que ocurre
            once = false;                       // Para evitar problemas, que solo ocurra una vez esto
            contador.enPlay = false;
            // Pasar el tiempo del contador al MainManager, o donde sea, para tener el record máximo personal
            segundosRestantesAnimacion = segundosMusicaFadeOut;
            onceMusicaFadeOut = true;                   // Va reduciendo el volumen de la musica
            animacionesUI.SetBool("escenaOut",true);    // Reproduce la animación
            // Parar al jugador. Usar los mismos métodos que se usa en las pausas para detener al jugador
        }
    }

    [SerializeField]
    float segundosMusicaFadeOut = 3f;
    float segundosRestantesAnimacion = 0f;
    bool onceMusicaFadeOut = false;
    // Update is called once per frame
    void Update()
    {
        if (onceMusicaFadeOut){
            float cantidadRestar = Time.deltaTime / segundosMusicaFadeOut;
            if (Mathf.Approximately(musica.volume,0f)){
                onceMusicaFadeOut = false;
            }else{
                musica.volume = Mathf.Clamp01(segundosRestantesAnimacion/animacionSalidaAlter.length);
                segundosRestantesAnimacion -= cantidadRestar;
            }
        }

    }
}
