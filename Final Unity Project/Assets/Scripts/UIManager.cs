using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {
  [SerializeField]
  private Text energyLevelText;
    [SerializeField]
    private Slider bar;
  private int currentEnergy;
  private int maxEnergy;


  void Start() {
    EnergyEvents.EnergyUIChange += ChangeEnergyLevels;
    maxEnergy = 3;
    currentEnergy = maxEnergy;
    ChangeUI();
    ChangeSlider();
  }


  private void ChangeUI() {
    energyLevelText.text = "Energy: " + currentEnergy.ToString();
  }


  private void ChangeEnergyLevels(int current, int max) {
    currentEnergy = current;
    maxEnergy = max;
    ChangeUI();
    ChangeSlider();
  }

    //Added by Ilia
    private void ChangeSlider()
    {
        bar.value =  currentEnergy;
    }
}
