using UnityEngine;

public class ApplyForce : MonoBehaviour
{

    public Vector3 applyPulseForce;
    public Vector3 applyConstantForce;

    public Vector3 applyPulseTorque;
    public Vector3 applyConstantTorque;

    private Rigidbody rb;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();

        rb.AddRelativeForce(Quaternion.Inverse(transform.rotation)*applyPulseForce);
        rb.AddRelativeTorque(Quaternion.Inverse(transform.rotation)*applyPulseTorque);
    }

    // Update is called once per frame
    void Update()
    {
        if (applyConstantForce.magnitude>0)
        {
            rb.AddRelativeForce(Quaternion.Inverse(transform.rotation)*applyConstantForce*Time.deltaTime);
        }
        if (applyConstantTorque.magnitude>0)
        {
            rb.AddRelativeTorque(Quaternion.Inverse(transform.rotation)*applyConstantTorque*Time.deltaTime);
        }
    }
}
