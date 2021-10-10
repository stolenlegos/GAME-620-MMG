using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeliverGoal : Goal {
  //goal specific variables not shared by another type of goal.
  ItemID ItemID { get; set; }
  NpcID NpcID { get; set; }


//constructor for a delivery goal. all variables are passed from a newly
//instantiated delivery quest.
  public DeliverGoal (Quest quest, ItemID itemID, NpcID npcID, string description, bool completed, int currentAmount, int requiredAmount) {
    this.Quest = quest;
    this.ItemID = itemID;
    this.NpcID = npcID;
    this.Description = description;
    this.Completed = completed;
    this.CurrentAmount = currentAmount;
    this.RequiredAmount = requiredAmount;
  }


//goal specific Initialise function. good for adding functions to delegates.
  public override void Initialise() {
    base.Initialise();
    //InventoryEvents.OnItemRemoved += ItemDelivered;
  }


//function needs to be added to a delegate for dropping an item
//enacted when the player drops off item to third party
  private void ItemDelivered(IItem item) {
    if (item.ID == this.ItemID) {
      this.CurrentAmount++;
      Evaluate();
    }
  }
}
