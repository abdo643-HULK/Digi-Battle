using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ResultHud: MonoBehaviour {
	public TextMeshProUGUI resultText;
	public Button playAgainButton;
	public Button mainMenuButton;

	private void Start() {
		playAgainButton.onClick.AddListener(OnPlayAgainButtonClicked);
		mainMenuButton.onClick.AddListener(OnMainMenuButtonClicked);
	}

	public void Activate() {
		gameObject.SetActive(true);
	}

	public void SetResultText(string text) {
		resultText.text = text;
	}

	private void OnPlayAgainButtonClicked() {
		SceneManager.LoadScene(GameScene.Battle);
	}

	private void OnMainMenuButtonClicked() {
		SceneManager.LoadScene(GameScene.MainMenu);
	}
}