using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CSharpAPI.Models;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    [FormerlySerializedAs("badgeManager")] [SerializeField] private MatchManager matchManager;
    [SerializeField] private APIManager apiManager;

    private void Start()
    {
        StartCoroutine(GetData());
    }

    IEnumerator GetData()
    {
        yield return apiManager.LoadMatchData();
        ReceiveMatchData(apiManager.Root.Matches);
    }

    public void ReceiveMatchData(List<Match> matches)
    {
        if (matches == null || matches.Count == 0)
        {
            Debug.LogWarning("No matches received.");
            return;
        }

        matchManager.SetMatches(matches);
        matchManager.ShowRandomMatch();
    }
}