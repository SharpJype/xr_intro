using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.InputSystem;

public class ExternalView : MonoBehaviour
{
    private bool active = false;
    public Vector3 position;
    private Vector3 prev_position;
    private Quaternion prev_rotation;
    public XROrigin origin;
    public InputActionReference action;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        action.action.Enable();
        action.action.performed += (ctx) =>
        {
            if (!active)
            {
                origin.transform.GetPositionAndRotation(out prev_position, out prev_rotation);
                origin.transform.SetPositionAndRotation(position, origin.transform.rotation);
                active = true;
            }
            else
            {
                origin.transform.SetPositionAndRotation(prev_position, origin.transform.rotation);
                active = false;
            }
        };
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
