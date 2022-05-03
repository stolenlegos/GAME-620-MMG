using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Checkpoints : MonoBehaviour
{
    private SaveManager sm;
    private SoundManager _sm;
    private PlayerController pC;
    public static event Action checkpointActivated;
    private bool firstPass = true;
    private bool dialoguePass = false;
    // Start is called before the first frame update
    void Start()
    {
        sm = GameObject.FindGameObjectWithTag("SM").GetComponent<SaveManager>();
        _sm = GameObject.FindGameObjectWithTag("SoundManager").GetComponent<SoundManager>();
        pC = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            /*if(firstPass == true)
            {
                checkpointActivated.Invoke();
                sm.SavePositions();
                _sm.Play("Checkpoint");
                firstPass = false;
            }
            else if (this.gameObject.name != "Checkpoint (1)")
            {
                checkpointActivated.Invoke();
                //_sm.Play("Checkpoint");
                //sm.SavePositions();
            }*/
        }
    }
    void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (firstPass == true && pC.groundedForDialogue)
            {
                checkpointActivated.Invoke();
                sm.SavePositions();
                _sm.Play("Checkpoint");
                firstPass = false;
            }
            else if (!firstPass && pC.groundedForDialogue)
            {
                checkpointActivated.Invoke();
            }
        }
    }
}
