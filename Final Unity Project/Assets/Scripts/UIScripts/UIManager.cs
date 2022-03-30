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
    private int savedCurrentEnergy;
    private int savedMaxEnergy;


  void Start() {
    EnergyEvents.EnergyUIChange += ChangeEnergyLevels;
    maxEnergy = 4;
    currentEnergy = maxEnergy;
    ChangeUI();
    bar.maxValue = maxEnergy;
    ChangeSlider();
  }


  private void ChangeUI() {
    energyLevelText.text = "Energy: " + currentEnergy.ToString();
  }


  private void ChangeEnergyLevels(int current, int max) {
    currentEnergy = current;
    maxEnergy = max;
        if(currentEnergy > maxEnergy)
        {
            currentEnergy = maxEnergy;
        }
    ChangeUI();
    ChangeSlider();
  }

    //Added by Ilia
    private void ChangeSlider()
    {
        bar.value =  currentEnergy;
    }
    public void SaveCurrentState()
    {
        savedCurrentEnergy = this.currentEnergy;
        savedMaxEnergy = this.maxEnergy;
    }
    public void ResetState()
    {
        this.currentEnergy = savedCurrentEnergy;
        this.maxEnergy = savedMaxEnergy;

        EnergyEvents.EnergyChange(savedCurrentEnergy, savedMaxEnergy);
    }
}
