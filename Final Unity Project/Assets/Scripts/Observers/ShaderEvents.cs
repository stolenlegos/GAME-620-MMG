using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShaderEvents  {
    public delegate void Saturation (GameObject obj);
    public static event Saturation SaturationChange;

    public static void ChangeColor(GameObject obj) {
      if (SaturationChange != null) {
        //Debug.Log(obj);
        SaturationChange(obj);
      }
    }
}
