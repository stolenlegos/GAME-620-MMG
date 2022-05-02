using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public Transform player;
    public Vector3 offset;

    public static CameraManager instance;

    private Camera cam;
    public int normalview = 30;
    public int zoomBackTimer;
    private float smooth = 2;
    private bool _followPlayer = true;
    public GameObject[] smallBoxes;
    public GameObject[] bigBoxes;
    public GameObject[] buttons;
    public GameObject[] platforms;
    public GameObject[] doors;
    //public Object[] pulseObjects;
    // Start is called before the first frame update
    private void Awake()
    {
        /*if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(instance);
        }
        else
        {
            Destroy(gameObject);
        }*/
    }

    void Start()
    {
        cam = GetComponent<Camera> ();
        smallBoxes = GameObject.FindGameObjectsWithTag("box_Small");
        bigBoxes = GameObject.FindGameObjectsWithTag("box_Big");
        buttons = GameObject.FindGameObjectsWithTag("Button");
        platforms = GameObject.FindGameObjectsWithTag("Platform");
        doors = GameObject.FindGameObjectsWithTag("Door");
        //pulseObjects = Object.FindObjectsOfType<SaturationControl>();
    }
    // Update is called once per frame
    void Update()
    {
        if (_followPlayer == true)
        {
            transform.position = new Vector3(player.position.x + offset.x, player.position.y + offset.y, offset.z);
        }
    }

    public void PlayerExamineStart()
    {
        _followPlayer = false;
        Debug.Log("ZoomOutStart");
        ZoomCameraOut();
    }
    public void PlayerExamineEnd()
    {
        _followPlayer = true;
        ZoomCameraOut();
    }
    private void ZoomCameraOut()
    {
        if (!_followPlayer)
        {
            zoomBackTimer = 550;
            GetComponent<Camera>().fieldOfView = Mathf.Lerp(GetComponent<Camera>().fieldOfView, normalview * 3, Time.deltaTime * smooth);
            transform.position = Vector3.Lerp(transform.position, new Vector3(player.position.x + offset.x + 10, player.position.y + offset.y + 5, offset.z), Time.deltaTime * smooth);
            foreach(GameObject gameObject in buttons)
            {
                gameObject.GetComponent<SaturationControl>().Pulse();
            }
            foreach (GameObject gameObject in bigBoxes)
            {
                gameObject.GetComponent<SaturationControl>().Pulse();
            }
            foreach (GameObject gameObject in smallBoxes)
            {
                gameObject.GetComponent<SaturationControl>().Pulse();
            }
            foreach (GameObject gameObject in platforms)
            {
                gameObject.GetComponent<SaturationControl>().Pulse();
            }
            foreach (GameObject gameObject in doors)
            {
                gameObject.GetComponent<SaturationControl>().Pulse();
            }
            /*foreach (GameObject pulse in pulseObjects)
            {
                pulse.GetComponent<SaturationControl>().Pulse();
            }*/
        }
        else if (_followPlayer)
        {
            zoomBackTimer = zoomBackTimer - 1;
            GetComponent<Camera>().fieldOfView = Mathf.Lerp(GetComponent<Camera>().fieldOfView, normalview, Time.deltaTime * smooth);
            transform.position = Vector3.Lerp(transform.position, new Vector3(player.position.x + offset.x, player.position.y + offset.y, offset.z), Time.deltaTime * smooth);
        }
    }
}
