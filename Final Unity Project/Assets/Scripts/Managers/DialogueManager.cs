using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    public Text nameText;
    public Text dialogueText;
    public static DialogueManager instance;
    private bool checkpointBarkFirst;

    public Animator animator;

    private Queue<string> sentences;

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
    }

    public void StartDialogue (Dialogue dialogue){
        animator.SetBool("IsOpen", true);
        nameText.text = dialogue.name;

        sentences.Clear();

        foreach(string sentence in dialogue.sentences){
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
        StopAllCoroutines();
        StartCoroutine(TypeSentence(sentence));
    }

    IEnumerator TypeSentence (string sentence){
        dialogueText.text = "";
        foreach(char letter in sentence.ToCharArray()){
            dialogueText.text += letter;
            yield return new WaitForSeconds(0.1f);
        }
    }

    void EndDialogue(){
        animator.SetBool("IsOpen", false);

        Debug.Log("End of Conversation");
    }

    void CheckpointBark()
    {
        Debug.Log("CheckpointBark");
        Dialogue dialogue = new Dialogue();
        dialogue.name = "Care";
        dialogue.sentences = new string[2];
        dialogue.sentences[0] = "Hello World.";
        dialogue.sentences[1] = "It's me.";
        StartDialogue(dialogue);
    }

}
