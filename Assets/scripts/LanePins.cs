using System.Collections.Generic;
using UnityEngine;

public class LanePins : MonoBehaviour
{
    public LaneMain main;
    private List<LanePin> pins;
    private List<LanePin> felledPins;

    public LanePin prefab;
    public int pinRows = 4;
    public float separation = 1f;
    public Vector3 pinSpawnOffset;

    void Start()
    {
        pins = new List<LanePin>();
        felledPins = new List<LanePin>();
        SpawnPins();
    }

    void OnTriggerEnter(Collider other)
    {
        LanePin pin = other.GetComponent<LanePin>();
        if (pin) pins.Add(pin);
    }

    void OnTriggerExit(Collider other)
    {
        LanePin pin = other.GetComponent<LanePin>();
        if (pin)
        {
            if (pins.Remove(pin)) felledPins.Add(pin);
        }
    }

    public bool PinsAreMoving(float threshold)
    {
        foreach(LanePin pin in pins)
        {
            if (pin.rigidbody.linearVelocity.magnitude>threshold) return true;
        }
        return false;
    }

    public int CurrentScore()
    {
        int score = 0;
        foreach(LanePin pin in felledPins) score += pin.value;
        return score;
    }

    public void ClearPins()
    {
        foreach(LanePin pin in pins) Destroy(pin.gameObject);
        pins.Clear();
        foreach(LanePin pin in felledPins) Destroy(pin.gameObject);
        felledPins.Clear();
    }

    public void SpawnPins()
    {
        Vector3 runningPosition = transform.position+pinSpawnOffset;
        int rowWidth = 1;
        for (int i=0; i<pinRows; i++)
        {
            for (int j=0; j<rowWidth; j++)
            {
                LanePin newObj = Instantiate(prefab);
                pins.Add(newObj);
                newObj.transform.position = main.transform.rotation*runningPosition;
                newObj.transform.rotation = main.transform.rotation;
                runningPosition.x += separation;
            }
            runningPosition.x -= separation/2;
            runningPosition.x -= separation*rowWidth;
            runningPosition.z += separation;
            rowWidth += 1;
        }
    }

    public void ResetPins()
    {
        ClearPins();
        SpawnPins();
    }

}