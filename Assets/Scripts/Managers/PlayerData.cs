using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData {

    public Vector2 volumes = Vector2.one; // Volumenes de audio

    //public List<string> records = new List<string>();
    //public List<int> semillas = new List<int>();
    //public List<int> dificultades = new List<int>();

    public int dificultadActual = 1;
    public int semillaCargar = 0;
    public bool semillaAleatoria = false;

    void Start() {

    }

    public void CopyValues(PlayerData other) {
        other.volumes = this.volumes;
//        other.records = this.records;
  //      other.semillas = this.semillas;
        other.dificultadActual = this.dificultadActual;
        other.semillaCargar = this.semillaCargar;
        other.semillaAleatoria = this.semillaAleatoria;
    }

    // Update is called once per frame
    void Update() {
    }


}
