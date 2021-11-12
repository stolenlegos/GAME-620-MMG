using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShaderEvents  {
    public delegate void Saturation ();
    public static event Saturation SaturationChange;

    public static void RemoveColor() {
      if (SaturationChange != null) {
        SaturationChange();
      }
    }
}
