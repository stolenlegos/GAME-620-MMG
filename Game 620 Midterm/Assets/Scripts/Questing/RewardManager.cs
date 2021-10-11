using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RewardManager : MonoBehaviour {
  [SerializeField]
  private PlayerCombat playerDamage;

    private void Start() {
      QuestEvents.GrantReward += RewardPlayer;
    }

//gives player reward based on Quest reward variable. needs to be reworked
//based on what rewards the player will be getting
    private void RewardPlayer(Quest quest) {
      playerDamage.damage += 2;
      Debug.Log("Turned in " + quest.QuestName);
      Destroy(quest);
    }
}
