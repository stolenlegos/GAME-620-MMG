using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slime : Enemy {

  public Slime(int health, int speed, int xp) {
    this.health = health;
    this.speed = speed;
    this.xp = xp;
    this.ID = EnemyID.Slimes;
  }


  public override Enemy Clone() {
    return new Slime(health, speed, xp);
  }


  public override void Initialise(Transform spawnLocation, GameObject enemyPrefab) {
    GameObject newSlime = GameObject.Instantiate(enemyPrefab);
    newSlime.transform.position = spawnLocation.position;
    SlimeObject newSlimeData = newSlime.GetComponent<SlimeObject>();
    newSlimeData.enemyData = this;
  }
}
