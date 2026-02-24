using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaneMain : MonoBehaviour
{
    public enum State {Ready=0,Score,Scoring,Cleaning,Reloading,ReloadDone};
    public State laneState = State.Ready;
    
    private List<int> frameScores;
    private int frameScore = 0; // points scored this frame
    private int frameThrowsLeft = 2; // throws left for this frame
    private List<int> frameResults; // 0/1/2 == open/spare/strike

    public int gameLength = 5;
    public int attemptsPerFrame = 2;

    private float movementThreshold = 0.2f;

    private LanePins pins;
    public float intervalSeconds = 3f;
    Coroutine waiter;

    public LaneScoreText scoreText;
    public LaneScoreText resultText;
    public LaneScoreText currentScoreText;

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
            if (pins.IsEmpty()) StartReloading();
            else if (movingPins) laneState = State.Score;
        }
        else if (laneState==State.Score)
        {
            if (!movingPins) StartScoring();
        }
        else if (laneState==State.ReloadDone)
        {
            if (!movingPins)
            {
                CheckGameFinish();
                laneState = State.Ready;
            }
        }
    }

    public bool IsReady()
    {
        return laneState==State.Ready;
    }

    private void CheckGameFinish()
    {
        if (frameResults.Count>=gameLength)
        {
            resultText.ShowEndResults(frameScores, frameResults);
            ResetGame();
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
        scoreText.UpdateScores(frameScores, frameResults);
        yield return new WaitForSeconds(intervalSeconds);
        
        frameScore += pins.CurrentScore();
        frameThrowsLeft -= 1;
        
        if (pins.RemainingScore()==0) // strike or spare
        {
            bool doExtraThrow = (frameScores.Count>0) & (frameScores.Count%10)==0;
            doExtraThrow &= frameThrowsLeft>0;

            if (frameThrowsLeft<(attemptsPerFrame-1)) NextFrame(1); // spare
            else NextFrame(2); // strike
            /*
            if (doExtraThrow)
            {
                StartReloading();
                frameThrowsLeft += 1; // extra throw
            }
            else NextFrame();
            */
        }
        else if (frameThrowsLeft>0) StartCleaning();
        else NextFrame(0); // open frame
        
        currentScoreText.SetText($"{frameScore}");
        scoreText.UpdateScores(frameScores, frameResults);
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


    public void NextFrame(int result=0)
    {
        frameResults.Add(result);
        frameScores.Add(frameScore);
        ExtraStrikeSpareScoring();
        ResetFrame();
    }

    private void ResetFrame()
    {
        StartReloading();
        frameScore = 0;
        currentScoreText.SetText("-");
        frameThrowsLeft = attemptsPerFrame;
    }

    void ExtraStrikeSpareScoring()
    {
        if (frameScores.Count>2) // strike score bonus to two turns before
        {
            if (frameResults[frameResults.Count-3]==2) frameScores[frameScores.Count-3] += frameScore;
        }
        if (frameScores.Count>1) // strike or spare score bonus to one turn before
        {
            if (frameResults[frameResults.Count-2]!=0) frameScores[frameScores.Count-2] += frameScore;
        }
    }

    public void ResetGame()
    {
        frameScores.Clear();
        frameResults.Clear();
        ResetFrame();
    }


}
