using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSatControl : MonoBehaviour {
    void Update() {
        if (Input.GetKeyDown(KeyCode.F)) {
          ShaderEvents.RemoveColor();
        }
    }
}
