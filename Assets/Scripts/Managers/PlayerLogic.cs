using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLogic : MonoBehaviour {

    public float[] actualValues;
    public float[] intensityMultipliers;
    public float[] minValues;
    public float[] maxValues;

    public Vector3 volumes = Vector3.one; // Volumenes de audio

    public bool simplified = false;

    //public float max, min;
    // 0.01 Salud
    // 0.01 Stamina
    //Cordura
    void Start(){
    }

    public void CopyValues(PlayerLogic other) {
        other.actualValues = this.actualValues;
        other.intensityMultipliers = this.intensityMultipliers;
        other.minValues = this.minValues;
        other.maxValues = this.maxValues;
        other.volumes = this.volumes;
    }

    // Update is called once per frame
    void Update() {
    }

    public float percentageValue(int i) {    // Hearth, Lungs, Eye
        float temp = actualValues[i] - minValues[i];
        return temp / totalValue(i);
    }

    public float totalValue(int i) {    // Hearth, Lungs, Eye
        return maxValues[i] - minValues[i];
    }

    public void applyPercentage(int i, float percentage) {
        float temp = percentage * actualValues[i];
        if (temp >= minValues[i] && temp <= maxValues[i]) {
            actualValues[i] = temp;
        }
    }

    public void applyDecrease(int i, float decrease) {
        float temp = actualValues[i] - decrease;
        if (temp >= minValues[i] && temp <= maxValues[i]) {
            actualValues[i] = temp;
        }

    }

    public void applyIncrease(int i, float increase) {
        float temp = actualValues[i] + increase;
        //Debug.Log("Temp: " + temp);
        //Debug.Log("Max: " + maxValues[i]);

        if (temp >= minValues[i] && temp <= maxValues[i]) {
            actualValues[i] = temp;
        }
        else if (temp >= maxValues[i] && i != actualValues.Length - 2)
        {
            MainManager.instance.LoadCurrentScene();
        }
    }

    public float getReal(int i) {

        return actualValues[i] - minValues[i];
    }

    public bool isZero(int i)
    {
        if(maxValues[i] - actualValues[i] <= 0) {
            return true;
        }
        return false;
    }

}
