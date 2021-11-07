using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner {
  private Enemy prototype;


  public Spawner (Enemy prototype) {
    this.prototype = prototype;
  }


  public Enemy SpawnEnemy() {
    return prototype.Clone();
  }
}
