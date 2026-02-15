using UnityEngine;

public class LaneGutter : MonoBehaviour
{
    public LaneMain main;

    public GameObject ball;

    private Vector3 ballReturnPoint;

    void Start()
    {
        ballReturnPoint = ball.transform.position;
    }

    void OnTriggerEnter(Collider other)
    {
        LaneReturnable x = other.GetComponent<LaneReturnable>();
        if (x)
        {
            if (main.IsReady()) main.StartScoring();
            x.transform.position = ballReturnPoint;
        }
    }

    void OnTriggerExit(Collider other)
    {
        
    }
}
