using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {
  [SerializeField]
  private Text energyLevelText;
  [SerializeField]
  private Slider bar;
  [SerializeField]
  private Slider energyReturnSlider;
  private int currentEnergy;
  private int maxEnergy;
  private float startTime = 0f;
  private float holdTime = 3.0f;
  private int savedCurrentEnergy;
  private int savedMaxEnergy;


  void Start() {
    EnergyEvents.EnergyUIChange += ChangeEnergyLevels;
    maxEnergy = 4;
    currentEnergy = maxEnergy;
    ChangeUI();
    bar.maxValue = maxEnergy;
    energyReturnSlider.maxValue = holdTime;
    ChangeSlider();
    ChangeEnergyReturnSlider();
    energyReturnSlider.gameObject.SetActive(false);
  }

    private void Update()
    {
        if (currentEnergy != maxEnergy)
        {
            if (Input.GetMouseButton(0))
            {
                startTime += Time.deltaTime;
                energyReturnSlider.gameObject.SetActive(true);
                ChangeEnergyReturnSlider();
            }
            else if (!Input.GetMouseButton(0) && startTime > 0f)
            {
                ChangeEnergyReturnSlider();
            }
        }
        else { energyReturnSlider.gameObject.SetActive(false); ChangeEnergyReturnSlider(); }
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
    //end
    private void ChangeEnergyReturnSlider()
    {
        energyReturnSlider.value = startTime;
        if(!Input.GetMouseButton(0) && startTime != 0f)
        {
            energyReturnSlider.value = startTime -= Time.deltaTime;
            if(energyReturnSlider.value <= 0)
            {
                energyReturnSlider.value = 0;
                startTime = 0;
            }
        }
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
