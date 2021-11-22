using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaturationControl : MonoBehaviour {
  private bool colored;
  [SerializeField]
  private Material material;
  private float saturationLevel;
  private int energyLevel;
  private int MaxEnergy = 3;


    void Start() {
        energyLevel = 3;
        colored = false;
        ShaderEvents.SaturationChange += BoolChange;
        saturationLevel = 1;
        EnergyEvents.addEnergy += IncreaseEnergy;
        EnergyEvents.subtractEnergy += DecreaseEnergy;
    }


    void Update() {
      if (!colored) {
        ReduceSat();
      } else if (colored) {
        IncreaseSat();
      }

      material.SetFloat("_Saturation", saturationLevel);
    }


    private void ReduceSat(){
      if (saturationLevel < 1) {
        saturationLevel += 0.5f * Time.deltaTime;
      }
    }


    private void IncreaseSat(){
      if (saturationLevel > 0 && energyLevel > 0) {
        saturationLevel -= 0.5f * Time.deltaTime;
      }
    }


    private void BoolChange (GameObject obj) {
      if (obj == this.gameObject) {
        colored = !colored;

        if (colored && energyLevel > 0) {
          EnergyEvents.SubtractEnergy(1);
        } else if (!colored && energyLevel < MaxEnergy) {
          EnergyEvents.AddEnergy(1);
          //adjust observer to pass energy level through so it is limited
        }
      }
    }


    private void IncreaseEnergy(int num) {
      StartCoroutine(LessEnergy(num));
    }


    private void DecreaseEnergy(int num) {
      StartCoroutine(MoreEnergy(num));
    }


    private IEnumerator LessEnergy(int num) {
      yield return new WaitForSeconds(2.1f);
      energyLevel += num;
    }


    private IEnumerator MoreEnergy(int num) {
      yield return new WaitForSeconds(2.1f);
      energyLevel -= num;
    }
}
