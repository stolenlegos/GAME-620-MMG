using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeliverGoal : Goal {
  ItemID ItemID { get; set; }
  NpcID NpcID { get; set; }

  public DeliverGoal (Quest quest, ItemID itemID, NpcID npcID, string description, bool completed, int currentAmount, int requiredAmount) {
    this.Quest = quest;
    this.ItemID = itemID;
    this.NpcID = npcID;
    this.Description = description;
    this.Completed = completed;
    this.CurrentAmount = currentAmount;
    this.RequiredAmount = requiredAmount;
  }


  public override void Initialise() {
    base.Initialise();
    //InventoryEvents.OnItemRemoved += ItemDelivered;
  }


  private void ItemDelivered(IItem item) {
    if (item.ID == this.ItemID) {
      this.CurrentAmount++;
      Evaluate();
    }
  }
}
