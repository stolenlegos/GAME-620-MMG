using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public Transform player;
    public Vector3 offset;

    private Camera cam;
    public int normalview = 30;
    public int zoomBackTimer;
    private float smooth = 2;
    private bool _followPlayer = true;
    // Start is called before the first frame update
    void Start()
    {
        cam = GetComponent<Camera> ();
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
            Debug.Log("Normalview: " + zoomBackTimer);
            GetComponent<Camera>().fieldOfView = Mathf.Lerp(GetComponent<Camera>().fieldOfView, normalview * 3, Time.deltaTime * smooth);
            transform.position = Vector3.Lerp(transform.position, new Vector3(player.position.x + offset.x + 10, player.position.y + offset.y + 5, offset.z), Time.deltaTime * smooth);
        }
        else if (_followPlayer)
        {
            zoomBackTimer = zoomBackTimer - 1;
            Debug.Log("Normalview: " + zoomBackTimer);
            GetComponent<Camera>().fieldOfView = Mathf.Lerp(GetComponent<Camera>().fieldOfView, normalview, Time.deltaTime * smooth);
            transform.position = Vector3.Lerp(transform.position, new Vector3(player.position.x + offset.x, player.position.y + offset.y, offset.z), Time.deltaTime * smooth);
        }
    }
}
