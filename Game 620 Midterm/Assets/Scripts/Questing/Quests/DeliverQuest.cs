using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeliverQuest : Quest {
  private ItemID itemID;
  private NpcID npcID;


  private void Start() {
    itemID = RandomiseType();
    npcID = RandomiseNPC();
    QuestName = RandomiseName();
    Description = RandomiseDescription();
    MainQuest = false;

    Goals.Add(new DeliverGoal(this, itemID, npcID, Description, false, 0, 1));

    Goals.ForEach(g => g.Initialise());
    AddToList(this);

    UIEvents.QuestRemoved += DeleteQuest;
    QuestEvents.CompleteDelivery += CheckGoals;

    PrepareRecipient();
  }


  private void DeleteQuest(Quest quest){
    if (quest == this) {
      Destroy(this);
    }
  }


  private void CheckGoals (DeliveryRecipient recipient) {
    foreach (DeliverGoal goal in Goals) {
      goal.ItemDelivered(recipient);
    }
  }


  private void PrepareRecipient() {
    GameObject recipient = GameObject.Find(npcID.ToString());
    Debug.Log(recipient);
    DeliveryRecipient recipientBool = recipient.GetComponent<DeliveryRecipient>();
    recipientBool.needsDelivery = true;
  }


  private string RandomiseName() {
    string name = "Deliver " + itemID.ToString();
    return name;
  }


  private string RandomiseDescription() {
    string des = "You gotta deliver " + itemID.ToString() + " to " + npcID.ToString() + ".";
    return des;
  }


  private NpcID RandomiseNPC() {
    NpcID npc = (NpcID)Random.Range(1, 4);
    return npc;
  }


  private ItemID RandomiseType() {
    ItemID id = (ItemID)Random.Range(0, 3);
    return id;
  }
}
