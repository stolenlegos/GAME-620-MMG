using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainQuest : MonoBehaviour {
  private int KillQuestCompelted;
  private int FetchQuestCompleted;
  private int DeliveryQuestsCompleted;

  private int stage;
  private bool stageAssigned;
  private bool playerIsNear;


    private void Start() {
      QuestEvents.GrantReward += UpdateQuestCompleted;
      KillQuestCompelted = 0;
      FetchQuestCompleted = 0;
      DeliveryQuestsCompleted = 0;
      stage = 1;
      playerIsNear = false;
      stageAssigned = false;
    }


    private void Update() {
      if (playerIsNear && Input.GetKeyDown(KeyCode.F)) {
        Interact();
      }
    }


    private void Interact() {
      if (stage == 1) {
        StageOneDialogue();
      }
      else if (stage == 2) {
        StageTwoDialogue();
      }
      else if (stage == 3) {
        StageThreeDialogue();
      }
      else if (stage == 4) {
        StageFourDialogue();
      }
      else if (stage == 5) {
        StageFiveDialogue();
      }
    }

    private void ResetVariables() {
      stageAssigned = false;
      KillQuestCompelted = 0;
      FetchQuestCompleted = 0;
      DeliveryQuestsCompleted = 0;
    }


    private void UpdateQuestCompleted(Quest quest) {
      if (quest.QuestType == "KillQuest") {
        KillQuestCompelted++;
      }
      else if (quest.QuestType == "FetchQuest") {
        FetchQuestCompleted++;
      }
      else if (quest.QuestType == "DeliverQuest") {
        DeliveryQuestsCompleted++;
      }
    }


    private void OnTriggerEnter2D(Collider2D other) {
      if (other.tag == "Player") {
        playerIsNear = true;
      }
    }


    private void OnTriggerExit2D(Collider2D other) {
      if (other.tag == "Player") {
        playerIsNear = false;
      }
    }


    private void StageOneDialogue() {
      if(!stageAssigned) {
        UIEvents.PassMQDialogue(MQDialogueRepos.MQ1Dialogue, 12);
        stageAssigned = true;
      }
      else if (stageAssigned) {
        if (EvaluateStageOne()) {
          UIEvents.PassMQDialogue(MQDialogueRepos.MQ1Finished, 2);
          stage = 2;
          ResetVariables();
        }
        else {
          UIEvents.PassMQDialogue(MQDialogueRepos.MQ1InProgress, 2);
        }
      }
    }


    private bool EvaluateStageOne() {
      if (KillQuestCompelted == 3) {
        return true;
      }
      else {
        return false;
      }
    }


    private void StageTwoDialogue() {
      if(!stageAssigned) {
        UIEvents.PassMQDialogue(MQDialogueRepos.MQ2Dialogue, 7);
        stageAssigned = true;
      }
      else if (stageAssigned) {
        if (EvaluateStageTwo()) {
          UIEvents.PassMQDialogue(MQDialogueRepos.MQ2Finished, 2);
          stage = 3;
          ResetVariables();
        }
        else {
          UIEvents.PassMQDialogue(MQDialogueRepos.MQ2InProgress, 2);
        }
      }
    }


    private bool EvaluateStageTwo() {
      if (DeliveryQuestsCompleted == 3) {
        return true;
      }
      else {
        return false;
      }
    }


    private void StageThreeDialogue() {
      if(!stageAssigned) {
        UIEvents.PassMQDialogue(MQDialogueRepos.MQ3Dialogue, 6);
        stageAssigned = true;
      }
      else if (stageAssigned) {
        if (EvaluateStageThree()) {
          UIEvents.PassMQDialogue(MQDialogueRepos.MQ3Finished, 2);
          stage = 4;
          ResetVariables();
        }
        else {
          UIEvents.PassMQDialogue(MQDialogueRepos.MQ3InProgress, 2);
        }
      }
    }


    private bool EvaluateStageThree() {
      if (FetchQuestCompleted == 3) {
        return true;
      }
      else {
        return false;
      }
    }


    private void StageFourDialogue() {
      if(!stageAssigned) {
        UIEvents.PassMQDialogue(MQDialogueRepos.MQ4Dialogue, 5);
        stageAssigned = true;
      }
      else if (stageAssigned) {
        if (EvaluateStageFour()) {
          UIEvents.PassMQDialogue(MQDialogueRepos.MQ4Finished, 2);
          stage = 5;
          ResetVariables();
        }
        else {
          UIEvents.PassMQDialogue(MQDialogueRepos.MQ4InProgress, 1);
        }
      }
    }


    private bool EvaluateStageFour() {
      if (KillQuestCompelted == 2 && FetchQuestCompleted == 2 && DeliveryQuestsCompleted == 2) {
        return true;
      }
      else {
        return false;
      }
    }


    private void StageFiveDialogue() {
      if(!stageAssigned) {
        UIEvents.PassMQDialogue(MQDialogueRepos.MQ5Dialogue, 11);
        stageAssigned = true;
      }
      else if (stageAssigned) {
        if (EvaluateStageFive()) {
          UIEvents.PassMQDialogue(MQDialogueRepos.MQ5Finished, 6);
          stage = 6;
          ResetVariables();
        }
        else {
          UIEvents.PassMQDialogue(MQDialogueRepos.MQ5InProgress, 1);
        }
      }
    }


    private bool EvaluateStageFive() {
      if (KillQuestCompelted == 1 && FetchQuestCompleted == 1 && DeliveryQuestsCompleted == 1) {
        return true;
      }
      else {
        return false;
      }
    }
}
