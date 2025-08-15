using UnityEngine;
using TMPro;
using System.Collections.Generic;
using CSharpAPI.Filters;
using CSharpAPI.Models;
using Models; // assumindo que seu tipo Match está aqui

public class LastMatches : MonoBehaviour
{
    [Header("Referências de UI")]
    [SerializeField] private GameObject popupBase;
    [SerializeField] private TextMeshProUGUI HomeTeamName;
    [SerializeField] private TextMeshProUGUI AwayTeamName;
    [SerializeField] private Transform groupResults; 
    [SerializeField] private ResultGroupItem resultPrefab;

    [Header("Banco de Escudos")]
    [SerializeField] private BadgeDb badgeDatabase;

    [Header("Configuração")]
    [SerializeField] private int maxMatchesToShow = 5;

    private Dictionary<string, Sprite> badgeDictionary;
    private List<Match> allMatches;

    void Awake()
    {
        // Monta dicionário para buscar escudo pelo nome
        badgeDictionary = new Dictionary<string, Sprite>();
        foreach (var badge in badgeDatabase.badges)
        {
            if (!badgeDictionary.ContainsKey(badge.teamName))
                badgeDictionary.Add(badge.teamName, badge.badgeSprite);
        }
    }

    public void Initialize(List<Match> matches)
    {
        allMatches = matches;
    }
    
    public void OnTeamClicked(string teamType)
    {
        string teamName = teamType == "home" ? HomeTeamName.text.Trim() : AwayTeamName.text.Trim();
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

        // Limpa histórico anterior
        foreach (Transform child in groupResults)
            Destroy(child.gameObject);

        // Pega últimos X jogos do time
        var lastMatches = LinqFilter.GetLastMatches(allMatches, teamName, maxMatchesToShow);

        foreach (var match in lastMatches)
        {
            var item = Instantiate(resultPrefab, groupResults);

            // Busca sprite de cada time
            Sprite homeBadge = GetBadge(match.HomeTeam);
            Sprite awayBadge = GetBadge(match.AwayTeam);

            int homeScore = match.Score?.Ft?[0] ?? 0;
            int awayScore = match.Score?.Ft?[1] ?? 0;

            item.SetData(
                match.HomeTeam,
                homeBadge,
                homeScore,
                match.AwayTeam,
                awayBadge,
                awayScore,
                GetResultStatus(match, teamName)
            );
        }

        popupBase.SetActive(true);
    }

    private Sprite GetBadge(string teamName)
    {
        if (badgeDictionary == null || badgeDictionary.Count == 0)
        {
            badgeDictionary = new Dictionary<string, Sprite>();
            foreach (var badge in badgeDatabase.badges)
            {
                if (!badgeDictionary.ContainsKey(badge.teamName))
                    badgeDictionary.Add(badge.teamName, badge.badgeSprite);
            }
        }

        badgeDictionary.TryGetValue(teamName, out var sprite);
        return sprite;
    }


    private string GetResultStatus(Match match, string currentTeam)
    {
        int homeScore = match.Score?.Ft?[0] ?? 0;
        int awayScore = match.Score?.Ft?[1] ?? 0;
    
        if (match.HomeTeam == currentTeam)
            return homeScore > awayScore ? "V" : homeScore < awayScore ? "D" : "E";
        else
            return awayScore > homeScore ? "V" : awayScore < homeScore ? "D" : "E";
    }
    
    public void ClosePopup()
    {
        popupBase.SetActive(false);
    }
}