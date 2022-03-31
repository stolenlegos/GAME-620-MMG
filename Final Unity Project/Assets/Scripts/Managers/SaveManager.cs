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
    public GameObject _mUIManager;

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
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void SavePositions()
    {
        smallBoxes = GameObject.FindGameObjectsWithTag("box_Small");
        bigBoxes = GameObject.FindGameObjectsWithTag("box_Big");
        buttons = GameObject.FindGameObjectsWithTag("Button");
        platforms = GameObject.FindGameObjectsWithTag("Platform");
        player = GameObject.FindGameObjectWithTag("Player");
        _mUIManager = GameObject.FindGameObjectWithTag("UIManager");

        foreach (GameObject gameObject in buttons)
        {
            gameObject.GetComponent<SaturationControl>().SaveCurrentState();
        }
        foreach (GameObject gameObject in bigBoxes)
        {
            gameObject.GetComponent<SaturationControl>().SaveCurrentState();
            gameObject.GetComponent<AllowMovement>().SaveCurrentState();
        }
        foreach (GameObject gameObject in smallBoxes)
        {
            gameObject.GetComponent<SaturationControl>().SaveCurrentState();
            gameObject.GetComponent<AllowMovement>().SaveCurrentState();
        }
        foreach (GameObject gameObject in platforms)
        {
            gameObject.GetComponent<SaturationControl>().SaveCurrentState();
            gameObject.GetComponent<PlatformMovement>().SaveCurrentState();
        }
        player.GetComponent<PlayerController>().SaveCurrentState();
        player.GetComponent<EnergyManager>().SaveCurrentState();
        _mUIManager.GetComponent<UIManager>().SaveCurrentState();

    }
    public void ResetPositions()
    {
        smallBoxes = GameObject.FindGameObjectsWithTag("box_Small");
        bigBoxes = GameObject.FindGameObjectsWithTag("box_Big");
        buttons = GameObject.FindGameObjectsWithTag("Button");
        platforms = GameObject.FindGameObjectsWithTag("Platform");
        player = GameObject.FindGameObjectWithTag("Player");
        _mUIManager = GameObject.FindGameObjectWithTag("UIManager");

        foreach (GameObject gameObject in buttons)
        {
            gameObject.GetComponent<SaturationControl>().ResetState();
        }
        foreach (GameObject gameObject in bigBoxes)
        {
            gameObject.GetComponent<SaturationControl>().ResetState();
            gameObject.GetComponent<AllowMovement>().ResetState();
        }
        foreach (GameObject gameObject in smallBoxes)
        {
            gameObject.GetComponent<SaturationControl>().ResetState();
            gameObject.GetComponent<AllowMovement>().ResetState();
        }
        foreach (GameObject gameObject in platforms)
        {
            gameObject.GetComponent<SaturationControl>().ResetState();
            gameObject.GetComponent<PlatformMovement>().ResetState();
        }
        player.GetComponent<PlayerController>().ResetState();
        player.GetComponent<EnergyManager>().ResetState();
        _mUIManager.GetComponent<UIManager>().ResetState();
    }
}
