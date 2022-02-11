using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyManager : MonoBehaviour {
  public int maxEnergy = 4;
  public int currentEnergy;
  private bool objColored;


  void Start() {
    currentEnergy = maxEnergy;
    EnergyEvents.checkEnergy += CheckEnergy;
  }


  private void CheckEnergy(GameObject obj) {
    objColored = obj.GetComponent<SaturationControl>().colored;

    if (objColored) {
      ShaderEvents.ChangeColor(obj);
      currentEnergy += 1;
    } else if (!objColored && currentEnergy > 0) {
      ShaderEvents.ChangeColor(obj);
      currentEnergy -= 1;
    }

    EnergyEvents.EnergyChange(currentEnergy, maxEnergy);
  }
}
