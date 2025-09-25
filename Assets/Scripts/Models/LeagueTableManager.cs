using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;
using CSharpAPI.Models;

public class LeagueTableManager : MonoBehaviour
{
    [Header("Referências de UI")]
    [SerializeField] private GameObject leagueTablePopup;
    [SerializeField] private Transform tableContainer; // Container da tabela
    [SerializeField] private GameObject teamRowPrefab; // Prefab do GroupTeam

    [Header("Dados")]
    [SerializeField] private MatchManager matchManager;
    [SerializeField] private BadgeDb badgeDb;

    private Dictionary<string, TeamStats> teamStatsDict = new();

    public void ShowLeagueTable()
    {
        if (matchManager == null)
        {
            Debug.LogError("MatchManager não atribuído!");
            return;
        }

        ClearTable();

        List<Match> allMatches = matchManager.GetAllMatches();
        if (allMatches == null || allMatches.Count == 0)
        {
            Debug.LogWarning("Nenhuma partida disponível para gerar tabela.");
            return;
        }

        teamStatsDict.Clear();

        // Calcula os dados por time
        foreach (var match in allMatches)
        {
            if (match.Score?.Ft == null || match.Score.Ft.Count < 2)
                continue;

            int homeGoals = match.Score.Ft[0];
            int awayGoals = match.Score.Ft[1];

            AddOrUpdateTeamStats(match.HomeTeam, homeGoals, awayGoals);
            AddOrUpdateTeamStats(match.AwayTeam, awayGoals, homeGoals);
        }

        // Ordena e instancia visualmente
        int position = 1;
        foreach (var kvp in teamStatsDict
            .OrderByDescending(e => e.Value.Points)
            .ThenByDescending(e => e.Value.GoalDifference))
        {
            string teamName = kvp.Key;
            TeamStats stats = kvp.Value;

            // Busca escudo
            Sprite badge = null;
            var badgeEntry = badgeDb.badges.Find(b => b.teamName.ToLower() == teamName.ToLower());
            if (badgeEntry != null)
                badge = badgeEntry.badgeSprite;

            // Instancia prefab
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
