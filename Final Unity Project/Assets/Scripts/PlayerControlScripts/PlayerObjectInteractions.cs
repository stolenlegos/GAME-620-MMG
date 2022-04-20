using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerObjectInteractions : MonoBehaviour {
  [SerializeField]
  public List<GameObject> _objectsNear = new List<GameObject>();
    public List<GameObject> _objectsToColor = new List<GameObject>();
    private PlayerController playerController;

    private void Awake()
    {
        playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
    }

    void Update() {
        if (Input.GetMouseButtonDown(0) && _objectsToColor.Count != 0 /*&& !Input.GetMouseButton(0)*/) {
          EnergyEvents.ChangeColor(_objectsToColor[_objectsToColor.Count - 1]);
        }

        if (Input.GetKeyDown(KeyCode.W) && _objectsNear.Count != 0 && playerController.mPlayerState != CharacterState.JUMPING) {
          PlayerActions.ObjectGrab(_objectsNear[_objectsNear.Count - 1]);
        }

        if (Input.GetKeyUp(KeyCode.W) &&_objectsNear.Count != 0) {
            int i = 0;
            while (i < _objectsNear.Count)
            {
                PlayerActions.ObjectRelease(_objectsNear[i]);
                i += 1;
            }
        }
    }

    void OnTriggerEnter2D (Collider2D other) {
        if (other.tag != "Spiral" || other.tag != "Examiner") {
            _objectsNear.Add(other.gameObject);
        }
    }

    void OnTriggerExit2D(Collider2D other) {
        if (other.tag == "box_Big" || other.tag == "box_Small") { 
        PlayerActions.ObjectDropped(other.gameObject);
    }
        _objectsNear.Remove(other.gameObject);

    }

}
