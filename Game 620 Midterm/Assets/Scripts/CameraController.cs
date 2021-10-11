using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
  private Camera mainCam;
  private Vector3 offset;

  [SerializeField]
  private Transform player;


    private void Start() {
      offset = transform.position - player.position;
      QuestEvents.QuestAccepted += ZoomAndSlow;
      UIEvents.QuestRemoved += ZoomOutandSpeed;

      mainCam = GetComponent<Camera>();

      mainCam.fieldOfView = 60;
    }


    private void LateUpdate() {
      transform.position = player.position + offset;
    }


    private void ZoomAndSlow(QuestGiver questGiver){
      mainCam.fieldOfView -= 2;
      if (Time.timeScale > 0.1f) {
        Time.timeScale -= 0.1f;
      }
    }


    private void ZoomOutandSpeed (Quest quest) {
      if (mainCam.fieldOfView < 60) {
        mainCam.fieldOfView += 2;
      }

      if (Time.timeScale < 1f) {
        Time.timeScale += 0.1f;
      }
    }
}
