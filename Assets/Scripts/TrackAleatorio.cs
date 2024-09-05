using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

public class TrackAleatorio : MonoBehaviour
{

    [SerializeField]
    AudioClip[] clips = null;

    // Start is called before the first frame update
    void Start()
    {
        AudioSource fuenteSonidos = this.GetComponent<AudioSource>();
        if (fuenteSonidos != null && clips.Length > 0){
            Random rand = new Random();
            fuenteSonidos.clip = clips[rand.Next()%clips.Length];
            fuenteSonidos.Play();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
