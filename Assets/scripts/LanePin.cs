using UnityEngine;

public class LanePin : MonoBehaviour
{
    public int value = 1;

    public Rigidbody rigidbody;

    void Start()
    {
        if (!rigidbody) rigidbody = GetComponent<Rigidbody>();
    }
}
