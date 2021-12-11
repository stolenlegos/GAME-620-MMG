using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerCollisionCounter : MonoBehaviour
{
    public int collisionCount;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        CollisionCount();
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.collider.tag == "Terrain" || other.collider.tag == "box_Big" || other.collider.tag == "box_Small")
        {
            collisionCount++;
        }
    }
    void OnCollisionExit2D(Collision2D other)
    {
        if (other.collider.tag == "Terrain" || other.collider.tag == "box_Big" || other.collider.tag == "box_Small")
        {
            collisionCount--;
        }
    }

    public int CollisionCount()
    {
        return collisionCount;
    }
}
