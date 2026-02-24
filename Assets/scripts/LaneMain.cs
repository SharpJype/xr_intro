using System;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;

public class LaneMain : MonoBehaviour
{
    public enum State {Ready=0,Score,Scoring,Cleaning,Reload,Reloading,ReloadDone};
    public State laneState = State.Reload;
    
    private List<int> frameScores;
    private int frameScore = 0; // points scored this frame
    private int frameThrowsLeft = 2; // throws left for this frame
    private List<int> frameResults; // 0/1/2 == open/spare/strike

    public int gameLength = 5;

    private float movementThreshold = 0.5f;

    private LanePins pins;
    public float intervalSeconds = 3f;
    Coroutine waiter;

    public LaneScoreText scoreText;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        frameScores = new List<int>();
        frameResults = new List<int>();
        
        foreach(LanePins c in GetComponentsInChildren<LanePins>())
        {
            if (c != this)
                pins = c;
        }
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
            if (!movingPins)
            {
                BeforeReady();
                laneState = State.Ready;
            }
        }
    }

    public bool IsReady()
    {
        return laneState==State.Ready;
    }

    private void BeforeReady()
    {
        if (frameResults.Count>=gameLength)
        {
            // show end results & reset game
            string s = "";
            for(int i=0; i<frameResults.Count; i++)
            {
                s += string.Format("{0}. [{1} - {2}]\n", i+1, frameScores[i], frameResults[i]);
            }
            //print(s);
            scoreText.ShowEndResults(frameScores, frameResults);
            GameReset();
        }
    }


    public void StartScoring()
    {
        laneState = State.Scoring;
        if (waiter!=null) StopCoroutine(waiter); // stop old one if running
        waiter = StartCoroutine(ScoringWaiter()); // start waiting
    }

    private IEnumerator ScoringWaiter()
    {
        //print("scoring");
        scoreText.UpdateScores(frameScores, frameScore);
        yield return new WaitForSeconds(intervalSeconds);
        
        int score = pins.CurrentScore();
        frameScore += score;
        frameThrowsLeft -= 1;
        
        if (pins.RemainingScore()==0) // strike or spare
        {
            bool doExtraThrow = (frameScores.Count>0) & (frameScores.Count%10)==0;
            doExtraThrow &= frameThrowsLeft>0;

            if (frameThrowsLeft>0) frameResults.Add(1); // spare
            else frameResults.Add(2); // strike
            /*
            if (doExtraThrow)
            {
                StartReloading();
                frameThrowsLeft += 1; // extra throw
            }
            else NextFrame();
            */
            NextFrame();
        }
        else if (frameThrowsLeft>0) StartCleaning();
        else // open frame
        {
            frameResults.Add(0);
            NextFrame();
        }

        
        scoreText.UpdateScores(frameScores, frameScore);
    }


    public void StartReloading()
    {
        laneState = State.Reloading;
        if (waiter!=null) StopCoroutine(waiter); // stop old one if running
        waiter = StartCoroutine(ReloadingWaiter()); // start waiting
    }
    private IEnumerator ReloadingWaiter()
    {
        //print("reloading");
        pins.ClearPins();
        yield return new WaitForSeconds(intervalSeconds);
        pins.SpawnPins();
        yield return new WaitForSeconds(1f);
        laneState = State.ReloadDone;
    }


    public void StartCleaning()
    {
        laneState = State.Cleaning;
        if (waiter!=null) StopCoroutine(waiter); // stop old one if running
        waiter = StartCoroutine(CleaningWaiter()); // start waiting
    }
    private IEnumerator CleaningWaiter()
    {
        pins.ClearFelledPins();
        yield return new WaitForSeconds(intervalSeconds);
        laneState = State.ReloadDone;
    }


    public void NextFrame()
    {
        StartReloading();
        frameScores.Add(frameScore);

        if (frameScores.Count>2) // strike score bonus to two turns before
        {
            if (frameResults[frameResults.Count-3]==2) frameScores[frameScores.Count-3] += frameScore;
        }
        if (frameScores.Count>1) // strike or spare score bonus to one turn before
        {
            if (frameResults[frameResults.Count-2]!=0) frameScores[frameScores.Count-2] += frameScore;
        }

        frameScore = 0;
        frameThrowsLeft = 2;
    }

    /*void ExtraFrame()
    {
        StartReloading();
        if (frameThrowsLeft>1) frameThrowsLeft = 1;
        else frameThrowsLeft = 2;
        frameScore = 0;
    }*/

    public void GameReset()
    {
        StartReloading();
        frameScores.Clear();
        frameResults.Clear();
        frameScore = 0;
        frameThrowsLeft = 2;
    }


}
