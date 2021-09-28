using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class giveQuest : QuestInterface
{
  public questManagerScript qManager;
  public bool playerNear = false;


    void Start()
    {
      killThis = killNeed();
      nameThis = questName();
    }


    void Update()
    {
      if (Input.GetKeyDown(KeyCode.F) && playerNear) {
        qManager.questList.Add(this);
      }
    }

    void OnTriggerEnter2D(Collider2D other) {
      if (other.tag == "Player") {
        playerNear = true;
      }
    }

    void OnTriggerExit2D(Collider2D other) {
      if(other.tag == "Player") {
        playerNear = false;
      }
    }

    public override int killNeed() {
      int number = Random.Range(1, 10);
      return number;
    }

    public override string questName() {
      string alpha = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
      char c = alpha[Random.Range(0, alpha.Length)];
      string name = "This is quest " + c;
      return name;
    }
}
