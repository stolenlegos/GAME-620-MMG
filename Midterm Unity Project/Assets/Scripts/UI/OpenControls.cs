using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenControls : MonoBehaviour
{
  [SerializeField]
  private GameObject controlsUI;
  private bool controlsOpen;


    void Start() {
      controlsOpen = false;
    }

    // Update is called once per frame
    void Update()
    {
      if (Input.GetKeyDown(KeyCode.Tab)) {
        controlsOpen = !controlsOpen;
      }

      Controls();
    }


    private void Controls() {
      if (controlsOpen){
        controlsUI.SetActive(true);
      }
      else {
        controlsUI.SetActive(false);
      }
    }
}
