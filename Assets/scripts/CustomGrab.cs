using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CustomGrab : MonoBehaviour
{
    // This script should be attached to both controller objects in the scene
    // Make sure to define the input in the editor (LeftHand/Grip and RightHand/Grip recommended respectively)
    CustomGrab otherHand = null;
    public List<Transform> nearObjects = new List<Transform>();
    public Transform grabbedObject = null;
    public InputActionReference action;

    private Vector3 prevPosition;
    private Quaternion prevRotation;

    private Vector3 grabNormal;
    private Vector3 deltaPos;
    private Quaternion deltaRot;

    private void Start()
    {
        action.action.Enable();

        // Find the other hand
        foreach(CustomGrab c in transform.parent.GetComponentsInChildren<CustomGrab>())
        {
            if (c != this)
                otherHand = c;
        }
    }

    void Update()
    {
        if (action.action.IsPressed())
        {
            // Grab nearby object or the object in the other hand
            if (!grabbedObject)
                grabbedObject = nearObjects.Count > 0 ? nearObjects[0] : otherHand.grabbedObject;
                grabbedObject.gameObject.GetComponent<Rigidbody>().useGravity = false;

            if (grabbedObject)
            {
                // Change these to add the delta position and rotation instead
                // Save the position and rotation at the end of Update function, so you can compare previous pos/rot to current here
                
                grabNormal = grabbedObject.position - transform.position;
                deltaPos = transform.position - prevPosition;
                deltaRot = transform.rotation * Quaternion.Inverse(prevRotation);

                //grabbedObject.position = transform.position;
                grabbedObject.position -= grabNormal;
                grabbedObject.position += deltaRot * grabNormal;
                grabbedObject.position += deltaPos;

                grabbedObject.rotation = deltaRot * grabbedObject.rotation;
            }
        }
        // If let go of button, release object
        else if (grabbedObject)
        {
            if (!otherHand.grabbedObject)
                grabbedObject.gameObject.GetComponent<Rigidbody>().useGravity = true;
            
            grabbedObject = null;
        }

        // Should save the current position and rotation here
        prevPosition = transform.position;
        prevRotation = transform.rotation;
    }

    private void OnTriggerEnter(Collider other)
    {
        // Make sure to tag grabbable objects with the "grabbable" tag
        // You also need to make sure to have colliders for the grabbable objects and the controllers
        // Make sure to set the controller colliders as triggers or they will get misplaced
        // You also need to add Rigidbody to the controllers for these functions to be triggered
        // Make sure gravity is disabled though, or your controllers will (virtually) fall to the ground

        Transform t = other.transform;
        if(t && t.tag.ToLower()=="grabbable")
            nearObjects.Add(t);
    }

    private void OnTriggerExit(Collider other)
    {
        Transform t = other.transform;
        if( t && t.tag.ToLower()=="grabbable")
            nearObjects.Remove(t);
    }
}