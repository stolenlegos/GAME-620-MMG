using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyManager : MonoBehaviour {
  public int maxEnergy = 4;
  public int currentEnergy;
    private float startTime = 0f;
    private float holdTime = 3.0f;
    //public List<GameObject> objectsColoredList = new List<GameObject>();
    private bool objColored;


  void Start() {
    currentEnergy = maxEnergy;
    EnergyEvents.checkEnergy += CheckEnergy;
  }

    private void Update()
    {
        //objectsColoredList = EnergyEvents.objectsColored;
        if (currentEnergy != maxEnergy)
        {                
                Debug.Log("Phase1");
                if (Input.GetKey(KeyCode.F))
                {
                    Debug.Log("Phase2");
                    startTime += Time.deltaTime;
                    Debug.Log(startTime);
                    if (startTime > holdTime)
                    {
                        Debug.Log("Phase3");
                        foreach (GameObject gameObject in EnergyEvents.objectsColored.ToArray())
                        {
                            EnergyEvents.ChangeColor(gameObject);
                        }
                        EnergyEvents.objectsColored.Clear();
                        currentEnergy = maxEnergy;
                    }
                }           
        }
        else if(startTime > 0 && currentEnergy == maxEnergy)
        {
            startTime = 0;
        }
        if (currentEnergy > maxEnergy)
        {
            currentEnergy = maxEnergy;
        }
    }
    private void CheckEnergy(GameObject obj) {
    objColored = obj.GetComponent<SaturationControl>().colored;

    if (objColored) {
      ShaderEvents.ChangeColor(obj);
      currentEnergy += 1;
            Debug.Log("EnergyAdded");
    } else if (!objColored && currentEnergy > 0) {
      ShaderEvents.ChangeColor(obj);
      currentEnergy -= 1;
            Debug.Log("EnergyRemoved");
        }

    EnergyEvents.EnergyChange(currentEnergy, maxEnergy);
  }
}
