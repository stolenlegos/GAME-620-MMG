using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillQuest : Quest {
  private int targetNumber;
  private EnemyID enemyID;


  void Start() {
    enemyID = RandomiseType();
    targetNumber = RandomiseNumber();
    QuestName = RandomiseName();
    Description = RandomiseDescription();

    Goals.Add(new KillGoal(this, enemyID, Description, false, 0, targetNumber));

    Goals.ForEach(g => g.Initialise());
    Debug.Log(QuestName);
    Debug.Log(Description);
  }


  private string RandomiseName() {
    string name = "Kill " + targetNumber.ToString() + " " + enemyID.ToString();
    return name;
  }


  private string RandomiseDescription() {
    string des = "You gotta kill " + targetNumber.ToString() + " " + enemyID.ToString();
    return des;
  }


  private int RandomiseNumber() {
    int number = Random.Range(1, 10);
    return number;
  }


  private EnemyID RandomiseType() {
    EnemyID id = (EnemyID)Random.Range(0, 3);
    return id;
  }
}
