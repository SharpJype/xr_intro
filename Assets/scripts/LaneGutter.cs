using System.Collections;
using Unity.Collections;
using UnityEngine;

public class LaneGutter : MonoBehaviour
{
    private LaneMain main;

    public bool antiGutter = false;

    public float gutterDelay = 0f;
    Coroutine waiter;

    void Start()
    {
        FindLaneMain();
    }

    void GutterInteraction(Collider other)
    {
        LaneReturnable x = other.GetComponent<LaneReturnable>();
        if (x)
        {
            x.targetedByGutter = false;
            if (main.IsReady()) main.StartScoring();
            x.transform.position = x.GetReturnPosition();
            Rigidbody rigidbody = x.GetComponent<Rigidbody>();
            rigidbody.linearVelocity *= 0;
            rigidbody.angularVelocity *= 0;
        }
    }
    void OnTriggerEnter(Collider other)
    {
        
        if (!antiGutter)
        {
            if (gutterDelay>0f) StartDelayedGutter(other);
            else GutterInteraction(other);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (antiGutter) 
        {
            if (gutterDelay>0f) StartDelayedGutter(other);
            else GutterInteraction(other);
        }
        
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


    
    public void StartDelayedGutter(Collider other)
    {
        LaneReturnable x = other.GetComponent<LaneReturnable>();
        if (x)
        {
            if (x.targetedByGutter) return;
            x.targetedByGutter = true;
        }
        else return;
        
        if (waiter!=null) StopCoroutine(waiter); // stop old one if running
        waiter = StartCoroutine(GutterWaiter(other)); // start waiting
    }
    private IEnumerator GutterWaiter(Collider other)
    {
        yield return new WaitForSeconds(gutterDelay);
        LaneReturnable x = other.GetComponent<LaneReturnable>();
        if (x)
        {
            if (x.targetedByGutter) GutterInteraction(other);
        }
    }

}
