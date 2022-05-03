using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuCursorScript : MonoBehaviour
{
    [SerializeField]
    private Texture2D[] cursorTextureArray;
    [SerializeField]
    private int frameCount;
    [SerializeField]
    private float frameRate;

    private int currentFrame;
    private float frameTimer;
    private Vector3 lastMouseCoordinate = Vector3.zero;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        frameTimer -= Time.deltaTime;
        if (frameTimer <= 0f)
        {
            frameTimer += frameRate;
            currentFrame = (currentFrame + 1) % frameCount;
            Cursor.SetCursor(cursorTextureArray[currentFrame], new Vector2(10, 10), CursorMode.Auto);
        }
    }
}
