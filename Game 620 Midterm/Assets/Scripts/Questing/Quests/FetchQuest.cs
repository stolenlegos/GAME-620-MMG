using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FetchQuest : Quest {
    public ItemID itemID;
    public GameObject spawnLocation;


    private void Start() {
      itemID = RandomiseType();
      spawnLocation = RandomiseSpawn();
      QuestName = RandomiseName();
      Description = RandomiseDescription();
      MainQuest = false;
      QuestType = "FetchQuest";

      Goals.Add(new FetchGoal(this, itemID, spawnLocation, Description, false, 0, 1));

      Goals.ForEach(g => g.Initialise());
      AddToList(this);

      UIEvents.QuestRemoved += DeleteQuest;

      QuestEvents.SpawnItem(this);
      Debug.Log(Description);
    }


    private void DeleteQuest(Quest quest) {
      if (quest == this) {
        Destroy(this);
      }
    }


    private string RandomiseName() {
      string name = "Fetch " + itemID.ToString();
      return name;
    }


    private string RandomiseDescription() {
      string des = "Fetch " + itemID.ToString() + " from " + spawnLocation.name + ".";
      return des;
    }


    private GameObject RandomiseSpawn() {
      GameObject[] spawnList = GameObject.FindGameObjectsWithTag("FetchSpawnLocation");
      GameObject spawn = spawnList[Random.Range(0, spawnList.Length)];
      return spawn;
    }


    private ItemID RandomiseType() {
      ItemID id = (ItemID)Random.Range(0,3);
      return id;
    }
}
