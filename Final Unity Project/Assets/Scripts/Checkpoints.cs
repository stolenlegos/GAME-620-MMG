using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Checkpoints : MonoBehaviour
{
    private SaveManager sm;
    public static event Action checkpointActivated;
    private bool firstPass = true;
    // Start is called before the first frame update
    void Start()
    {
        sm = GameObject.FindGameObjectWithTag("SM").GetComponent<SaveManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if(firstPass == true)
            {
                checkpointActivated.Invoke();
                sm.SavePositions();
                firstPass = false;
            }
            else
            {
                checkpointActivated?.Invoke();
                sm.SavePositions();
            }
        }
    }
}
