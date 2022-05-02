using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {
    public static UIManager instance;

    [SerializeField]
  private Text energyLevelText;
  [SerializeField]
  private Slider bar;
  [SerializeField]
  private Slider energyReturnSlider;
  [SerializeField]
  private Text energyReturnSliderText;
    public Image transBar;
    public Image fillBar;
    public GameObject Timer;
  //private 
  private int currentEnergy;
  private int maxEnergy;
    private float energyFall;
  private float startTime = 0f;
  private float holdTime = 3.0f;
  private int savedCurrentEnergy;
  private int savedMaxEnergy;
    private float fillSpeed = .25f;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(instance);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start() {
        if (energyLevelText != null && bar != null && energyReturnSlider != null && energyReturnSliderText != null && transBar != null && fillBar != null && Timer != null)
        {
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
        else if (energyReturnSlider != null){ energyReturnSlider.gameObject.SetActive(false); ChangeEnergyReturnSlider(); }
        if(transBar.fillAmount >= fillBar.fillAmount)
        {
            transBar.fillAmount -= fillSpeed * Time.deltaTime;
        }
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
        if(transBar.fillAmount <= fillBar.fillAmount)
        {
            transBar.fillAmount = fillBar.fillAmount;
        }
    }
    //end
    private void ChangeEnergyReturnSlider()
    {
        energyReturnSlider.value = startTime;
        if (energyReturnSlider.value > 0f && energyReturnSlider.value <= .99f)
        {
            energyReturnSliderText.text = "3";
        }
        else if (energyReturnSlider.value >= 1f && energyReturnSlider.value <= 1.99f)
        {
            energyReturnSliderText.text = "2";
        }
        else if (energyReturnSlider.value >= 2f && energyReturnSlider.value <= 2.99f)
        {
            energyReturnSliderText.text = "1";
        }
        if (!Input.GetMouseButton(0) && startTime != 0f)
        {
            energyReturnSlider.value = startTime -= Time.deltaTime;
            if (energyReturnSlider.value <= 0)
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
    public void DisplayTimer(Transform buttonPosition, float amount)
    {
        if (amount < 100f)
        {
            GameObject timer;
            timer = Instantiate(Timer, buttonPosition.position, buttonPosition.rotation);
            timer.transform.parent = buttonPosition;
        }
    }
    
}
