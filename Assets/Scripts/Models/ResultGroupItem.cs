using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Models
{
    public class ResultGroupItem : MonoBehaviour
    {
        [Header("Referências")]
        public TextMeshProUGUI homeTeamText;
        public TextMeshProUGUI awayTeamText;
        public Image homeTeamLogo;
        public Image awayTeamLogo;
        public TextMeshProUGUI homeScoreText;
        public TextMeshProUGUI awayScoreText;
        public TextMeshProUGUI resultStatusText;
        
        [Header("UI References")]
        public Image backgroundImage;
        [SerializeField] private Color victoryColor;
        [SerializeField] private Color drawColor;
        [SerializeField] private Color loseColor;
        

        public void SetData(string homeTeam, Sprite homeLogo, int homeScore,
            string awayTeam, Sprite awayLogo, int awayScore,
            string resultStatus)
        {
            homeTeamText.text = homeTeam;
            homeTeamLogo.sprite = homeLogo;
            homeScoreText.text = homeScore.ToString();

            awayTeamText.text = awayTeam;
            awayTeamLogo.sprite = awayLogo;
            awayScoreText.text = awayScore.ToString();

            resultStatusText.text = resultStatus;
            ApplyBackgroundColor(resultStatus);
        }
        
        private void ApplyBackgroundColor(string resultStatus)
        {
            if (backgroundImage == null) return;

            switch (resultStatus)
            {
                case "V":
                    backgroundImage.color = victoryColor;
                    break;
                case "D":
                    backgroundImage.color = loseColor;
                    break;
                case "E":
                    backgroundImage.color = drawColor;
                    break;
                default:
                    backgroundImage.color = Color.white;
                    break;
            }
        }
    }
}