using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSatControl : MonoBehaviour {
  [SerializeField]
  private List<GameObject> _objectsNear = new List<GameObject>();

//hello
    void Update() {
        if (Input.GetKeyDown(KeyCode.F) && _objectsNear.Count != 0) {
          ShaderEvents.RemoveColor(_objectsNear[_objectsNear.Count - 1]);
        }
    }

    void OnTriggerEnter2D (Collider2D other) {
      _objectsNear.Add(other.gameObject);
    }

    void OnTriggerExit2D (Collider2D other) {
      _objectsNear.Remove(other.gameObject);
      }
}
