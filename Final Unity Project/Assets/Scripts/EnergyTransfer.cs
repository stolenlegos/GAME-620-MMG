using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyTransfer : MonoBehaviour
{
    public bool returning;
    public bool giving;
    private float speed;
    private Vector3 playerPosition;
    
    // Start is called before the first frame update
    void Start()
    {
        playerPosition = GameObject.FindGameObjectWithTag("Player").transform.position;
        speed = 6.5f;
    }

    // Update is called once per frame
    void Update()
    {
        playerPosition = GameObject.FindGameObjectWithTag("Player").transform.position;
        if (returning){
            transform.position = Vector3.MoveTowards(this.transform.position, playerPosition, speed * Time.deltaTime);
            if(transform.position == playerPosition){
                Destroy(gameObject);
            }
        }
        else if (giving){
            if(EnergyEvents.objectsColored.Count >= 1){
                transform.position = Vector3.MoveTowards(this.transform.position, EnergyEvents.objectsColored[EnergyEvents.objectsColored.Count - 1].transform.position, speed * Time.deltaTime);
                if (transform.position == EnergyEvents.objectsColored[EnergyEvents.objectsColored.Count - 1].transform.position)
                {
                    Destroy(gameObject);
                }
            }
            else if(EnergyEvents.objectsColored.Count <= 0)
            {
                Destroy(gameObject);
            }
        }
    }
}
