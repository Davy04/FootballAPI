using UnityEngine;
using UnityEngine.UI;

public class TeamGridItem : MonoBehaviour
{
    [SerializeField] private Image badgeImage; // arrasta a imagem do escudo no Inspector

    public void SetBadge(Sprite sprite)
    {
        if (badgeImage != null)
        {
            badgeImage.sprite = sprite;
        }
        else
        {
            Debug.LogError("BadgeImage não atribuído no TeamGridItem!");
        }
    }
}