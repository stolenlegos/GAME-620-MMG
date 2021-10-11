using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestList : MonoBehaviour {
  private List<Quest> questList = new List<Quest>();
  [SerializeField]
  private GameObject questListObj;
  [SerializeField]
  private Text questListText;
  private bool logOpen;


  private void Start(){
    UIEvents.QuestListAdd += AddQuest;
    QuestEvents.GrantReward += QuestCompleted;
    logOpen = false;
    questListObj.SetActive(false);
  }


  private void Update() {
    if (logOpen) {
      CloseLog();
    }
    else if (!logOpen){
      OpenLog();
    }

    questListText.text = ListToText(questList);
  }


  private void AddQuest(Quest quest) {
    questList.Add(quest);
  }


  public void RemoveQuest(ButtonID buttonID) {
    Quest removedQuest = questList[buttonID.id];
    UIEvents.QuestListRemove(removedQuest);
    questList.RemoveAt(buttonID.id);
  }


  private void QuestCompleted(Quest quest){
    questList.Remove(quest);
  }


  private void OpenLog() {
    if(Input.GetKeyDown(KeyCode.E)){
      questListObj.SetActive(true);
      logOpen = true;
    }
  }


  private void CloseLog() {
    if(Input.GetKeyDown(KeyCode.E)) {
      questListObj.SetActive(false);
      logOpen = false;
    }
  }


  private string ListToText(List<Quest> list) {
    string result = "";
    foreach(var listMember in list) {
      result += listMember.QuestName + "\n";
    }
    return result;
  }
}
