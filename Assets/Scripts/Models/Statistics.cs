using UnityEngine;
using System.Collections.Generic;

public class Statistics : MonoBehaviour
{
    [Header("Referências de UI")]
    [SerializeField] private GameObject popupBase;       // painel único
    [SerializeField] private Transform gridContainer;    // onde os escudos serão instanciados
    [SerializeField] private GameObject teamPrefab;      // prefab de 1 item (tem TeamGridItem)

    [Header("Banco de Escudos")]
    [SerializeField] private BadgeDb badgeDatabase;

    private Dictionary<string, Sprite> badgeDictionary;

    void Awake()
    {
        // monta o dicionário de escudos
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

        Debug.Log($"Total de escudos encontrados: {badgeDictionary.Count}");

        // limpa grid
        foreach (Transform child in gridContainer)
            Destroy(child.gameObject);

        // popula com escudos
        foreach (var kvp in badgeDictionary)
        {
            var item = Instantiate(teamPrefab, gridContainer);

            // usa o script TeamGridItem
            var teamItem = item.GetComponent<TeamGridItem>();
            if (teamItem == null)
            {
                Debug.LogError("O teamPrefab não possui o script TeamGridItem!");
                continue;
            }

            teamItem.SetBadge(kvp.Value); // define apenas o escudo
            Debug.Log($"Instanciado escudo do time: {kvp.Key}");
        }

        // exibe popup
        popupBase.SetActive(true);
    }

    public void ClosePopup()
    {
        popupBase.SetActive(false);
    }
}