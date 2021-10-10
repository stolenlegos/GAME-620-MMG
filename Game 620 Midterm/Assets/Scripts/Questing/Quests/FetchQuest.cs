using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FetchQuest : Quest {
    private ItemID itemID;
    private GameObject spawnLocation;


    private void Start() {
      itemID = RandomiseType();
      spawnLocation = RandomiseSpawn();
      QuestName = RandomiseName();
      Description = RandomiseDescription();

      Goals.Add(new FetchGoal(this, itemID, spawnLocation, Description, false, 0, 1));

      Goals.ForEach(g => g.Initialise());
      AddToList(this);

      UIEvents.QuestRemoved += DeleteQuest;
    }


    private void DeleteQuest(Quest quest) {
      if (quest == this) {
        Destroy(this);
      }
    }


    private string RandomiseName() {
      string name = "";
      return name;
    }


    private string RandomiseDescription() {
      string des = "";
      return des;
    }


    private GameObject RandomiseSpawn() {
      GameObject[] spawnList = GameObject.FindGameObjectsWithTag("FetchSpawnLocation");
      GameObject spawn = spawnList[Random.Range(0,spawnList.Length + 1)];
      return spawn;
    }


    private ItemID RandomiseType() {
      ItemID id = (ItemID)Random.Range(0,3);
      return id;
    }
}
