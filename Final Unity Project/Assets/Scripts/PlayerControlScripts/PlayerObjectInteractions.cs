using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerObjectInteractions : MonoBehaviour {
  [SerializeField]
  private List<GameObject> _objectsNear = new List<GameObject>();


    void Update() {
        if (Input.GetKeyDown(KeyCode.F) && _objectsNear.Count != 0) {
          EnergyEvents.ChangeColor(_objectsNear[_objectsNear.Count - 1]);
        }

        if (Input.GetMouseButtonDown(0) && _objectsNear.Count != 0) {
          PlayerActions.ObjectGrab(_objectsNear[_objectsNear.Count - 1]);
        }

        if (Input.GetMouseButtonUp(0) &&_objectsNear.Count != 0) {
          PlayerActions.ObjectRelease(_objectsNear[_objectsNear.Count - 1]);
        }
    }

    void OnTriggerEnter2D (Collider2D other) {
      _objectsNear.Add(other.gameObject);
    }

    void OnTriggerExit2D (Collider2D other) {
        PlayerActions.ObjectDropped(_objectsNear[_objectsNear.Count - 1]);
        _objectsNear.Remove(other.gameObject);
    }
}
