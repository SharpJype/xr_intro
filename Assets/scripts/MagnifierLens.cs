using UnityEngine;

public class MagnifierLens : MonoBehaviour
{
    public Camera lensCamera;
    public Camera playerCamera;

    private Vector3 pos1;
    private Vector3 pos2;
    private Quaternion rot;
    private bool active = true;

    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (active)
        {
            UpdateLensCamera();
        }
        
    }

    void UpdateLensCamera()
    {
        //Quaternion rot = new Quaternion();
        transform.GetPositionAndRotation(out pos2, out rot);
        float z = rot.eulerAngles.z;
        playerCamera.transform.GetPositionAndRotation(out pos1, out rot);

        Vector3 posDiff = pos2-pos1;
        rot.SetLookRotation(posDiff);
        rot.eulerAngles = new Vector3(rot.eulerAngles.x, rot.eulerAngles.y, -rot.eulerAngles.z+z);

        lensCamera.transform.SetPositionAndRotation(pos2+posDiff, rot);
        //transform.SetPositionAndRotation(pos2, rot);
    }
}
