using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeliverQuest : Quest {

  private void Start() {
    ItemID = RandomiseType();
    QuestName = RandomiseName();
    Description = RandomiseDescription();
    MainQuest = false;
    QuestType = "DeliverQuest";

    Goals.Add(new DeliverGoal(this, ItemID, NpcID.MailBox, Description, false, 0, 1));

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
    GameObject recipient = GameObject.Find("Mail Box");
    DeliveryRecipient recipientBool = recipient.GetComponent<DeliveryRecipient>();
    recipientBool.needsDelivery = true;
  }


  private string RandomiseName() {
    string name = "Deliver " + ItemID.ToString();
    return name;
  }


  private string RandomiseDescription() {
    string des = "You gotta deliver " + ItemID.ToString() + " to the mail post.";
    return des;
  }


  private ItemID RandomiseType() {
    ItemID id = (ItemID)Random.Range(0, 6);
    return id;
  }
}
