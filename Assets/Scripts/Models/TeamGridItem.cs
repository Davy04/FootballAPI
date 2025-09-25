using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class TeamGridItem : MonoBehaviour
{
    [SerializeField] private Image badgeImage;
    [SerializeField] private Button button; // botão clicável

    private string teamName;

    public void Initialize(Sprite sprite, string teamName, UnityAction<string> onClickCallback)
    {
        this.teamName = teamName;

        if (badgeImage != null)
            badgeImage.sprite = sprite;

        if (button != null)
        {
            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(() => onClickCallback?.Invoke(this.teamName));
        }
        else
        {
            Debug.LogError("Button não atribuído no TeamGridItem!");
        }
    }
}