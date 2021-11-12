using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaturationControl : MonoBehaviour
{
  private bool colored;
  [SerializeField]
  private Material material;
  private float saturationLevel;


    void Start() {
        colored = false;
        ShaderEvents.SaturationChange += BoolChange;
        saturationLevel = 1;
    }


    void Update() {
      if (!colored) {
        ReduceSat();
      } else if (colored) {
        IncreaseSat();
      }

      material.SetFloat("_Saturation", saturationLevel);
      Debug.Log(saturationLevel);
    }


    private void ReduceSat(){
      if (saturationLevel < 1) {
        saturationLevel += 0.5f * Time.deltaTime;
      }
    }

    private void IncreaseSat(){
      if (saturationLevel > 0) {
        saturationLevel -= 0.5f * Time.deltaTime;
      }
    }


    private void BoolChange() {
      colored = !colored;
    }
}
