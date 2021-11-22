using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public Transform player;
    public Vector3 offset;

    private Camera cam;
    private float zoomMultiplier = 2;
    private float defaultFov = 90;
    private float zoomDuration = 2;
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
        ZoomCamera(defaultFov * zoomMultiplier);
        Debug.Log("ZoomOUtStart");
    }
    public void PlayerExamineEnd()
    {
        ZoomCamera(defaultFov);
        _followPlayer = true;
    }
    private void ZoomCamera(float target)
    {
        float angle = Mathf.Abs((defaultFov / zoomMultiplier) - defaultFov);
        cam.fieldOfView = Mathf.MoveTowards(cam.fieldOfView, target, angle / zoomDuration * Time.deltaTime);
    }
}
