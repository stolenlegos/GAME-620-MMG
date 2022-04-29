using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorManager : MonoBehaviour
{
    public static CursorManager instance;
    [SerializeField]
    private Texture2D[] cursorTextureArray;
    [SerializeField]
    private int frameCount;
    [SerializeField]
    private float frameRate;

    private int currentFrame;
    private float frameTimer;
    private Vector3 lastMouseCoordinate = Vector3.zero;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(instance);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    // Start is called before the first frame update
    private void OnEnable()
    {
        DialogueManager.careOn += ActivateCareCursor;
    }
    private void Start()
    {
        
        //Cursor.visible = false;
    }
    private void Update()
    {
        /*Vector3 mouseDelta = Input.mousePosition - lastMouseCoordinate;
        if(mouseDelta.x < 0)
        {
            cursorTextureArray[currentFrame].
        }*/
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
