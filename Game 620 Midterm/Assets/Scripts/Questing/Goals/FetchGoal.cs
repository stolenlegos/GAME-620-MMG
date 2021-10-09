using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FetchGoal : Goal {
    ItemID ItemID { get; set; }
    GameObject SpawnLocation { get; set; }


    public FetchGoal(Quest quest, ItemID itemID, GameObject spawnLocation, string description, bool completed, int currentAmount, int requiredAmount) {
      this.Quest = quest;
      this.ItemID = itemID;
      this.SpawnLocation = spawnLocation;
      this.Description = description;
      this.Completed = completed;
      this.CurrentAmount = currentAmount;
      this.RequiredAmount = requiredAmount;
    }

    public override void Initialise() {
      base.Initialise();
    }


    private void ItemDelivered(IItem item) {
      if (item.ID == this.ItemID) {
        this.CurrentAmount++;
        Evaluate();
      }
    }
}
