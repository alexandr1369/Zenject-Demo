using UnityEngine;

public class ToggleButton : InteractableButton
{
    private GameManager _gameManager;

    [SerializeField]
    private GameObject panel; // panel to toggle
    [SerializeField]
    private bool hasPauseToggle = false; // has permission to toggle game pause

    [Zenject.Inject]
    public void Construct(GameManager gameManager)
    {
        _gameManager = gameManager;
    }

    protected override void Perform()
    {
        // toggle game state
        if (hasPauseToggle)
            _gameManager.ToggleGameState();

        // toggle panel
        panel.SetActive(!panel.activeSelf);
    }
}
