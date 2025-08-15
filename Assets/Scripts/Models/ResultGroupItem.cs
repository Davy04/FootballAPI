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
        }
    }
}