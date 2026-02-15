using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaneMain : MonoBehaviour
{
    public enum State {Ready=0,Score,Scoring,Reload,Reloading,ReloadDone};
    public State laneState;
    

    private List<int> scores;

    private float movementThreshold = 0.1f;

    public LanePins pins;
    public float intervalSeconds = 3f;
    Coroutine waiter;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        scores = new List<int>();
    }

    // Update is called once per frame
    void Update()
    {
        bool movingPins = pins.PinsAreMoving(movementThreshold);
        if (laneState==State.Ready)
        {
            if (movingPins) laneState = State.Score;
        }
        else if (laneState==State.Score)
        {
            if (!movingPins) StartScoring();
        }
        else if (laneState==State.Reload) StartReloading();
        else if (laneState==State.ReloadDone)
        {
            if (!movingPins) laneState = State.Ready;
        }
    }

    public bool IsReady()
    {
        return laneState==State.Ready;
    }


    public void StartScoring()
    {
        laneState = State.Scoring;
        if (waiter!=null) StopCoroutine(waiter); // stop old one if running
        waiter = StartCoroutine(ScoringWaiter()); // start waiting
    }

    private IEnumerator ScoringWaiter()
    {
        print("scoring");
        yield return new WaitForSeconds(intervalSeconds);
        
        scores.Add(pins.CurrentScore());
        if (scores[scores.Count-1]==0) laneState = State.Ready;
        else laneState = State.Reload;
        print("scored");
        print(scores[scores.Count-1]);
    }


    public void StartReloading()
    {
        laneState = State.Reloading;
        if (waiter!=null) StopCoroutine(waiter); // stop old one if running
        waiter = StartCoroutine(ReloadingWaiter()); // start waiting
    }
    private IEnumerator ReloadingWaiter()
    {
        print("reloading");
        pins.ResetPins();
        yield return new WaitForSeconds(1f);
        laneState = State.ReloadDone;
    }

}
