using UnityEngine;
using UnityEngine.UI;

public class ToggleGameModeButton : InteractableButton
{
    private GameManager _gameManager;
    private Image _btnImage;

    public GameDifficulty gameDifficulty;

    [Zenject.Inject]
    public void Construct(GameManager gameManager)
    {
        _gameManager = gameManager;
    }

    protected override void Init()
    {
        _btnImage = GetComponent<Image>();
        if (_gameManager.GameDifficulty == gameDifficulty)
        {
            Color32 color = new Color32(0, 0, 0, 128);
            _btnImage.color = color;

            isInteractable = false;
            isAnimated = false;
        }
    }
    protected override void Perform()
    {
        _gameManager.ChangeGameDifficulty(gameDifficulty);
    }
}
