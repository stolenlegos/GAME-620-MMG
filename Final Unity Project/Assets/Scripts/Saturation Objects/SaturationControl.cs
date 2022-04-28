using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaturationControl : MonoBehaviour {
  public bool colored;
    public bool hovered;
    public Vector3 savedPosition;
    private Transform playerPostion;
    private GameObject coloredEnergy;
    public bool savedColorState;
    private bool swap;
    private PlayerObjectInteractions POI;
    [SerializeField]
  private Material material;
  private float saturationLevel;


    void Start() {
        POI = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerObjectInteractions>();
        playerPostion = GameObject.FindGameObjectWithTag("Player").transform;
        coloredEnergy = GameObject.FindGameObjectWithTag("ColoredEnergy");
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
        playerPostion = GameObject.FindGameObjectWithTag("Player").transform;
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

    private void ReduceSat2(){
        if (saturationLevel < 1){
            saturationLevel += 0.8f * Time.deltaTime;
        }
    }

    private void IncreaseSat2(){
        if (saturationLevel > 0){
            saturationLevel -= 0.8f * Time.deltaTime;
        }
    }

    private void BoolChange (GameObject obj) {
      if (obj == this.gameObject) {
        colored = !colored;
            if (colored && this.gameObject.tag != "Door"){
                EnergyEvents.objectsColored.Add(this.gameObject);
                EnergyGive();
                //Debug.Log(EnergyEvents.objectsColored);
            }
            if (!colored && this.gameObject.tag != "Door"){
                EnergyEvents.objectsColored.Remove(this.gameObject);
                EnergyReturn();
            }
            //Debug.Log("Ran");
      }
    }
    private void OnMouseOver(){
        hovered = true;
        if (!colored && hovered){
            if (saturationLevel > 0 && !swap){
                IncreaseSat2();
                if (saturationLevel <= 0f){
                    swap = true;
                }
            }
            else if (saturationLevel < 1 && swap){
                ReduceSat2();
                if (saturationLevel >= 1f){
                    swap = false;
                }
            }
        }
    }
    private void OnMouseEnter(){
        swap = false;
        if (this.tag != "Spiral" || this.tag != "Examiner"){
            if (POI._objectsNear.Contains(this.gameObject)){
                POI._objectsNear.Remove(this.gameObject);
                POI._objectsNear.Add(this.gameObject);
            }
            POI._objectsToColor.Add(this.gameObject);
        }
    }
    private void OnMouseExit(){
        hovered = false;
        if (!colored){
            saturationLevel = 1;
        }
        if (!hovered) {
            PlayerActions.ObjectDropped(this.gameObject);
            POI._objectsToColor.Remove(this.gameObject);
        }
    }
    public void Pulse(){
        if (saturationLevel > 0 && !swap){
            IncreaseSat2();
            if (saturationLevel <= 0f){
                swap = true;
                Debug.Log("Swap is true");
            }
        }
        else if (saturationLevel < 1 && swap){
            ReduceSat2();
            if (saturationLevel >= 1f){
                swap = false;
                Debug.Log("Swap is false");
            }
        }
        if(saturationLevel > 1f)
        {
            swap = false;
            Debug.Log("Pulse Break");
        }
    }
    private void EnergyGive()
    {
        GameObject energyBall;
        energyBall = Instantiate(coloredEnergy, playerPostion.position, playerPostion.rotation);
        energyBall.GetComponent<EnergyTransfer>().giving = true;
        energyBall.GetComponent<EnergyTransfer>().targetObject = this.gameObject;
        //Debug.Log("Give");
    }
    private void EnergyReturn()
    {
        GameObject energyBall;
        energyBall = Instantiate(coloredEnergy, this.gameObject.transform.position, this.gameObject.transform.rotation);
        energyBall.GetComponent<EnergyTransfer>().returning = true;
        //Debug.Log("Return");
    }
    public void SaveCurrentState(){
        savedPosition = this.transform.position;
        savedColorState = this.colored;
    }
    public void ResetState(){
        this.transform.position = savedPosition;
        this.colored = savedColorState;
    }
}
