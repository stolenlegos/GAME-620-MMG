using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bear : Enemy {


  public Bear(int health, int speed, int xp) {
    this.health = health;
    this.speed = speed;
    this.xp = xp;
    this.ID = EnemyID.Bears;
  }


  public override Enemy Clone() {
    return new Bear(health, speed, xp);
  }


  public override void Initialise(Transform spawnLocation, GameObject enemyPrefab) {
    GameObject newBear = GameObject.Instantiate(enemyPrefab);
    newBear.transform.position = spawnLocation.position;
    BearObject newBearData = newBear.GetComponent<BearObject>();
    newBearData.enemyData = this;
  }
}
