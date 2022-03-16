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

    private void Update()
    {
        if (currentEnergy == maxEnergy - 4)
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                foreach (GameObject gameObject in EnergyEvents.objectsColored.ToArray())
                {
                    EnergyEvents.ChangeColor(gameObject);
                }
                EnergyEvents.objectsColored.Clear();
                currentEnergy = maxEnergy;
            }
        }
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
