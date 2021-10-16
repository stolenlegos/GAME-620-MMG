using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeleWolf : Enemy {


  public SkeleWolf(int health, int speed, int xp) {
    this.health = health;
    this.speed = speed;
    this.xp = xp;
    this.ID = EnemyID.SkeleWolf;
  }


  public override Enemy Clone() {
    return new SkeleWolf(health, speed, xp);
  }


  public override void Initialise(Transform spawnLocation, GameObject enemyPrefab) {
    GameObject newSkeleWolf = GameObject.Instantiate(enemyPrefab);
    newSkeleWolf.transform.position = spawnLocation.position;
    SkeleWolfObject newSkeleWolfData = newSkeleWolf.GetComponent<SkeleWolfObject>();
    newSkeleWolfData.enemyData = this;
  }
}
