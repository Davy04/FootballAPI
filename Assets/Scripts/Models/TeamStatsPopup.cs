using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;
using CSharpAPI.Models;

public class TeamStatsPopup : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI teamNameText;
    [SerializeField] private Image teamBadgeImage;
    
    [Header("Aproveitamento")]
    [SerializeField] private TextMeshProUGUI totalPerformanceText;
    [SerializeField] private TextMeshProUGUI homePerformanceText;
    [SerializeField] private TextMeshProUGUI awayPerformanceText;
    
    [Header("Estatísticas")]
    [SerializeField] private TextMeshProUGUI victoriesText;
    [SerializeField] private TextMeshProUGUI drawsText;
    [SerializeField] private TextMeshProUGUI defeatsText;
    
    [Header("Gols")]
    [SerializeField] private TextMeshProUGUI goalsScoredText;
    [SerializeField] private TextMeshProUGUI goalsConcededText;
    [SerializeField] private TextMeshProUGUI cleanSheetsText;

    public void SetTeamInfo(string teamName, Sprite badge, List<Match> allMatches)
    {
        teamNameText.text = teamName;
        teamBadgeImage.sprite = badge;

        int wins = 0, draws = 0, losses = 0;
        int goalsScored = 0, goalsConceded = 0, cleanSheets = 0;

        int homePoints = 0, awayPoints = 0;
        int homeGames = 0, awayGames = 0;

        foreach (var match in allMatches)
        {
            bool isHome = match.HomeTeam.Equals(teamName, System.StringComparison.OrdinalIgnoreCase);
            bool isAway = match.AwayTeam.Equals(teamName, System.StringComparison.OrdinalIgnoreCase);

            if (!isHome && !isAway)
                continue;

            if (match.Score?.Ft == null || match.Score.Ft.Count < 2)
                continue;

            int goalsFor = isHome ? match.Score.Ft[0] : match.Score.Ft[1];
            int goalsAgainst = isHome ? match.Score.Ft[1] : match.Score.Ft[0];
            
            goalsScored += goalsFor;
            goalsConceded += goalsAgainst;

            if (goalsAgainst == 0)
                cleanSheets++;
            
            int points = 0;
            if (goalsFor > goalsAgainst)
            {
                wins++;
                points = 3;
            }
            else if (goalsFor == goalsAgainst)
            {
                draws++;
                points = 1;
            }
            else
            {
                losses++;
                points = 0;
            }
            
            if (isHome)
            {
                homePoints += points;
                homeGames++;
            }
            else if (isAway)
            {
                awayPoints += points;
                awayGames++;
            }
        }
        
        victoriesText.text = wins.ToString();
        drawsText.text = draws.ToString();
        defeatsText.text = losses.ToString();

        float homePct = homeGames > 0 ? (homePoints / (homeGames * 3f)) * 100f : 0f;
        float awayPct = awayGames > 0 ? (awayPoints / (awayGames * 3f)) * 100f : 0f;
        int totalPoints = homePoints + awayPoints;
        int totalGames = homeGames + awayGames;
        float totalPct = totalGames > 0 ? (totalPoints / (totalGames * 3f)) * 100f : 0f;

        totalPerformanceText.text = $"{totalPct:0.0}%";
        homePerformanceText.text = $"{homePct:0.0}%";
        awayPerformanceText.text = $"{awayPct:0.0}%";
        
        goalsScoredText.text = goalsScored.ToString();
        goalsConcededText.text = goalsConceded.ToString();
        cleanSheetsText.text = cleanSheets.ToString();
    }
}
