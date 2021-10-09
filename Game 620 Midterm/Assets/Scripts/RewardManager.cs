using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RewardManager : MonoBehaviour {
    private void Start() {
      QuestEvents.GrantReward += RewardPlayer;
    }


    private void RewardPlayer(Quest quest) {
      Debug.Log("Turned in " + quest.QuestName);
      Debug.Log(quest.questReward);
      Destroy(quest);
    }
}
