using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CareLightScript : MonoBehaviour
{
    public float depth = -10.0f;
    private CursorManager cursorManager;
    // Start is called before the first frame update
    void Start()
    {
        cursorManager = GameObject.FindGameObjectWithTag("CursorManager").GetComponent<CursorManager>();
    }

    // Update is called once per frame
    void Update()
    {
        FollowMousePosition();
    }

    void FollowMousePosition()
    {
        if (cursorManager.careActivated)
        {
            var mousePos = Input.mousePosition;
            var wantedPos = Camera.main.ScreenToWorldPoint(new Vector3(mousePos.x /*+ 15f*/, mousePos.y /*- 15f*/, depth));
            transform.position = wantedPos;
        }
    }
}
