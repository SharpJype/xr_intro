using UnityEngine;

public class ApplyForce : MonoBehaviour
{

    public Vector3 applyPulseForce;
    public Vector3 applyConstantForce;

    public Vector3 applyPulseTorque;
    public Vector3 applyConstantTorque;

    private Rigidbody rigidbody;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();

        rigidbody.AddRelativeForce(Quaternion.Inverse(transform.rotation)*applyPulseForce);
        rigidbody.AddRelativeTorque(Quaternion.Inverse(transform.rotation)*applyPulseTorque);
    }

    // Update is called once per frame
    void Update()
    {
        if (applyConstantForce.magnitude>0)
        {
            rigidbody.AddRelativeForce(Quaternion.Inverse(transform.rotation)*applyConstantForce*Time.deltaTime);
        }
        if (applyConstantTorque.magnitude>0)
        {
            rigidbody.AddRelativeTorque(Quaternion.Inverse(transform.rotation)*applyConstantTorque*Time.deltaTime);
        }
    }
}
