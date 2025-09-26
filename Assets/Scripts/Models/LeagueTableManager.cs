using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;
using CSharpAPI.Filters;
using CSharpAPI.Models;
using TMPro;

public class LeagueTableManager : MonoBehaviour
{
    [Header("Referências de UI")]
    [SerializeField] private GameObject leagueTablePopup;
    [SerializeField] private Transform tableContainer;
    [SerializeField] private GameObject teamRowPrefab;

    [Header("Dados")]
    [SerializeField] private MatchManager matchManager;
    [SerializeField] private BadgeDb badgeDb;
    
    [Header("Rodadas")]
    [SerializeField] private TMP_Dropdown roundDropdown;


    private Dictionary<string, TeamStats> teamStatsDict = new();

    private void Start()
    {
        if (roundDropdown != null)
        {
            roundDropdown.ClearOptions();
            List<string> options = new List<string>();
            for (int i = 1; i <= 38; i++)
                options.Add($"Rodada {i}");

            roundDropdown.AddOptions(options);
            roundDropdown.onValueChanged.AddListener(OnRoundChanged);

            roundDropdown.value = 37;
            roundDropdown.RefreshShownValue();

            ShowLeagueTable(round: 38);
        }
    }


    private void OnRoundChanged(int dropdownIndex)
    {
        int round = dropdownIndex + 1;
        ShowLeagueTable(round);
    }

    
    public void ShowLeagueTable(int round)
    {
        if (matchManager == null)
        {
            Debug.LogError("MatchManager não atribuído!");
            return;
        }

        ClearTable();

        List<Match> allMatches = matchManager.GetAllMatches();
        List<Match> filteredMatches = allMatches
            .Where(m => LinqFilter.ExtractRoundNumber(m.Round) <= round)
            .ToList();

        if (filteredMatches.Count == 0)
        {
            Debug.LogWarning($"Nenhuma partida encontrada até a rodada {round}.");
            return;
        }

        teamStatsDict.Clear();

        foreach (var match in filteredMatches)
        {
            if (match.Score?.Ft == null || match.Score.Ft.Count < 2)
                continue;

            int homeGoals = match.Score.Ft[0];
            int awayGoals = match.Score.Ft[1];

            AddOrUpdateTeamStats(match.HomeTeam, homeGoals, awayGoals);
            AddOrUpdateTeamStats(match.AwayTeam, awayGoals, homeGoals);
        }

        int position = 1;
        foreach (var kvp in teamStatsDict
                     .OrderByDescending(e => e.Value.Points)
                     .ThenByDescending(e => e.Value.GoalDifference))
        {
            string teamName = kvp.Key;
            TeamStats stats = kvp.Value;

            Sprite badge = null;
            var badgeEntry = badgeDb.badges.Find(b => b.teamName.ToLower() == teamName.ToLower());
            if (badgeEntry != null)
                badge = badgeEntry.badgeSprite;

            GameObject row = Instantiate(teamRowPrefab, tableContainer);
            GroupTeam groupTeam = row.GetComponent<GroupTeam>();

            if (groupTeam != null)
            {
                groupTeam.SetData(
                    position,
                    teamName,
                    stats.Played,
                    stats.Points,
                    stats.GoalDifference,
                    badge
                );
            }

            position++;
        }

        leagueTablePopup.SetActive(true);
    }


    private void AddOrUpdateTeamStats(string teamName, int goalsFor, int goalsAgainst)
    {
        if (!teamStatsDict.ContainsKey(teamName))
        {
            teamStatsDict[teamName] = new TeamStats();
        }

        var stats = teamStatsDict[teamName];
        stats.Played++;
        stats.GoalsFor += goalsFor;
        stats.GoalsAgainst += goalsAgainst;

        if (goalsFor > goalsAgainst)
            stats.Points += 3;
        else if (goalsFor == goalsAgainst)
            stats.Points += 1;

        stats.GoalDifference = stats.GoalsFor - stats.GoalsAgainst;
    }

    private void ClearTable()
    {
        foreach (Transform child in tableContainer)
        {
            Destroy(child.gameObject);
        }
    }
}

public class TeamStats
{
    public int Played;
    public int Points;
    public int GoalsFor;
    public int GoalsAgainst;
    public int GoalDifference;
}
