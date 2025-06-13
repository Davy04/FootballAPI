using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using TMPro;
using Random = UnityEngine.Random;

public class MatchManager : MonoBehaviour
{
    private List<Match> allMatches;
    [SerializeField] private BadgeDb database;
    [SerializeField] private Image homeTeam;
    [SerializeField] private Image awayTeam;
    [SerializeField] private TMP_Text homeTeamTXT;
    [SerializeField] private TMP_Text awayTeamTXT;
    [SerializeField] private TMP_Text homeScoreTXT;
    [SerializeField] private TMP_Text awayScoreTXT;
    [SerializeField] private TMP_Text roundTXT;

    public void SetMatches(List<Match> matches)
    {
        allMatches = matches;
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ShowRandomMatch();
        }
        
    }

    public void ShowRandomMatch()
    {
        if (allMatches == null || allMatches.Count == 0)
        {
            Debug.LogWarning("No matches available in MatchManager.");
            return;
        }

        int index = Random.Range(0, allMatches.Count);
        Match match = allMatches[index];

        UpdateMatch(match);
    }

    
    public void UpdateMatch(Match match)
    {
        SetTeamData(homeTeam,homeTeamTXT, match.HomeTeam);
        SetTeamData(awayTeam,awayTeamTXT, match.AwayTeam);
        SetRound(match.Round);
        SetScore(match.Score.Ft);
        match.ShowDetails();
    }

    private void SetTeamData(Image image, TMP_Text text, string teamName)
    {
        var Badge = database.badges.FirstOrDefault(e => e.teamName.ToLower() == teamName.ToLower());
        if (Badge != null)
        {
            image.sprite = Badge.badgeSprite;
            text.text = teamName;
        }
        else
        {
            Debug.LogWarning("ta errado");
        }
    }

    private void SetRound(string round)
    {
        if (!string.IsNullOrEmpty(round))
        {
            roundTXT.text = $"{round}";
        }
        else
        {
            roundTXT.text = "Round: N/A";
            Debug.LogWarning("No round data available.");
        }
    }
    private void SetScore(List<int> score)
    {
        if (score != null && score.Count>=2)
        {
            homeScoreTXT.text = score[0].ToString();
            awayScoreTXT.text = score[1].ToString();
        }
        else
        {
            Debug.LogWarning("ta errado dnv");
        }
    }
}