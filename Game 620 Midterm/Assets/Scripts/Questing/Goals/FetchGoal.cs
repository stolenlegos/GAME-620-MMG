using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FetchGoal : Goal {
//goal specific variables not shared by another type of goal.
  ItemID ItemID { get; set; }
  GameObject SpawnLocation { get; set; }


//constructor for a fetch goal. all variables are passed from a newly
//instantiated fetch quest.
    public FetchGoal(Quest quest, ItemID itemID, GameObject spawnLocation, string description, bool completed, int currentAmount, int requiredAmount) {
      this.Quest = quest;
      this.ItemID = itemID;
      this.SpawnLocation = spawnLocation;
      this.Description = description;
      this.Completed = completed;
      this.CurrentAmount = currentAmount;
      this.RequiredAmount = requiredAmount;
    }



//goal specific Initialise function. good for adding functions to delegates.
    public override void Initialise() {
      base.Initialise();
      QuestEvents.ItemPickedUp += ItemFetched;
    }


//function needs to be added to a delegate for dropping an item
//enacted when the player drops off item to the original quest giver.
    private void ItemFetched(ItemID item) {
      if (item == this.ItemID) {
        this.CurrentAmount++;
        Evaluate();
      }
    }
}
