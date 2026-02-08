using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GrabObjects : MonoBehaviour
{
    private List<Collider> grabCandidates;
    private List<GameObject> grabObjects;

    private bool active = false;

    private Vector3 pos;
    private Quaternion rot;

    public InputActionReference action;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        action.action.Enable();
        action.action.performed += (ctx) =>
        {
            if (active)
            {
                grabObjects.Clear();
            }
            else
            {
                TryGrabbing();
                active = true;
            }
        };
    }

    // Update is called once per frame
    void Update()
    {
        if (grabObjects.Count>0)
        {
            transform.GetPositionAndRotation(out pos, out rot);
            for (int i = 0; i < grabObjects.Count; i++)
            {
                grabObjects[i].transform.SetPositionAndRotation(pos, rot);
            }
        }
    }

    private void TryGrabbing()
    {
        for (int i = 0; i < grabCandidates.Count; i++)
        {
            Collider coll = grabCandidates[i];
            grabObjects.Add(coll.gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!grabCandidates.Contains(other))
        {
            grabCandidates.Add(other);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (grabCandidates.Contains(other))
        {
            grabCandidates.Remove(other);
        }
    }


}
