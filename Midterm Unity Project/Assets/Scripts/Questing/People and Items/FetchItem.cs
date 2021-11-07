using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FetchItem : MonoBehaviour {
  [SerializeField]
  private ItemID id;


  private void OnTriggerEnter2D (Collider2D other) {
    if (other.tag == "Player"){
      QuestEvents.PickUpItem(id);
      Destroy(gameObject);
    }
  }
}
