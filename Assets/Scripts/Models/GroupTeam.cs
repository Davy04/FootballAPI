using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class GroupTeam : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private TextMeshProUGUI positionText;
    [SerializeField] private Image badgeImage;
    [SerializeField] private TextMeshProUGUI teamNameText;
    [SerializeField] private TextMeshProUGUI playedText;
    [SerializeField] private TextMeshProUGUI pointsText;
    [SerializeField] private TextMeshProUGUI goalDiffText;
    [SerializeField] private Image backgroundImage;

    [Header("Cores por posição")]
    [SerializeField] private Color championsColor; 
    [SerializeField] private Color europaColor; 
    [SerializeField] private Color conferenceColor; 
    [SerializeField] private Color relegationColor; 
    [SerializeField] private Color defaultColor;     

    public void SetData(int position, string teamName, int played, int points, int goalDifference, Sprite badge)
    {
        if (positionText != null)
            positionText.text = position.ToString();

        if (badgeImage != null)
            badgeImage.sprite = badge;

        teamNameText.text = teamName;
        playedText.text = played.ToString();
        pointsText.text = points.ToString();
        goalDiffText.text = goalDifference.ToString("+#;-#;0");

        if (backgroundImage != null)
        {
            backgroundImage.color = GetColorForPosition(position);
        }
    }

    private Color GetColorForPosition(int pos)
    {
        if (pos >= 1 && pos <= 4) return championsColor;
        if (pos == 5) return europaColor;
        if (pos == 6) return conferenceColor;
        if (pos >= 18) return relegationColor;
        return defaultColor;
    }
}