using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyEvents  {
    public delegate void ChangeEnergy (int num);
    public static event ChangeEnergy subtractEnergy;
    public static event ChangeEnergy addEnergy;


    public static void SubtractEnergy(int num) {
      if (subtractEnergy != null) {
        subtractEnergy(num);
      }
    }


    public static void AddEnergy(int num) {
      if (addEnergy != null) {
        addEnergy(num);
      }
    }
}
