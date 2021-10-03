using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class giveQuest
{
  public int killThis;
  public string nameThis;

    public void doThis() {
      killThis = killNeed();
      nameThis = questName();
    }


    private int killNeed() {
      int number = Random.Range(1, 10);
      return number;
    }


    private string questName() {
      string alpha = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
      char c = alpha[Random.Range(0, alpha.Length)];
      string name = "This is quest " + c;
      return name;
    }
}
