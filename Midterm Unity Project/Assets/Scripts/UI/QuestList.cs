using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestList : MonoBehaviour {
  private List<Quest> questList = new List<Quest>();
  [SerializeField]
  private List<GameObject> buttonList = new List<GameObject>();
  [SerializeField]
  private GameObject questListObj;
  [SerializeField]
  private Text questListText;
  private bool logOpen;


  private void Start(){
    UIEvents.QuestListAdd += AddQuest;
    QuestEvents.GrantReward += QuestCompleted;
    QuestEvents.QuestRejected += this.QuestRejected;
    logOpen = false;
    questListObj.SetActive(false);
    FillButtonList();
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
    ShowButtons(questList);
  }


  public void RemoveQuest(ButtonID buttonID) {
    Quest removedQuest = questList[buttonID.id];
    UIEvents.QuestListRemove(removedQuest);
    questList.RemoveAt(buttonID.id);
    ShowButtons(questList);
  }


  private void QuestRejected(QuestGiver questGiver){
    questList.Remove(questGiver.Quest);
  }


  private void QuestCompleted(Quest quest){
    questList.Remove(quest);
    ShowButtons(questList);
  }


  private void OpenLog() {
    if(Input.GetKeyDown(KeyCode.E)){
      questListObj.SetActive(true);
      logOpen = true;
      ShowButtons(questList);
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

  private void ShowButtons(List<Quest> list) {
      foreach (Quest q in list){
        int temp = list.IndexOf(q);
        GameObject tempButton = buttonList[temp];
        if (buttonList[temp] != null){
          buttonList[temp].SetActive(true);
        }
      }

      for (int j = list.Count; j < buttonList.Count; j++){
        buttonList[j].SetActive(false);   
      }  
  }

  private void FillButtonList(){
    GameObject tempParent = questListObj.transform.Find("DropButtons").gameObject;
    //i is number of buttons
    for (int i = 0; i <8; i++){
       GameObject tempButton = tempParent.transform.Find("Button"+i).gameObject;
       buttonList.Add(tempButton);
    }
  }
}
