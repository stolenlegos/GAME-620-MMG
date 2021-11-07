using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ghost : Enemy {


  public Ghost(int health, int speed, int xp) {
    this.health = health;
    this.speed = speed;
    this.xp = xp;
    this.ID = EnemyID.Ghosts;
  }


  public override Enemy Clone() {
    return new Ghost(health, speed, xp);
  }


  public override void Initialise(Transform spawnLocation, GameObject enemyPrefab) {
    GameObject newGhost = GameObject.Instantiate(enemyPrefab);
    newGhost.transform.position = spawnLocation.position;
    GhostObject newGhostData = newGhost.GetComponent<GhostObject>();
    newGhostData.enemyData = this;
  }
}
