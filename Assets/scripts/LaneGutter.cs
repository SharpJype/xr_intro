using UnityEngine;

public class LaneGutter : MonoBehaviour
{
    private LaneMain main;

    public bool antiGutter = false;

    void Start()
    {
        FindLaneMain();
    }

    void GutterInteraction(Collider other)
    {
        LaneReturnable x = other.GetComponent<LaneReturnable>();
        if (x)
        {
            if (main.IsReady()) main.StartScoring();
            x.transform.position = x.GetReturnPosition();
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



    void FindLaneMain()
    {
        GameObject obj = this.gameObject;
        while (obj.transform.parent)
        {
            obj = obj.transform.parent.gameObject;
            main = obj.GetComponent<LaneMain>();
            if (main) break;
        }
    }
}
