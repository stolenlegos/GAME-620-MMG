using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxBackground : MonoBehaviour
{
    private Transform _cameraTransform; 
    private Vector3 _lastCameraPosition; 

    private void Start() { 
        _cameraTransform = Camera.main.transform;
        _lastCameraPosition = _cameraTransform.position; 
    }

    private void LateUpdate() { 
        Vector3 deltaMovement = _cameraTransform.position - _lastCameraPosition;
        float parallaxEffectMultiplier = .5f; 
        transform.position += deltaMovement * parallaxEffectMultiplier; 
        _lastCameraPosition = _cameraTransform.position; 
    }
}
