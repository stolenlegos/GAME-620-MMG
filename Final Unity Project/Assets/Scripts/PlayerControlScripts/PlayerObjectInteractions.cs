using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerObjectInteractions : MonoBehaviour {
  [SerializeField]
  private List<GameObject> _objectsNear = new List<GameObject>();
    private PlayerController playerController;

    private void Awake()
    {
        playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.F) && _objectsNear.Count != 0 && !Input.GetMouseButton(0)) {
          EnergyEvents.ChangeColor(_objectsNear[_objectsNear.Count - 1]);
        }

        if (Input.GetMouseButtonDown(0) && _objectsNear.Count != 0 && playerController.mPlayerState != CharacterState.JUMPING) {
          PlayerActions.ObjectGrab(_objectsNear[_objectsNear.Count - 1]);
        }

        if (Input.GetMouseButtonUp(0) &&_objectsNear.Count != 0) {
            int i = 0;
            while (i < _objectsNear.Count)
            {
                PlayerActions.ObjectRelease(_objectsNear[i]);
                i += 1;
            }
        }
    }

    void OnTriggerEnter2D (Collider2D other) {
      _objectsNear.Add(other.gameObject);
    }

    void OnTriggerExit2D (Collider2D other) {
        //Debug.Log("Dropped");
        PlayerActions.ObjectDropped(_objectsNear[_objectsNear.Count - 1]);
        _objectsNear.Remove(other.gameObject);
    }

}
