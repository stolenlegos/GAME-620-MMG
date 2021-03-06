using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnController : MonoBehaviour {
  private float timer;

  private Ghost ghostPrototype;
  private SkeleWolf skeleWolfPrototype;
  private Bear bearPrototype;
  private Rat ratPrototype;
  private Slime slimePrototype;

  private Spawner[] enemySpawners;
  
  [SerializeField]
  public int numLivingEnemies;
  public int maxLivingEnemies; 

  [SerializeField]
  private List<Transform> enemySpawnLocations = new List<Transform>();

  [Header("as the Spawners Array in the Script")]
  [Header("The following must be in the same order")]
  [SerializeField]
  private List<GameObject> enemyPrefabs = new List<GameObject>();

  [Header("as the ItemID enum.")]
  [Header("The following must be in the same order")]
  [SerializeField]
  private List<GameObject> itemPrefabs = new List<GameObject>();


  private void Start() {
    timer = 0;
    numLivingEnemies = 0; 

    ghostPrototype = new Ghost(75, 20, 20);
    skeleWolfPrototype = new SkeleWolf(300, 100, 200);
    bearPrototype = new Bear(150, 75, 75);
    ratPrototype = new Rat(10, 30, 10);
    slimePrototype = new Slime(40, 40, 40);

    enemySpawners = new Spawner[] {
      new Spawner(ghostPrototype),
      new Spawner(skeleWolfPrototype),
      new Spawner(bearPrototype),
      new Spawner(ratPrototype),
      new Spawner(slimePrototype)
    };

    QuestEvents.SpawnFetchItem += FetchItemSpawn;
  }


  private void Update() {
    if (timer <= 0) {
      int randomInt = Random.Range(0, enemySpawners.Length);

      Spawner randomSpawner = enemySpawners[randomInt];
      Enemy randomEnemy = randomSpawner.SpawnEnemy();
      GameObject randomPrefab = enemyPrefabs[randomInt];
      Transform randomSpawnLocation = enemySpawnLocations[Random.Range(0, enemySpawnLocations.Count)];
      SpawnObject SpawnBlocked = randomSpawnLocation.GetComponent<SpawnObject>();

      if (!SpawnBlocked.spawnerBlocked && numLivingEnemies < maxLivingEnemies) {
        randomEnemy.Initialise(randomSpawnLocation, randomPrefab);
        numLivingEnemies++;
      }

      timer = 5;
    }
    else {
      timer -= Time.deltaTime;
    }
  }


  private void FetchItemSpawn(FetchQuest quest){
    GameObject itemToSpawn = itemPrefabs[(int)quest.ItemID];

    GameObject item = Instantiate(itemToSpawn);
    item.transform.position = quest.spawnLocation.transform.position;
  }
}
