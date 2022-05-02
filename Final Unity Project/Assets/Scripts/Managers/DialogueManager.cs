using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using Random = UnityEngine.Random;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class DialogueManager : MonoBehaviour
{
    //Dialogue framework
    public Text nameText;
    public Text dialogueText;
    public Image textEnder;
    public static DialogueManager instance;
    private Coroutine typeWriterCoroutine;
    private bool typing = false;
    private float starTime = 0f;
    private float endTime = 1f;

    private Dialogue currentDialogue;
    //Referenced Managers and other Classes
    public Animator animator;
    public SoundManager soundManager;
    private PlayerController playerController;
    private ChangeScene changeScene;

    //Sentence and bark framework
    private Queue<string> sentences;
    private string currentSentence;
    private string barkString;
    private Scene scene;

    //Referenced Actions
    public static event Action careOn;

    //Cursoe activation bool
    public bool careOff = true;

    //prevent repeats

    //conversation and bark bools
    public bool introTutorialOccured = false; //1
    private bool introMovementTutorialOccured = false; //2
    //movement tutorial bools
    //private bool introMovementMoveOccured = false; //3
    //private bool introMovementJumpOccured = false; //4
    // object bools
    private bool smallBoxTutorial1Occured = false; //5
    public bool smallBoxTutorial2Occured = false; //6
    private bool smallBoxTutorial3Occured = false;//7
    private bool smallBoxTutorial4Occured = false; 
    private bool bigBoxTutorial1Occured = false; //8
    private bool bigBoxTutorial2Occured = false; //9
    public bool buttonTutorial1Occured = false; //10
    private bool buttonTutorial2Occured = false; //11
    private bool buttonTutorial3Occured = false; //12
    private bool restPointTutorialOccured = false; //13
    public bool examinerTutorialOccured = false; //14
    private bool platformTutorialOccured = false; //15
    //puzzle bools
    private bool puzzle1 = false; //16
    private bool puzzle2 = false; //17
    private bool puzzle3 = false; //18
    private bool puzzle4 = false; //19
    private bool puzzle5 = false; //20
    private bool gameEnd = false; //21
    //Rest Point bools
    private bool checkpointBarkFirst = true;
    private bool returnedToRestPoint = false;


    // Start is called before the first frame update

    private void Awake(){
        if (instance == null){
            instance = this;
            DontDestroyOnLoad(instance);
        }
        else{
            Destroy(gameObject);
        }
    }

    private void OnEnable(){
        Checkpoints.checkpointActivated += CheckpointBark;
        //dialogue interact actions
        DialogueInteract.introActivated += Intro;
        DialogueInteract.movementActivated += IntroMovementTutorial;
        DialogueInteract.smallBoxActivated += SmallBoxTutorialOne;
        DialogueInteract.bigBoxActivated += BigBoxTutorialOne;
        DialogueInteract.buttonStartActivated += ButtonTutorialOne;
        DialogueInteract.buttonEndActivated += ButtonTutorialThree;
        DialogueInteract.examinerStartActivated += ExaminerTutorial;
        DialogueInteract.puzzleOneActivated += PuzzleOne;
        DialogueInteract.puzzleTwoActivated += PuzzleTwo;
        DialogueInteract.puzzleThreeActivated += PuzzleThree;
        DialogueInteract.puzzleFourActivated += PuzzleFour;
        DialogueInteract.puzzleFiveActivated += PuzzleFive;
        DialogueInteract.gameEndActivated += GameEnd;
        DialogueInteract.platformTutorialActivated += PlatformTutorial;
        // saturation control actions
        SaturationControl.smallBoxTutorialActivated += SmallBoxTutorialTwo;
        SaturationControl.smallBoxTutorialClickActivated += SmallBoxTutorialThree;
        // Allow movement actions
        AllowMovement.smallBoxGrabActivated += SmallBoxTutorialFour;
        AllowMovement.bigBoxGrabActivated += BigBoxTutorialTwo;
        // Button Push actions
        AllowButtonPush.buttonTutorialPush += ButtonTutorialTwo;
    }

    void Start(){
        sentences = new Queue<string>();
        scene = SceneManager.GetActiveScene();
        soundManager = GameObject.FindGameObjectWithTag("SoundManager").GetComponent<SoundManager>();
        playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        changeScene = GameObject.FindGameObjectWithTag("SceneChange").GetComponent<ChangeScene>();
}
    private void Update(){
        if(scene.name != "MainGame")
        {
            Destroy(gameObject);
        }
        if (sentences.Count > 0)
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (!typing)
                {
                    DisplayNextSentence(currentDialogue);
                }
                else
                {
                    if (typeWriterCoroutine != null)
                    {
                        StopCoroutine(typeWriterCoroutine);
                    }
                    typing = false;
                    dialogueText.text = currentSentence;
                }
            }
        }
        else if (animator != null)
        {
            if (sentences.Count == 0 && animator.GetBool("IsOpen") == true)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    if (!typing)
                    {
                        DisplayNextSentence(currentDialogue);
                    }
                    else
                    {
                        if (typeWriterCoroutine != null)
                        {
                            StopCoroutine(typeWriterCoroutine);
                        }
                        typing = false;
                        dialogueText.text = currentSentence;
                    }
                }
            }
        }
        if (typing){
            textEnder.enabled = false;
        }
        if (!typing){
            soundManager.Stop("CareTalk");
            if (starTime <= 0){
                textEnder.enabled = true;
                starTime += Time.deltaTime;
            }
            else if (starTime >= endTime){
                textEnder.enabled = false;
                starTime -= Time.deltaTime;
            }
        }
        if (Input.GetKeyDown(KeyCode.R)){
            returnedToRestPoint = true;
        }
    }

    public void StartDialogue (Dialogue dialogue){
        if (animator != null)
        {
            animator.SetBool("IsOpen", true);
        }
        nameText.text = dialogue.name;
        currentDialogue = dialogue;
        sentences.Clear();

            foreach (string sentence in dialogue.sentences){
                sentences.Enqueue(sentence);
            }

        DisplayNextSentence(currentDialogue);
    }

    public void DisplayNextSentence(Dialogue currentDialogue){
        if (sentences.Count == 0){
            EndDialogue(currentDialogue);
            return;
        }

        string sentence = sentences.Dequeue();
        dialogueText.text = sentence;
        if (typeWriterCoroutine != null){
            StopCoroutine(typeWriterCoroutine);
        }
        typeWriterCoroutine = StartCoroutine(TypeSentence(sentence));
    }

    IEnumerator TypeSentence (string sentence){
        currentSentence = sentence;
        typing = true;
        if (typing){
            soundManager.Play("CareTalk");
        }
        dialogueText.text = "";
        foreach(char letter in sentence.ToCharArray()){
            dialogueText.text += letter;
            yield return new WaitForSeconds(0.1f);
        }
        typing = false;
    }

    void EndDialogue(Dialogue currentDialogue){
        if (careOff){
            careOn?.Invoke();
            careOff = false;
        }
        if(currentDialogue.dialogueTag == "Intro"){
            introTutorialOccured = true;
            playerController._bMovementDisabled = false;
        }
        else if (currentDialogue.dialogueTag == "IntroMovement"){
            introMovementTutorialOccured = true;
        }
        else if (currentDialogue.dialogueTag == "SmallBoxTutorial1"){
            smallBoxTutorial1Occured = true;
        }
        else if (currentDialogue.dialogueTag == "SmallBoxTutorial2"){
            smallBoxTutorial2Occured = true;
        }
        else if (currentDialogue.dialogueTag == "SmallBoxTutorial3"){
            smallBoxTutorial3Occured = true;
            playerController._bMovementDisabled = false;
        }
        else if (currentDialogue.dialogueTag == "SmallBoxTutorial4"){
            smallBoxTutorial4Occured = true;
        }
        else if (currentDialogue.dialogueTag == "BigBoxTutorial1"){
            bigBoxTutorial1Occured = true;
            playerController._bMovementDisabled = false;
        }
        else if (currentDialogue.dialogueTag == "BigBoxTutorial2"){
            bigBoxTutorial2Occured = true;
        }
        else if (currentDialogue.dialogueTag == "ButtonTutorial1"){
            buttonTutorial1Occured = true;
            playerController._bMovementDisabled = false;
        }
        else if (currentDialogue.dialogueTag == "ButtonTutorial2"){
            buttonTutorial2Occured = true;
        }
        else if (currentDialogue.dialogueTag == "ButtonTutorial3"){
            buttonTutorial3Occured = true;
            playerController._bMovementDisabled = false;
        }
        else if (currentDialogue.dialogueTag == "RestPoint"){
            restPointTutorialOccured = true;
            playerController._bMovementDisabled = false;
        }
        else if (currentDialogue.dialogueTag == "ExaminerTutorial"){
            examinerTutorialOccured = true;
            playerController._bMovementDisabled = false;
        }
        else if (currentDialogue.dialogueTag == "PlatformTutorial"){
            platformTutorialOccured = true;
            playerController._bMovementDisabled = false;
        }
        else if (currentDialogue.dialogueTag == "PuzzleOne"){
            puzzle1 = true;
        }
        else if (currentDialogue.dialogueTag == "PuzzleTwo"){
            puzzle2 = true;
        }
        else if (currentDialogue.dialogueTag == "PuzzleThree"){
            puzzle3 = true;
        }
        else if (currentDialogue.dialogueTag == "PuzzleFour"){
            puzzle4 = true;
        }
        else if (currentDialogue.dialogueTag == "PuzzleFive"){
            puzzle5 = true;
        }
        else if (currentDialogue.dialogueTag == "GameEnd"){
            gameEnd = true;
            changeScene.SwitchScene("Credits");
        }

        if (animator != null)
        {
            animator.SetBool("IsOpen", false);
        }
    }
    void Intro()
    {
        if (!introTutorialOccured){
            Dialogue intro = new Dialogue();
            intro.name = "????";
            intro.dialogueTag = "Intro";
            intro.sentences = new string[9];
            intro.sentences[0] = "........";
            intro.sentences[1] = "....llo?....";
            intro.sentences[2] = "...Hello?";
            intro.sentences[3] = "Can you hear me?";
            intro.sentences[4] = "You can? Oh, that’s wonderful! Wonderful!";
            intro.sentences[5] = "I’ve been looking for you for a while now. I hear you have forgotten about me. That’s okay. People forget things all the time, especially after they go through something like you did.";
            intro.sentences[6] = "I’m just glad I can remind you that I’m here.";
            intro.sentences[7] = "Anyway, I think a formal introduction, or rather, reintroduction is in order. My name is Care.";
            intro.sentences[8] = "Now, let’s get you moving. (press A or D to start moving).";
            playerController._bMovementDisabled = true;
            soundManager.Stop("Walking");
            StartDialogue(intro);
        }
    }
    void IntroMovementTutorial()
    {
        if (introTutorialOccured && !introMovementTutorialOccured){
            Dialogue introMovement = new Dialogue();
            introMovement.name = "Care";
            introMovement.dialogueTag = "IntroMovement";
            introMovement.sentences = new string[4];
            introMovement.sentences[0] = "Look at that! Comes naturally, doesn’t it? And you’re quick on your feet, too. Now, let’s get you off your feet (press Space Bar to jump).";
            introMovement.sentences[1] = "Woah, that’s impressive!";
            introMovement.sentences[2] = "Does it feel nice? To move around a bit? Get yourself out of the funk? I bet it does.";
            introMovement.sentences[3] = "Now then, how about we start getting you out of this place, okay? And don’t worry, I’ll be with you every step of the way.";
            StartDialogue(introMovement);
        }
    }
    void SmallBoxTutorialOne()
    {
        if (introMovementTutorialOccured && !smallBoxTutorial1Occured){
            Dialogue smallBoxTutorial1 = new Dialogue();
            smallBoxTutorial1.name = "Care";
            smallBoxTutorial1.dialogueTag = "SmallBoxTutorial1";
            smallBoxTutorial1.sentences = new string[2];
            smallBoxTutorial1.sentences[0] = "Hey, look. A box! One of nature’s finest creations, haha.";
            smallBoxTutorial1.sentences[1] = "That…that was a joke. Anyway, something about it looks…wrong. Let me go over there and check it out. (Put Care over the box using the mouse)";
            playerController._bMovementDisabled = true;
            soundManager.Stop("Walking");
            StartDialogue(smallBoxTutorial1);
        }
    }
    void SmallBoxTutorialTwo()
    {
        if (smallBoxTutorial1Occured && !smallBoxTutorial2Occured){
            Dialogue smallBoxTutorial2 = new Dialogue();
            smallBoxTutorial2.name = "Care";
            smallBoxTutorial2.dialogueTag = "SmallBoxTutorial2";
            smallBoxTutorial2.sentences = new string[3];
            smallBoxTutorial2.sentences[0] = "Ah, I see. You’ve withdrawn a lot, haven’t you? Makes even the smallest things seem insurmountable. That’s okay, I understand. ";
            smallBoxTutorial2.sentences[1] = "Everyone gets this way sometimes. You’ve watched your world turn to darkness because how could it ever be anything else after you’ve experienced what you have?";
            smallBoxTutorial2.sentences[2] = "But it’s okay, I’m here to help. Now, we’ve gotta start somewhere, and this box looks like the perfect candidate. (To give energy to the box, click on it)";
            StartDialogue(smallBoxTutorial2);
        }
    }
    void SmallBoxTutorialThree()
    {
        if (smallBoxTutorial2Occured && !smallBoxTutorial3Occured){
            Dialogue smallBoxTutorial3 = new Dialogue();
            smallBoxTutorial3.name = "Care";
            smallBoxTutorial3.dialogueTag = "SmallBoxTutorial3";
            smallBoxTutorial3.sentences = new string[7];
            smallBoxTutorial3.sentences[0] = "Good! Did you feel that? Felt weird, right? That was energy. It’s probably been a while for you, but to do anything in the world, you have to use energy.";
            smallBoxTutorial3.sentences[1] = "When you do, it’ll make you a little slower, a little heavier, because putting energy into other things is a drain on yourself. But it’s also how we have to move forward. ";
            smallBoxTutorial3.sentences[2] = "The key is balancing the energy you have and the energy you give out. Finding that balance is the key to moving forward from what you’ve experienced.";
            smallBoxTutorial3.sentences[3] = "(You can track your energy using the Energy Bar on the top left of the screen)";
            smallBoxTutorial3.sentences[4] = "Now, let’s go move a box. I’ll help you.";
            smallBoxTutorial3.sentences[5] = "(Walk over to the box, move Care over it, and pick it up using the interact key (E))";
            smallBoxTutorial3.sentences[6] = "(To interact with any interactable item, Care must be over the item when you want to interact with it)";
            StartDialogue(smallBoxTutorial3);
        }
    }
    void SmallBoxTutorialFour()
    {
        if (smallBoxTutorial3Occured && !smallBoxTutorial4Occured){
            Dialogue smallBoxTutorial4 = new Dialogue();
            smallBoxTutorial4.name = "Care";
            smallBoxTutorial4.dialogueTag = "SmallBoxTutorial4";
            smallBoxTutorial4.sentences = new string[4];
            smallBoxTutorial4.sentences[0] = "Wonderful! Now, carry the box over to the other side of the room. Don’t take your energy back from it though. If you do, you’ll drop it.";
            smallBoxTutorial4.sentences[1] = "If you place it on the other side of the room, you should be able to jump on it to get over that wall.";
            smallBoxTutorial4.sentences[2] = "You’ll probably need all your energy though, so just do the same thing you did to give it energy to get your energy back!";
            smallBoxTutorial4.sentences[3] = "(Move the box, move Care off the box, and press the interact key to drop it. Then click the box to regain your energy)";
            StartDialogue(smallBoxTutorial4);
        }
    }
    void BigBoxTutorialOne()
    {
        if (smallBoxTutorial4Occured && !bigBoxTutorial1Occured){
            Dialogue bigBoxTutorial1 = new Dialogue();
            bigBoxTutorial1.name = "Care";
            bigBoxTutorial1.dialogueTag = "BigBoxTutorial1";
            bigBoxTutorial1.sentences = new string[2];
            bigBoxTutorial1.sentences[0] = "Look at that, another box! Bigger this time though. Looks like they’re evolving.";
            bigBoxTutorial1.sentences[1] = "Ok, yeah, I’ll stop trying to be funny. Anyway, I bet if you give that box some of your energy, you could push it around, just like the other one. You probably couldn’t jump around with it, though. (Interact with the big box)";
            playerController._bMovementDisabled = true;
            soundManager.Stop("Walking");
            StartDialogue(bigBoxTutorial1);
        }
    }
    void BigBoxTutorialTwo()
    {
        if (bigBoxTutorial1Occured && !bigBoxTutorial2Occured){
            Dialogue bigBoxTutorial2 = new Dialogue();
            bigBoxTutorial2.name = "Care";
            bigBoxTutorial2.dialogueTag = "BigBoxTutorial2";
            bigBoxTutorial2.sentences = new string[1];
            bigBoxTutorial2.sentences[0] = "Woah, it’s definitely heavy. Okay, let’s push this thing forward.";
            StartDialogue(bigBoxTutorial2);
        }
    }
    void ButtonTutorialOne()
    {
        if (bigBoxTutorial2Occured && !buttonTutorial1Occured){
            Dialogue buttonTutorial1 = new Dialogue();
            buttonTutorial1.name = "Care";
            buttonTutorial1.dialogueTag = "ButtonTutorial1";
            buttonTutorial1.sentences = new string[1];
            buttonTutorial1.sentences[0] = "Uh oh, looks like we’re blocked by a door. That button behind us probably opens it. Let’s go check. (give energy to the button and interact with it)";
            playerController.mPlayerState = CharacterState.IDLE;
            playerController._bMovementDisabled = true;
            soundManager.Stop("BoxPush/Pull");
            soundManager.Stop("Walking");
            StartDialogue(buttonTutorial1);
        }
    }
    void ButtonTutorialTwo()
    {
        Debug.Log("Invoked");
        if (buttonTutorial1Occured && !buttonTutorial2Occured){
            Dialogue buttonTutorial2 = new Dialogue();
            buttonTutorial2.name = "Care";
            buttonTutorial2.dialogueTag = "ButtonTutorial2";
            buttonTutorial2.sentences = new string[2];
            buttonTutorial2.sentences[0] = "You hear that? Sounds like the door opened up! Let’s go get that box through the door.";
            buttonTutorial2.sentences[1] = "Oh! Don’t take your energy away from the button. If you do, the door will close, and we don’t want that.";
            StartDialogue(buttonTutorial2);
        }
    }
    void ButtonTutorialThree()
    {
        if (buttonTutorial2Occured && !buttonTutorial3Occured){
            Dialogue buttonTutorial3 = new Dialogue();
            buttonTutorial3.name = "Care";
            buttonTutorial3.dialogueTag = "ButtonTutorial3";
            buttonTutorial3.sentences = new string[7];
            buttonTutorial3.sentences[0] = "Well done! You did great. The task may have seemed simple, but I know it was a lot for you. I’m proud of you.";
            buttonTutorial3.sentences[1] = "I’ll mention though, not all doors are as forgiving as that one. If a door is colored red, that means its on a timer, and when that timer runs out, the door closes...forever.";
            buttonTutorial3.sentences[2] = "...or until you press the button again! Sorry, I lied earlier about not joking anymore.";
            buttonTutorial3.sentences[3] = "Anyway, you need to get your energy back from that button. I know it’s too far away, but if you concentrate, you’ll get your energy back from it.";
            buttonTutorial3.sentences[4] = "I will warn you though, concentrating returns energy from everywhere, so you’ll get your energy back from the box here too. Then once you’re done concentrating, we can check out the Rest Point up on that ledge.";
            buttonTutorial3.sentences[5] = "Anyway, I’ll stop talking. Time to concentrate!";
            buttonTutorial3.sentences[6] = "(Click and hold the mouse button to concentrate. Your Concentration Time is tracked by the bar below the Energy Bar in the top left of the screen)";
            playerController.mPlayerState = CharacterState.IDLE;
            playerController._bMovementDisabled = true;
            soundManager.Stop("BoxPush/Pull");
            soundManager.Stop("Walking");
            StartDialogue(buttonTutorial3);
        }
    }
    void CheckpointBark()
    {
        if (buttonTutorial3Occured && !restPointTutorialOccured){
            Dialogue firstCheckpointTutorial = new Dialogue();
            firstCheckpointTutorial.name = "Care";
            firstCheckpointTutorial.dialogueTag = "RestPoint";
            firstCheckpointTutorial.sentences = new string[5];
            firstCheckpointTutorial.sentences[0] = "This is a rest point. Passing over one of these will take a snapshot of everything in the world.";
            firstCheckpointTutorial.sentences[1] = "Sometimes, you might get stuck and not be able to continue. If that happens, just think about the Rest Point and then you’ll return to it.";
            firstCheckpointTutorial.sentences[2] = "When you return to the point, some of the progress you make may be lost, but I’m confident you’ll be able to get right back to where you were.";
            firstCheckpointTutorial.sentences[3] = "(If you are ever unable to continue, press R to return to a Rest Point)";
            firstCheckpointTutorial.sentences[4] = "Now, let’s go see what that purple swirly thing is on the ground. I think I know, but I want to be sure.";
            playerController.mPlayerState = CharacterState.IDLE;
            playerController._bMovementDisabled = true;
            soundManager.Stop("Walking");
            StartDialogue(firstCheckpointTutorial);
        }
        else if (checkpointBarkFirst && returnedToRestPoint){
            Dialogue firstCheckpointReturn = new Dialogue();
            firstCheckpointReturn.name = "Care";
            firstCheckpointReturn.dialogueTag = "RestPointReturnFirst";
            firstCheckpointReturn.sentences = new string[5];
            firstCheckpointReturn.sentences[0] = "Looks like something went wrong.";
            firstCheckpointReturn.sentences[1] = "That's okay though! It happens to all of us. Take your time and manage yourself.";
            firstCheckpointReturn.sentences[2] = "Remember, things take time. We all run into obstacles. We all fall down.";
            firstCheckpointReturn.sentences[3] = "If it was easy, well, I'd be out of a job, haha.";
            firstCheckpointReturn.sentences[4] = "Just remember, I'm with you every step of the way.";
            checkpointBarkFirst = false;
            returnedToRestPoint = false;
            StartDialogue(firstCheckpointReturn);
        }
        else if (returnedToRestPoint){
            Dialogue checkpointBarkList = new Dialogue();
            Dialogue checkpointBark = new Dialogue();
            checkpointBark.name = "Care";
            checkpointBark.dialogueTag = "RestPointBarks";
            checkpointBark.sentences = new string[1];
            checkpointBarkList.sentences = new string[8];
            checkpointBarkList.sentences[0] = "Take your time. You'll figure it out.";
            checkpointBarkList.sentences[1] = "Take in all your surroundings. You've got time.";
            checkpointBarkList.sentences[2] = "It's okay to take a break if you need to.";
            checkpointBarkList.sentences[3] = "I know it's rough, but you got this.";
            checkpointBarkList.sentences[4] = "Remember, managing your energy is key. Keep an eye on how you're feeling.";
            checkpointBarkList.sentences[5] = "No need to rush. Just breath.";
            checkpointBarkList.sentences[6] = "You're improving each time. Just a little more.";
            checkpointBarkList.sentences[7] = "Don't wear yourself out. Relax, think, then try again.";

            returnedToRestPoint = false;
            int index = Random.Range(0, checkpointBarkList.sentences.Length - 1);
            checkpointBark.sentences[0] = checkpointBarkList.sentences[index];

            StartDialogue(checkpointBark);
        }
    }
    void ExaminerTutorial()
    {
        if (restPointTutorialOccured && !examinerTutorialOccured){
            Dialogue examinerTutorial = new Dialogue();
            examinerTutorial.name = "Care";
            examinerTutorial.dialogueTag = "ExaminerTutorial";
            examinerTutorial.sentences = new string[4];
            examinerTutorial.sentences[0] = "I was right! This is an Examiner. From here, you can expand your vision and see everything that lies before you. Cool, right?";
            examinerTutorial.sentences[1] = "Not only that, when you use the examiner, objects that use will pulse, so you can see where you can use your energy.";
            examinerTutorial.sentences[2] = "(When standing on an Examiner, hold Spacebar to use it. Releasing Space Bar will end the examination)";
            examinerTutorial.sentences[3] = "Oh! Looks like there’s one more object here to check out. Put me over that platform there.";
            playerController._bMovementDisabled = true;
            soundManager.Stop("Walking");
            StartDialogue(examinerTutorial);
        }
    }
    void PlatformTutorial()
    {
        if (examinerTutorialOccured && !platformTutorialOccured){
            Dialogue platformTutorial = new Dialogue();
            platformTutorial.name = "Care";
            platformTutorial.dialogueTag = "PlatformTutorial";
            platformTutorial.sentences = new string[5];
            platformTutorial.sentences[0] = "This is a platform. Giving it energy will let it move in the direction shown by its path.";
            platformTutorial.sentences[1] = "It will go back and forth on the path endlessly as long as it has energy. Kind of a sad existence, but I think it enjoys it, so I won’t judge.";
            platformTutorial.sentences[2] = "Oh, if you take energy away from a platform, it will move back to the point that it started at unless it was already there. Good thing to keep in mind, I think.";
            platformTutorial.sentences[3] = "Anyway, I think that’s all I remember seeing when I made my way in here, so now it’s time to get you out of here.";
            platformTutorial.sentences[4] = "And like I said, I’ll be with you every step of the way.";
            playerController._bMovementDisabled = true;
            soundManager.Stop("Walking");
            StartDialogue(platformTutorial);
        }
    }
    void PuzzleOne()
    {
        if (!puzzle1){
            Dialogue firstPuzzle = new Dialogue();
            firstPuzzle.name = "Care";
            firstPuzzle.dialogueTag = "PuzzleOne";
            firstPuzzle.sentences = new string[3];
            firstPuzzle.sentences[0] = "Well done! You did a great job back there.";
            firstPuzzle.sentences[1] = "I’m glad I’m here to help you through this. It’s easy to forget how to help yourself through hard times. I’m glad I can remind you.";
            firstPuzzle.sentences[2] = "Let’s keep going.";
            StartDialogue(firstPuzzle);
        }
    }
    void PuzzleTwo()
    {
        if (!puzzle2){
            Dialogue secondPuzzle = new Dialogue();
            secondPuzzle.name = "Care";
            secondPuzzle.dialogueTag = "PuzzleTwo";
            secondPuzzle.sentences = new string[1];
            secondPuzzle.sentences[0] = "You’re doing so well! Just remember, don’t push yourself too hard. Just remember, I’m here to help.";
            StartDialogue(secondPuzzle);
        }
    }
    void PuzzleThree()
    {
        if (!puzzle3){
            Dialogue thirdPuzzle = new Dialogue();
            thirdPuzzle.name = "Care";
            thirdPuzzle.dialogueTag = "PuzzleThree";
            thirdPuzzle.sentences = new string[2];
            thirdPuzzle.sentences[0] = "Wow! We’re halfway through! You’re doing so well.";
            thirdPuzzle.sentences[1] = "Just keeping on moving. If anything weighs heavy on you, just keep following my light. I’ll be here the whole time.";
            StartDialogue(thirdPuzzle);
        }
    }
    void PuzzleFour()
    {
        if (!puzzle4){
            Dialogue fourthPuzzle = new Dialogue();
            fourthPuzzle.name = "Care";
            fourthPuzzle.dialogueTag = "PuzzleFour";
            fourthPuzzle.sentences = new string[3];
            fourthPuzzle.sentences[0] = "You’re closing in on the end. Just a little more.";
            fourthPuzzle.sentences[1] = "I know it’s hard. It’s hard to keep moving when so much has happened to you. But, you’ll be okay. Trust me.";
            fourthPuzzle.sentences[2] = "I’m here to make sure of that.";
            StartDialogue(fourthPuzzle);
        }
    }
    void PuzzleFive()
    {
        if (!puzzle5){
            Dialogue fifthPuzzle = new Dialogue();
            fifthPuzzle.name = "Care";
            fifthPuzzle.dialogueTag = "PuzzleFive";
            fifthPuzzle.sentences = new string[2];
            fifthPuzzle.sentences[0] = "That was the last one! The way out is just below.";
            fifthPuzzle.sentences[1] = "You’ve done so well. I’m proud of you.";
            StartDialogue(fifthPuzzle);
        }
    }
    void GameEnd()
    {
        if (!gameEnd){
            Dialogue gameEndDialogue = new Dialogue();
            gameEndDialogue.name = "Care";
            gameEndDialogue.dialogueTag = "GameEnd";
            gameEndDialogue.sentences = new string[3];
            gameEndDialogue.sentences[0] = "You made it! I’m proud of you. I really am. Despite all your hardship, you made it here.";
            gameEndDialogue.sentences[1] = "I know that the rest of the world seems scary. That moving forward after so much pain is hard. But you’ll get through it. You’ve already made it to this end, so you’ll make it to the next.";
            gameEndDialogue.sentences[2] = "And I’ll be right there with you.";
            playerController._bMovementDisabled = true;
            soundManager.Stop("Walking");
            StartDialogue(gameEndDialogue);
        }
    }

}
