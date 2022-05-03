using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CursorManager : MonoBehaviour
{
    public static CursorManager instance;
    [SerializeField]
    private Texture2D[] cursorTextureArray;
    [SerializeField]
    private int frameCount;
    [SerializeField]
    private float frameRate;

    private Scene scene;
    private int currentFrame;
    private float frameTimer;
    private Vector3 lastMouseCoordinate = Vector3.zero;
    public bool careActivated;

    private void Awake()
    {
        scene = SceneManager.GetActiveScene();
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
        if(scene.name != "MainGame")
        {
            Destroy(gameObject);
        }
    }
    private void ActivateCareCursor()
    {
        Cursor.visible = true;
        Cursor.SetCursor(cursorTextureArray[0], new Vector2(10, 10), CursorMode.Auto);
        careActivated = true;
    }

}
