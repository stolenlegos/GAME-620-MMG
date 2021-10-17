using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestGiver : NPC {
  public QuestGiverID id;
  public bool AssignedQuest { get; set; }

  [SerializeField]
  private GameObject quests;

  public string questType;
  public Quest Quest { get; set; }

  private string[] proposalDialogue;
  private bool droppedLastQuest;
  private bool neverHelped;
  private string inProgressDialogue;
  private string questCompleteDialogue;


//assigns functions to delegates
  private void Start() {
    QuestEvents.QuestAccepted += AssignQuest;
    UIEvents.QuestRemoved += DroppedQuest;
    QuestEvents.QuestRejected += this.QuestRejected;
    droppedLastQuest = false;
    neverHelped = true;
  }


//checks if player has quest from this NPC. Pass relevant non quest dialogue here.
  public override void Interact() {
    if (!AssignedQuest) {
      base.Interact();
      Quest = (Quest)quests.AddComponent(System.Type.GetType(questType));

      //I need it to pause here until Quest does its Start()
      StartCoroutine("DialogueThisShit");

    }
    else if (AssignedQuest) {
      CheckQuest();
    }

    neverHelped = false;
  }


  IEnumerator DialogueThisShit(){
    for (float ft = 1f; ft >=0; ft -= 0.1f){
      yield return new WaitForSeconds(.1f);

      if(questType == "KillQuest"){
        proposalDialogue = CreateProposalDialogueKill();
      }
      else if(questType == "FetchQuest") {
        proposalDialogue = CreateProposalDialogueFetch();
      }
      else if (questType == "DeliverQuest") {
        proposalDialogue = CreateProposalDialogueDeliver();
      }

      QuestEvents.ProposeQuest(this, proposalDialogue);
    }
  }


//pulls up the quest accept UI screen
  private void AssignQuest(QuestGiver questGiver) {
    if (questGiver.id == this.id) {
      AssignedQuest = true;
    }
  }


  private void QuestRejected(QuestGiver questGiver) {
    if (questGiver.id == this.id) {
      Debug.Log("Destroied");
      Destroy(Quest);
      droppedLastQuest = true;
    }
  }


//checks if quest goals have all be completed by checking quest completion bool
//and decides appropriate dialogue options.
  private void CheckQuest() {
    if (Quest.Completed) {
      Quest.GiveReward(Quest);
      AssignedQuest = false;
      droppedLastQuest = false;

      if (questType == "KillQuest"){
        questCompleteDialogue = DialogueManager.KillReturnDialogue;
      }
      else if (questType == "FetchQuest"){
        questCompleteDialogue = DialogueManager.FetchReturnDialogue;
      }
      else if (questType == "DeliverQuest") {
        questCompleteDialogue = DialogueManager.DeliveryReturnDialogue;
      }

      UIEvents.QuestHasFinished(questCompleteDialogue);
    }
    else {
      Debug.Log("Finish your task, please.");
      if (questType == "KillQuest"){
        inProgressDialogue = DialogueManager.KillBeforeCompleteDialogue[Random.Range(0, 4)];
      }
      else if (questType == "FetchQuest"){
        inProgressDialogue = DialogueManager.FetchBeforeCompleteDialogue[Random.Range(0, 4)];
      }
      else if (questType == "DeliverQuest") {
        inProgressDialogue = DialogueManager.DeliveryBeforeCompleteDialogue[Random.Range(0, 4)];
      }

      UIEvents.QuestStillInProgress(inProgressDialogue);
    }
  }


//changes variables if player drops the quest assigned by this person.
//do not pass dialogue here.
  private void DroppedQuest(Quest quest) {
    if (quest == this.Quest) {
      AssignedQuest = false;
      Quest = null;
      droppedLastQuest = true;
      //pass world quicker mechanic here
    }
  }


  private string[] CreateProposalDialogueKill() {
    int idNum = (int)Quest.EnemyID + 1;
    int num;
    string lineTwo;

    if (droppedLastQuest) {
      num = Random.Range(8, 10);
    } else {
      num = Random.Range(6, 8);
    }

    string lineOne = DialogueManager.KillProposalDialogue[0];

    if(!neverHelped){
      lineTwo = DialogueManager.KillProposalDialogue[idNum] + "\n" + DialogueManager.KillProposalDialogue[num];
    }
    else {
      lineTwo = DialogueManager.KillProposalDialogue[idNum];
    }

    string lineThree = DialogueManager.KillDenyDialogue[0];
    string lineFour = DialogueManager.KillDenyDialogue[1];
    string lineFive = DialogueManager.KillDenyDialogue[2];
    string lineSix = DialogueManager.KillDenyDialogue[3];
    string lineSeven = DialogueManager.KillAcceptDialogue;

    string[] dialogue = new string[] {
      lineOne,
      lineTwo,
      lineThree,
      lineFour,
      lineFive,
      lineSix,
      lineSeven
    };

    return dialogue;
  }


  private string[] CreateProposalDialogueFetch() {
    int idNum = (int)Quest.ItemID + 1;
    int num;
    string lineTwo;

    if (droppedLastQuest) {
      num = Random.Range(9, 11);
    } else {
      num = Random.Range(7, 9);
    }

    string lineOne = DialogueManager.FetchProposeDialogue[0];

    if(!neverHelped){
      lineTwo = DialogueManager.FetchProposeDialogue[idNum] + "\n" + DialogueManager.FetchProposeDialogue[num];
    }
    else {
      lineTwo = DialogueManager.FetchProposeDialogue[idNum];
    }

    string lineThree = DialogueManager.FetchDenyDialogue[0];
    string lineFour = DialogueManager.FetchDenyDialogue[1];
    string lineFive = DialogueManager.FetchDenyDialogue[2];
    string lineSix = DialogueManager.FetchDenyDialogue[3];
    string lineSeven = DialogueManager.FetchAcceptDialogue;

    string[] dialogue = new string[] {
      lineOne,
      lineTwo,
      lineThree,
      lineFour,
      lineFive,
      lineSix,
      lineSeven
    };

    return dialogue;
  }


  private string[] CreateProposalDialogueDeliver() {
    int idNum = (int)Quest.ItemID + 1;
    int num;
    string lineTwo;

    if (droppedLastQuest) {
      num = Random.Range(9, 11);
    } else {
      num = Random.Range(7, 9);
    }

    string lineOne = DialogueManager.DeliveryProposalDialogue[0];

    if(!neverHelped){
      lineTwo = DialogueManager.DeliveryProposalDialogue[idNum] + "\n" + DialogueManager.DeliveryProposalDialogue[num];
    }
    else {
      lineTwo = DialogueManager.DeliveryProposalDialogue[idNum];
    }

    string lineThree = DialogueManager.DeliveryDenyDialogue[0];
    string lineFour = DialogueManager.DeliveryDenyDialogue[1];
    string lineFive = DialogueManager.DeliveryDenyDialogue[2];
    string lineSix = DialogueManager.DeliveryDenyDialogue[3];
    string lineSeven = DialogueManager.DeliveryAcceptDialogue;

    string[] dialogue = new string[] {
      lineOne,
      lineTwo,
      lineThree,
      lineFour,
      lineFive,
      lineSix,
      lineSeven
    };

    return dialogue;
  }
}



public enum QuestGiverID {
  Tom,
  Terry,
  Mary,
  Gregg,
  Goeff,
  Mig,
  Sam,
  Ilia,
  Meagan,
  TimTheToolManTaylor,
  HankHill,
  JustinTimberlake,
  BeachBum,
  Craig,
  RichardAyode,
  MattBerry,
  Chad,
  Brenda,
  Monica,
  Brad,
  Susan,
  Joey,
  Wiley,
  Phillip,
  Gene,
  Morgan,
  Alex,
  Bobby,
  Suzy,
  Sarah,
  Chris,
  Dave,
  Hannah,
  AlexanderHamilton,
  USGrant,
  GeorgeWashington,
  MrPresident
}
