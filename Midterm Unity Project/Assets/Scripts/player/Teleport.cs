using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleport : MonoBehaviour {
    [SerializeField]
    private Transform playerPos;
    [SerializeField]
    private Transform destination;

    private void OnTriggerEnter2D (Collider2D other) {
      if (other.tag == "Player" ) {
        playerPos.position = destination.position;  
      }

    }
}
