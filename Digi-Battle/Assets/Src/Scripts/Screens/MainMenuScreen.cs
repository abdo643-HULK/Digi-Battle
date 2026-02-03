using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuScreen : MonoBehaviour {
	public Button playerVsPlayerButton;
	public Button playerVsCpuButton;

	public Button exitButton;

	void Awake() {
		gameObject.AddComponent<DigiBattle.Screen>();
	}

	// Start is called before the first frame update
	void Start() {
		playerVsPlayerButton.interactable = false;
		playerVsCpuButton.onClick.AddListener(() => {
			Debug.Log("Player vs CPU");
			exitButton.gameObject.SetActive(false);
			playerVsCpuButton.gameObject.SetActive(false);
			playerVsPlayerButton.gameObject.SetActive(false);
			GameManager.Instance.gameMode = GameMode.SinglePlayer;
			SceneManager.LoadScene(GameScene.DigimonSelection);
		});
		exitButton.onClick.AddListener(() => Application.Quit());
	}
}
