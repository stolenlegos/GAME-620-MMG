using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorManager : MonoBehaviour
{
    [SerializeField]
    private Texture2D[] cursorTextureArray;
    [SerializeField]
    private int frameCount;
    [SerializeField]
    private float frameRate;

    private int currentFrame;
    private float frameTimer;

    // Start is called before the first frame update
    private void OnEnable()
    {
        DialogueManager.careOn += ActivateCareCursor;
    }
    private void Start()
    {
        Cursor.visible = false;
    }
    private void Update()
    {
        frameTimer -= Time.deltaTime;
        if(frameTimer <= 0f)
        {
            frameTimer += frameRate;
            currentFrame = (currentFrame + 1) % frameCount;
            Cursor.SetCursor(cursorTextureArray[currentFrame], new Vector2(10, 10), CursorMode.Auto);
        }
    }
    private void ActivateCareCursor()
    {
        Cursor.visible = true;
        Cursor.SetCursor(cursorTextureArray[0], new Vector2(10, 10), CursorMode.Auto);
    }

}
