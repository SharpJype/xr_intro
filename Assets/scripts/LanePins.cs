using System.Collections.Generic;
using UnityEngine;

public class LanePins : MonoBehaviour
{
    private LaneMain main;
    private List<LanePin> pins;

    public LanePin prefab;
    public int pinRows = 4;
    public float separation = 1f;
    public Vector3 pinSpawnOffset;

    void Start()
    {
        FindLaneMain();
        pins = new List<LanePin>();
    }

    void OnTriggerEnter(Collider other)
    {
        LanePin pin = other.GetComponent<LanePin>();
        if (pin) pin.felled = false;
    }

    void OnTriggerExit(Collider other)
    {
        LanePin pin = other.GetComponent<LanePin>();
        if (pin) pin.felled = true;
    }

    public bool PinsAreMoving(float threshold)
    {
        foreach(LanePin pin in pins)
        {
            if (pin.felled) continue; // ignore out of bounds pins
            if (pin.rb.linearVelocity.magnitude>threshold) return true;
        }
        return false;
    }

    public int CurrentScore()
    {
        int score = 0;
        foreach(LanePin pin in pins) 
        {
            if (pin.felled) score += pin.points;
        }
        return score;
    }

    public int RemainingScore()
    {
        int score = 0;
        foreach(LanePin pin in pins)
        {
            if (!pin.felled) score += pin.points;
        }
        return score;
    }

    public void ClearPins()
    {
        foreach(LanePin pin in pins) Destroy(pin.gameObject);
        pins.Clear();
    }
    public void ClearFelledPins()
    {
        List<LanePin> newPins = new List<LanePin>();
        foreach(LanePin pin in pins)
        {
            if (pin.felled) Destroy(pin.gameObject);
            else newPins.Add(pin);
        }
        pins = newPins;
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
                newObj.transform.parent = this.transform;
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

    public bool IsEmpty()
    {
        return pins.Count==0;
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