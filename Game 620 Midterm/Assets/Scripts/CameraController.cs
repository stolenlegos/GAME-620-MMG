using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
  private Vector3 offset;
  
  [SerializeField]
  private GameObject player;


    void Start() {
      offset = transform.position - player.transform.position;
    }


    void LateUpdate() {
      transform.position = player.transform.position + offset;
    }
}
