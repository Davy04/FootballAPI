using System;
using CSharpAPI.Models;
using Newtonsoft.Json;
using UnityEngine;

public class Match
{
    [JsonProperty("team1")]
    public string HomeTeam { get; set; }

    [JsonProperty("team2")]
    public string AwayTeam { get; set; }

    [JsonProperty("score")]
    public Score Score { get; set; }

    [JsonProperty("round")]
    public string Round { get; set; }
    
    public void ShowDetails()
    {
        Debug.Log($"Round: {Round}");
        Debug.Log($"Home Team: {HomeTeam}");
        Debug.Log($"Away Team: {AwayTeam}");
        Debug.Log($"Score: {Score?.Ft[0]} x {Score?.Ft[1]}");
    }
}