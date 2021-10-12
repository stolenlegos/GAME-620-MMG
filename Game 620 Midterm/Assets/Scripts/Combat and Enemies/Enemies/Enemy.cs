using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy {
  public int health;
  public int speed;
  public int xp;
  public EnemyID ID;


  public abstract Enemy Clone();


  public abstract void Initialise(Transform spawnLocation, GameObject enemyPrefab);
}


public enum EnemyID {
  Rats,
  Slimes,
  Bears,
  Ghosts,
  Dragons
}
