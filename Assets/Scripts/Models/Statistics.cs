using System.Collections.Generic;
using UnityEngine;

public class Statistics : MonoBehaviour
{
    [Header("Referências de UI")]
    [SerializeField] private GameObject popupBase;
    [SerializeField] private Transform gridContainer;
    [SerializeField] private GameObject teamPrefab;

    [Header("Banco de Escudos")]
    [SerializeField] private BadgeDb badgeDatabase;

    [Header("Popup de Detalhes")]
    [SerializeField] private GameObject teamDetailPopup;

    private Dictionary<string, Sprite> badgeDictionary;

    void Awake()
    {
        badgeDictionary = new Dictionary<string, Sprite>();
        foreach (var badge in badgeDatabase.badges)
        {
            if (!badgeDictionary.ContainsKey(badge.teamName))
                badgeDictionary.Add(badge.teamName, badge.badgeSprite);
        }
    }

    public void ShowPopup()
    {
        if (badgeDictionary == null || badgeDictionary.Count == 0)
        {
            Debug.LogWarning("Nenhum escudo carregado no BadgeDb.");
            return;
        }

        foreach (Transform child in gridContainer)
            Destroy(child.gameObject);

        foreach (var kvp in badgeDictionary)
        {
            var item = Instantiate(teamPrefab, gridContainer);
            var teamItem = item.GetComponent<TeamGridItem>();
            if (teamItem == null)
            {
                Debug.LogError("O teamPrefab não possui o script TeamGridItem!");
                continue;
            }

            teamItem.Initialize(kvp.Value, kvp.Key, OnTeamClicked);
        }
        
        popupBase.SetActive(true);
    }

    private void OnTeamClicked(string teamName)
    {
        Debug.Log($"Time clicado: {teamName}");

        if (!badgeDictionary.ContainsKey(teamName))
        {
            Debug.LogError("Escudo não encontrado!");
            return;
        }

        // Aqui você pode popular os dados do popup de detalhes
        popupBase.SetActive(false);
        teamDetailPopup.SetActive(true);
    }

    public void ClosePopup()
    {
        popupBase.SetActive(false);
    }

    public void CloseTeamDetailPopup()
    {
        teamDetailPopup.SetActive(false);
    }
}
