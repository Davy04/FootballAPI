using UnityEngine;
using TMPro;
using System.Collections.Generic;
using CSharpAPI.Filters;
using CSharpAPI.Models;

public class LastMatches : MonoBehaviour
{
    [Header("Referências")]
    [SerializeField] private GameObject popupBase;
    [SerializeField] private TextMeshProUGUI homeTeamName;
    [SerializeField] private TextMeshProUGUI awayTeamName;
    [Header("Campos de Resultado")]
    [SerializeField] private TextMeshProUGUI[] homeTeamTexts;  
    [SerializeField] private TextMeshProUGUI[] homeScoreTexts;   
    [SerializeField] private TextMeshProUGUI[] awayTeamTexts;
    [SerializeField] private TextMeshProUGUI[] awayScoreTexts; 
    [SerializeField] private TextMeshProUGUI[] resultStatusTexts; 

    [Header("Configuração")]
    [SerializeField] private int maxMatchesToShow = 5;

    private List<Match> allMatches;

    public void Initialize(List<Match> matches)
    {
        allMatches = matches;
    }
    
    public void OnTeamClicked(string teamType)
    {
        string teamName = teamType == "home" ? homeTeamName.text.Trim() : awayTeamName.text.Trim();
        ShowTeamHistory(teamName);
    }

    private void ShowTeamHistory(string teamName)
    {
        if (allMatches == null || allMatches.Count == 0)
        {
            Debug.LogWarning("Nenhuma partida carregada.");
            return;
        }

        if (string.IsNullOrEmpty(teamName))
        {
            Debug.LogError("Nome do time está vazio.");
            return;
        }

        var last5Matches = LinqFilter.GetLastMatches(allMatches, teamName, maxMatchesToShow);
        DisplayMatchResults(last5Matches, teamName);
        popupBase.SetActive(true);
    }

    
    private void DisplayMatchResults(List<Match> matches, string currentTeam)
    {
        int matchesToShow = Mathf.Min(matches.Count, maxMatchesToShow);
    
        for (int i = 0; i < maxMatchesToShow; i++)
        {
            bool hasMatch = i < matchesToShow;
            
            homeTeamTexts[i].gameObject.SetActive(hasMatch);
            homeScoreTexts[i].gameObject.SetActive(hasMatch);
            awayTeamTexts[i].gameObject.SetActive(hasMatch);
            awayScoreTexts[i].gameObject.SetActive(hasMatch);
            resultStatusTexts[i].gameObject.SetActive(hasMatch);

            if (!hasMatch) continue;

            Match match = matches[i];
            int homeScore = match.Score?.Ft?[0] ?? 0;
            int awayScore = match.Score?.Ft?[1] ?? 0;
            
            homeTeamTexts[i].text = match.HomeTeam;
            homeScoreTexts[i].text = homeScore.ToString();
            awayTeamTexts[i].text = match.AwayTeam;
            awayScoreTexts[i].text = awayScore.ToString();
            
            resultStatusTexts[i].text = GetResultStatus(match, currentTeam);
        }
    }

    private string GetResultStatus(Match match, string currentTeam)
    {
        int homeScore = match.Score?.Ft?[0] ?? 0;
        int awayScore = match.Score?.Ft?[1] ?? 0;
    
        if (match.HomeTeam == currentTeam)
        {
            return homeScore > awayScore ? "V" : 
                homeScore < awayScore ? "D" : "E";
        }
        else
        {
            return awayScore > homeScore ? "V" : 
                awayScore < homeScore ? "D" : "E";
        }
    }
    
    public void ClosePopup()
    {
        popupBase.SetActive(false);
    }
}