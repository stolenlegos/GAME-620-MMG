using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyManager : MonoBehaviour {
  public int maxEnergy = 4;
  public int currentEnergy;
    public int savedCurrentEnergy;
    public int savedMaxEnergy;
    private float startTime = 0f;
    private float holdTime = 3.0f;
    private bool objColored;
    public List<GameObject> savedObjectsColored = new List<GameObject>();


  void Start() {
    currentEnergy = maxEnergy;
    EnergyEvents.checkEnergy += CheckEnergy;
  }

    private void Update()
    {
        if (currentEnergy != maxEnergy)
        {                
                if (Input.GetMouseButton(0))
                {
                    startTime += Time.deltaTime;
                    
                    if (startTime > holdTime)
                    {
                        foreach (GameObject gameObject in EnergyEvents.objectsColored.ToArray())
                        {
                            EnergyEvents.ChangeColor(gameObject);
                        }
                        EnergyEvents.objectsColored.Clear();
                        currentEnergy = maxEnergy;
                    }
                }
                else if (!Input.GetMouseButton(0) && startTime != 0f)
                {
                startTime -= Time.deltaTime;

                    if (startTime < 0f)
                    {
                        startTime = 0;
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
        //Debug.Log("Objects in colored list " + EnergyEvents.objectsColored.Count);
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
    public void SaveCurrentState()
    {
        savedCurrentEnergy = this.currentEnergy;
        savedMaxEnergy = this.maxEnergy;
        savedObjectsColored = new List<GameObject>();
        foreach(GameObject obj in EnergyEvents.objectsColored)
        {
            savedObjectsColored.Add(obj);
        }
    }
    public void ResetState()
    {
        this.currentEnergy = savedCurrentEnergy;
        this.maxEnergy = savedMaxEnergy;
        EnergyEvents.objectsColored = savedObjectsColored;
    }
}
