using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyEvents  {
    public delegate void PlayerSatInput (GameObject obj);
    public static event PlayerSatInput checkEnergy;

    public delegate void EnergyLevelChange (int current, int max);
    public static event EnergyLevelChange EnergyUIChange;


    public static void ChangeColor (GameObject obj) {
      if (checkEnergy != null) {
        checkEnergy(obj);
      }
    }


    public static void EnergyChange (int current, int max) {
      if (EnergyUIChange != null) {
        EnergyUIChange(current, max);
      }
    }
}
