using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using Random = UnityEngine.Random;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    public Text nameText;
    public Text dialogueText;
    public Image textEnder;
    public static DialogueManager instance;
    private Coroutine typeWriterCoroutine;
    private bool typing = false;
    private bool checkpointBarkFirst = true;
    private float starTime = 0f;
    private float endTime = 1f;
    public bool careOff = true;

    public Animator animator;
    public SoundManager soundManager;

    private Queue<string> sentences;
    private string currentSentence;
    private string barkString;

    public static event Action careOn;

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

    private void OnEnable()
    {
        Checkpoints.checkpointActivated += CheckpointBark;
    }

    void Start(){
        sentences = new Queue<string>();
        soundManager = GameObject.FindGameObjectWithTag("SoundManager").GetComponent<SoundManager>();
    }
    private void Update()
    {
        if(sentences.Count > 0)
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (!typing)
                {
                    DisplayNextSentence();
                }
                else
                {
                    if(typeWriterCoroutine != null)
                    {
                        StopCoroutine(typeWriterCoroutine);
                    }
                    typing = false;
                    dialogueText.text = currentSentence;
                }
            }
        }
        else if (sentences.Count == 0 && animator.GetBool("IsOpen") == true)
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (!typing)
                {
                    DisplayNextSentence();
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
        if (typing)
        {
            textEnder.enabled = false;
        }
        if (!typing)
        {
            soundManager.Stop("CareTalk");
            if (starTime <= 0)
            {
                Debug.Log("On");
                textEnder.enabled = true;
                starTime += Time.deltaTime;
            }
            else if (starTime >= endTime)
            {
                Debug.Log("Off");
                textEnder.enabled = false;
                starTime -= Time.deltaTime;
            }
        }
        Debug.Log(textEnder.enabled);
    }

    public void StartDialogue (Dialogue dialogue){
        animator.SetBool("IsOpen", true);
        nameText.text = dialogue.name;

        sentences.Clear();

            foreach (string sentence in dialogue.sentences)
            {
                sentences.Enqueue(sentence);
            }

        DisplayNextSentence();
    }

    public void DisplayNextSentence(){
        if (sentences.Count == 0){
            EndDialogue();
            return;
        }

        string sentence = sentences.Dequeue();
        //dialogueText.text = sentence;
        if (typeWriterCoroutine != null)
        {
            StopCoroutine(typeWriterCoroutine);
        }
        typeWriterCoroutine = StartCoroutine(TypeSentence(sentence));
    }

    IEnumerator TypeSentence (string sentence){
        currentSentence = sentence;
        typing = true;
        if (typing)
        {
            soundManager.Play("CareTalk");
        }
        dialogueText.text = "";
        foreach(char letter in sentence.ToCharArray()){
            dialogueText.text += letter;
            yield return new WaitForSeconds(0.1f);
        }
        typing = false;
    }

    void EndDialogue(){
        if (careOff)
        {
            careOn?.Invoke();
            careOff = false;
        }

        animator.SetBool("IsOpen", false);

        Debug.Log("End of Conversation");
    }

    void CheckpointBark()
    {
        Debug.Log("CheckpointBark");
        if (checkpointBarkFirst)
        {
            Dialogue firstCheckpoint = new Dialogue();
            firstCheckpoint.name = "Care";
            firstCheckpoint.sentences = new string[5];
            firstCheckpoint.sentences[0] = "Looks like something went wrong.";
            firstCheckpoint.sentences[1] = "That's okay though! It happens to all of us. Take your time and manage yourself.";
            firstCheckpoint.sentences[2] = "Remember, things take time. We all run into obstacles. We all fall down.";
            firstCheckpoint.sentences[3] = "If it was easy, well, I'd be out of a job, haha.";
            firstCheckpoint.sentences[4] = "Just remember, I'm with you every step of the way.";
            checkpointBarkFirst = false;
            StartDialogue(firstCheckpoint);
        }
        else
        {
            Dialogue checkpointBarkList = new Dialogue();
            Dialogue checkpointBark = new Dialogue();
            checkpointBark.name = "Care";
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

            int index = Random.Range(0, checkpointBarkList.sentences.Length - 1);
            checkpointBark.sentences[0] = checkpointBarkList.sentences[index];

            StartDialogue(checkpointBark);
        }
    }

}
