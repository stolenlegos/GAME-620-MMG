using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    private static SaveManager instance;
    public GameObject[] smallBoxes;
    public GameObject[] bigBoxes;
    public GameObject[] buttons;
    public GameObject[] platforms;
    public GameObject player;

    private void Awake()
    {
        if(instance == null){
            instance = this;
            DontDestroyOnLoad(instance);
        } else {
            Destroy(gameObject);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        smallBoxes = GameObject.FindGameObjectsWithTag("box_Small");
        bigBoxes = GameObject.FindGameObjectsWithTag("box_Big");
        buttons = GameObject.FindGameObjectsWithTag("Button");
        platforms = GameObject.FindGameObjectsWithTag("Platform");
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void SavePositions()
    {
        foreach (GameObject gameObject in buttons)
        {
            gameObject.GetComponent<SaturationControl>().SaveCurrentState();
        }
        foreach (GameObject gameObject in bigBoxes)
        {
            gameObject.GetComponent<SaturationControl>().SaveCurrentState();
        }
        foreach (GameObject gameObject in smallBoxes)
        {
            gameObject.GetComponent<SaturationControl>().SaveCurrentState();
        }
        foreach (GameObject gameObject in platforms)
        {
            gameObject.GetComponent<SaturationControl>().SaveCurrentState();
        }
        player.GetComponent<PlayerController>().SaveCurrentState();
        
    }
    public void ResetPositions()
    {
        foreach (GameObject gameObject in buttons)
        {
            gameObject.GetComponent<SaturationControl>().ResetState();
        }
        foreach (GameObject gameObject in bigBoxes)
        {
            gameObject.GetComponent<SaturationControl>().ResetState();
        }
        foreach (GameObject gameObject in smallBoxes)
        {
            gameObject.GetComponent<SaturationControl>().ResetState();
        }
        foreach (GameObject gameObject in platforms)
        {
            gameObject.GetComponent<SaturationControl>().ResetState();
        }
        player.GetComponent<PlayerController>().ResetState();
    }
}
