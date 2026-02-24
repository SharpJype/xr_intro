using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LaneScoreText : MonoBehaviour
{

    TextMeshProUGUI tmp;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        tmp = GetComponent<TextMeshProUGUI>();
    }

    public void UpdateScores(List<int> scores, List<int> results)
    {
        string s = "";
        string ss;
        //foreach(int score in scores) s += string.Format("[{0,2}] ", score);
        for(int i=0; i<scores.Count; i++)
        {
            ss = string.Format("{0,2}", scores[i]);
            if (results[i]==2) ss = $"{ss}**";
            else if (results[i]==1) ss = $"{ss}*";
            s += $"[{ss,-4}] ";
        }
        //s += string.Format("\n({0})", current);
        tmp.text = s;
    }

    public void ShowEndResults(List<int> scores, List<int> results)
    {
        string s = "";
        int scoreTotal = 0;
        for(int i=0; i<scores.Count; i++)
        {
            scoreTotal += scores[i];
            s += string.Format("{1}. [{0}", scores[i], i+1);
            if (results[i]==2) s += " - strike!";
            else if (results[i]==1) s += " - spare";
            s += "]\n";
        }
        s += $"= {scoreTotal}";
        tmp.text = s;
    }


    public void SetText(string s)
    {
        tmp.text = s;
    }

}
