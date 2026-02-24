using UnityEngine;

public class LanePin : MonoBehaviour
{
    public int points = 1;
    public bool felled = false;

    public Rigidbody rb;

    void Start()
    {
        if (!rb) rb = GetComponent<Rigidbody>();
    }
}
