using UnityEngine;

public class LaneGutter : MonoBehaviour
{
    public LaneMain main;

    public GameObject ball;

    private Vector3 ballReturnPosition;

    public bool antiGutter = false;

    void Start()
    {
        ballReturnPosition = ball.transform.position;
    }

    void GutterInteraction(Collider other)
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
    void OnTriggerEnter(Collider other)
    {
        if (!antiGutter) GutterInteraction(other);
    }

    void OnTriggerExit(Collider other)
    {
        if (antiGutter) GutterInteraction(other);
    }
}
