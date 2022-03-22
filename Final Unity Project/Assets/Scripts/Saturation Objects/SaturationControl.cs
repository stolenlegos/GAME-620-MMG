using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaturationControl : MonoBehaviour {
  public bool colored;
    public bool hovered;
    private bool swap;
    private PlayerObjectInteractions POI;
    [SerializeField]
  private Material material;
  private float saturationLevel;


    void Start() {
        POI = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerObjectInteractions>();
        colored = false;
        ShaderEvents.SaturationChange += BoolChange;
        saturationLevel = 1;
    }


    void Update() {
      if (!colored && !hovered) {
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
      if (saturationLevel > 0) {
        saturationLevel -= 0.5f * Time.deltaTime;
      }
    }

    private void ReduceSat2()
    {
        if (saturationLevel < 1)
        {
            saturationLevel += 0.8f * Time.deltaTime;
        }
    }


    private void IncreaseSat2()
    {
        if (saturationLevel > 0)
        {
            saturationLevel -= 0.8f * Time.deltaTime;
        }
    }


    private void BoolChange (GameObject obj) {
      if (obj == this.gameObject) {
        colored = !colored;
            if (colored)
            {
                EnergyEvents.objectsColored.Add(this.gameObject);
                //Debug.Log(EnergyEvents.objectsColored);
            }
            if (!colored)
            {
                EnergyEvents.objectsColored.Remove(this.gameObject);
            }
            //Debug.Log("Ran");
      }
    }
    private void OnMouseOver()
    {
        hovered = true;
        if (!colored && hovered)
        {
            if (saturationLevel > 0 && !swap)
            {
                IncreaseSat2();
                if (saturationLevel <= 0f)
                {
                    swap = true;
                }
            }
            else if (saturationLevel < 1 && swap)
            {
                ReduceSat2();
                if (saturationLevel >= 1f)
                {
                    swap = false;
                }
            }
        }
    }
    private void OnMouseEnter()
    {
        if (this.tag != "Spiral" || this.tag != "Examiner")
        {
            if (POI._objectsNear.Contains(this.gameObject))
            {
                POI._objectsNear.Remove(this.gameObject);
                POI._objectsNear.Add(this.gameObject);
            }
            POI._objectsToColor.Add(this.gameObject);
        }
    }
    private void OnMouseExit()
    {
        hovered = false;
        if (!colored)
        {
            saturationLevel = 1;
        }
        if (!hovered) {
            PlayerActions.ObjectDropped(this.gameObject);
            POI._objectsToColor.Remove(this.gameObject);
        }
    }


}
