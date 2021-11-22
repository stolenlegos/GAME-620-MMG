using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {
  private int energyLevel;
  [SerializeField]
  private Text energyLevelText;


  void Start() {
    energyLevel = 3;
    EnergyEvents.addEnergy += IncreaseEnergy;
    EnergyEvents.subtractEnergy += DecreaseEnergy;
    ChangeUI();
  }


  private void ChangeUI() {
    energyLevelText.text = "Energy: " + energyLevel.ToString();
  }


  private void IncreaseEnergy(int num) {
    energyLevel += num;
    ChangeUI();
  }


  private void DecreaseEnergy(int num) {
    energyLevel -= num;
    ChangeUI();
  }
}
