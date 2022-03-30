using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformMovement : MonoBehaviour
{
    public Transform pos1, pos2;
    public float speed;
    public Transform startPos;
    private bool colored;
    private bool savedColored;

    Vector3 nextPos;

    // Start is called before the first frame update
    void Start()
    {
        nextPos = startPos.position;
        colored = false;
        ShaderEvents.SaturationChange += BoolChange;
    }

    // Update is called once per frame
    void Update()
    {
        if (colored)
        {
            if (transform.position == pos1.position)
            {
                nextPos = pos2.position;
            }
            if (transform.position == pos2.position)
            {
                nextPos = pos1.position;
            }
        }
        if (!colored && transform.position != pos1.position)
        {
            nextPos = pos1.position;
        }
        transform.position = Vector3.MoveTowards(transform.position, nextPos, speed * Time.deltaTime);
    }

    private void BoolChange(GameObject obj)
    {
        if (obj == this.gameObject)
        {
            colored = !colored;
        }
    }
    public void SaveCurrentState()
    {
        savedColored = this.colored;
    }
    public void ResetState()
    {
        this.colored = savedColored;
    }
}
