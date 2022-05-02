using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class DialogueInteract : MonoBehaviour
{
    //intro tutorials
    public static event Action introActivated;
    public static event Action movementActivated;
    //box tutorials
    public static event Action smallBoxActivated;
    public static event Action bigBoxActivated;
    //button tutorials
    public static event Action buttonStartActivated;
    public static event Action buttonEndActivated;
    //Examine tutorial and platform tutorials
    public static event Action examinerStartActivated;
    public static event Action platformTutorialActivated;
    //puzzles
    public static event Action puzzleOneActivated;
    public static event Action puzzleTwoActivated;
    public static event Action puzzleThreeActivated;
    public static event Action puzzleFourActivated;
    public static event Action puzzleFiveActivated;
    //game end
    public static event Action gameEndActivated;

    private bool doNotRepeatMovementIntro = false;
    private bool doNotRepeatPlatformTutorial = false;
    private bool doNotRepeatRestPointTutorial = false;
    private bool doNotRepeatExaminerTutorial = false;
    private DialogueManager _mDialogueManager;
    private PlayerController playerController;
    // Start is called before the first frame update
    void Start()
    {
        _mDialogueManager = GameObject.FindGameObjectWithTag("DialogueManager").GetComponent<DialogueManager>();
        playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(this.gameObject.tag == "GameStart" && collision.gameObject.tag == "Player")
        {
            introActivated.Invoke();
        }
        else if (this.gameObject.tag == "SmallBoxTutorial" && collision.gameObject.tag == "Player")
        {
            smallBoxActivated.Invoke();
        }
        else if(this.gameObject.tag == "BigBoxTutorial" && collision.gameObject.tag == "Player")
        {
            bigBoxActivated.Invoke();
        }
        else if(this.gameObject.tag == "ButtonTutorial" && collision.gameObject.tag == "Player")
        {
            buttonStartActivated.Invoke();
        }
        else if(this.gameObject.tag == "EnergyReturnTutorial" && collision.gameObject.tag == "Player")
        {
            buttonEndActivated.Invoke();
        }
        else if(this.gameObject.tag == "Examiner" && collision.gameObject.tag == "Player")
        {
            examinerStartActivated.Invoke();
        }
        else if (this.gameObject.tag == "PuzzleOne" && collision.gameObject.tag == "Player")
        {
            puzzleOneActivated.Invoke();
        }
        else if (this.gameObject.tag == "PuzzleTwo" && collision.gameObject.tag == "Player")
        {
            puzzleTwoActivated.Invoke();
        }
        else if (this.gameObject.tag == "PuzzleThree" && collision.gameObject.tag == "Player")
        {
            puzzleThreeActivated.Invoke();
        }
        else if (this.gameObject.tag == "PuzzleFour" && collision.gameObject.tag == "Player")
        {
            puzzleFourActivated.Invoke();
        }
        else if (this.gameObject.tag == "PuzzleFive" && collision.gameObject.tag == "Player")
        {
            puzzleFiveActivated.Invoke();
        }
        else if (this.gameObject.tag == "GameEnd" && collision.gameObject.tag == "Player")
        {
            gameEndActivated.Invoke();
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (this.gameObject.tag == "GameStart" && collision.gameObject.tag == "Player" && _mDialogueManager.introTutorialOccured && !doNotRepeatMovementIntro)
        {
            Debug.Log("invoke movement tutorial");
            movementActivated.Invoke();
            doNotRepeatMovementIntro = true;
        }
        else if (this.gameObject.tag == "Examiner" && collision.gameObject.tag == "Player" && !doNotRepeatExaminerTutorial && playerController.groundedForDialogue)
        {
            examinerStartActivated.Invoke();
            doNotRepeatExaminerTutorial = true;
        }
    }
    private void OnMouseEnter()
    {
        if(this.gameObject.name == "Platform" && !doNotRepeatPlatformTutorial && _mDialogueManager.examinerTutorialOccured)
        {
            Debug.Log("InvokePlatform");
            platformTutorialActivated.Invoke();
            doNotRepeatPlatformTutorial = true;
        }
    }
}
