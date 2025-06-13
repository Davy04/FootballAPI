using UnityEngine;
using UnityEngine.UI;

public class TeamBorderManager : MonoBehaviour
{
    [System.Serializable]
    public class TeamData
    {
        public Image teamLogo;
        public int goals;
    }
    
    public TeamData[] teams;
    public Color goldenBorderColor = new Color(1f, 0.843f, 0f, 1f);
    public float borderWidth = 0.05f;
    
    private Material borderMaterial;
    
    void Start()
    {
        // Carrega o material do shader
        borderMaterial = new Material(Shader.Find("Custom/AlphaBorder"));
        borderMaterial.SetColor("_BorderColor", goldenBorderColor);
        borderMaterial.SetFloat("_BorderWidth", borderWidth);
        
        // Exemplo: processa resultados
        ProcessMatchResults();
    }
    
    public void ProcessMatchResults()
    {
        // 1. Encontra o time vencedor
        TeamData winner = null;
        bool isDraw = false;
        
        if (teams.Length >= 2)
        {
            if (teams[0].goals > teams[1].goals)
            {
                winner = teams[0];
            }
            else if (teams[1].goals > teams[0].goals)
            {
                winner = teams[1];
            }
            else
            {
                isDraw = true;
            }
        }
        
        // 2. Aplica borda ao vencedor
        foreach (TeamData team in teams)
        {
            if (team.teamLogo != null)
            {
                if (team == winner && !isDraw)
                {
                    ApplyGoldenBorder(team.teamLogo);
                }
                else
                {
                    RemoveBorder(team.teamLogo);
                }
            }
        }
    }
    
    private void ApplyGoldenBorder(Image teamLogo)
    {
        teamLogo.material = borderMaterial;
        teamLogo.material.SetFloat("_BorderWidth", borderWidth);
    }
    
    private void RemoveBorder(Image teamLogo)
    {
        teamLogo.material = null; // Volta ao material padrão
    }
    
    // Chamar esta função quando os resultados mudarem
    public void UpdateResults(int[] newGoals)
    {
        for (int i = 0; i < Mathf.Min(teams.Length, newGoals.Length); i++)
        {
            teams[i].goals = newGoals[i];
        }
        ProcessMatchResults();
    }
}