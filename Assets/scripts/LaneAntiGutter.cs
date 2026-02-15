using UnityEngine;

public class LaneAntiGutter : MonoBehaviour
{
    public LaneMain main;

    public GameObject ball;

    private Vector3 ballReturnPosition;

    void Start()
    {
        ballReturnPosition = ball.transform.position;
    }

    void OnTriggerExit(Collider other)
    {
        LaneReturnable x = other.GetComponent<LaneReturnable>();
        if (x)
        {
            if (main.IsReady()) main.StartScoring();
            x.transform.position = ballReturnPosition;
            Rigidbody rigidbody = x.GetComponent<Rigidbody>();
            rigidbody.linearVelocity *= 0;
            rigidbody.angularVelocity *= 0;
        }
    }
}
