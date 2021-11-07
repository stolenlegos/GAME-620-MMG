using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rat : Enemy {

  public Rat(int health, int speed, int xp) {
    this.health = health;
    this.speed = speed;
    this.xp = xp;
    this.ID = EnemyID.Rats;
  }


  public override Enemy Clone() {
    return new Rat(health, speed, xp);
  }


  public override void Initialise(Transform spawnLocation, GameObject enemyPrefab) {
    GameObject newRat = GameObject.Instantiate(enemyPrefab);
    newRat.transform.position = spawnLocation.position;
    RatObject newRatData = newRat.GetComponent<RatObject>();
    newRatData.enemyData = this;
  }
}
