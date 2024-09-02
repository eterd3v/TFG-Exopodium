using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class Parallax : MonoBehaviour
{
    private float length;
    private float startpos;
    public GameObject cam;
    public float parallaxEffect;

    public Transform objetoParallax;
    public Transform objetoColindante;

    void Start()
    {
        startpos = transform.localPosition.x;
        if (this.transform.position.x > objetoColindante.position.x)
            length = this.transform.position.x - objetoColindante.position.x;
        else
            length = objetoColindante.position.x - this.transform.position.x;
        length = 1f; 
    }

    void Update()
    {

        float temp = cam.transform.position.x * (1 - parallaxEffect);
        float dist = cam.transform.position.x * parallaxEffect;

        transform.localPosition = new Vector3(startpos + dist, transform.localPosition.y, transform.localPosition.z);

        if (temp > startpos + length) {
            startpos += length;
        } else if (temp < startpos - length) {
            startpos -= length;
        }
        
    }
}
