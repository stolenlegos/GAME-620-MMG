using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dragon : Enemy {


  public Dragon(int health, int speed, int xp) {
    this.health = health;
    this.speed = speed;
    this.xp = xp;
    this.ID = EnemyID.Dragons;
  }


  public override Enemy Clone() {
    return new Dragon(health, speed, xp);
  }


  public override void Initialise(Transform spawnLocation, GameObject enemyPrefab) {
    GameObject newDragon = GameObject.Instantiate(enemyPrefab);
    newDragon.transform.position = spawnLocation.position;
    DragonObject newDragonData = newDragon.GetComponent<DragonObject>();
    newDragonData.enemyData = this;
  }
}
