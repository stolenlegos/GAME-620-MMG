using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeliveryRecipient : NPC {
  public NpcID id;
  public bool needsDelivery;


  private void Start() {
    needsDelivery = false;
  }


  public override void Interact() {
    base.Interact();
    if (needsDelivery) {
      QuestEvents.DeliveryGoalCompleted(this);
      needsDelivery = false;
    }
  }
}
