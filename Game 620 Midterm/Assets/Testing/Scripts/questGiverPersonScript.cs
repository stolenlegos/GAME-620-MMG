using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class questGiverPersonScript : MonoBehaviour
{
  private bool playerNear;
  public GameObject questAcceptUI;
    // Start is called before the first frame update
    void Start()
    {
      playerNear = false;
    }

    // Update is called once per frame
    void Update()
    {
      if (Input.GetKeyDown(KeyCode.F) && playerNear) {
        questAcceptUI.SetActive(true);
      }
    }

    void OnTriggerEnter2D (Collider2D other) {
      if (other.tag == "Player") {
        playerNear = true;
      }
    }

    void OnTriggerExit2D (Collider2D other) {
      if (other.tag == "Player") {
        playerNear = false;
      }
    }
}
