using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;
using Vuforia;



#nullable enable

public class DigimonSelectionScreen : MonoBehaviour {
	[SerializeField]
	ARSession? arSession = null;

	public DigimonSelectionSystem digimonSelectionSystem;

	public Button startCpuCharacterSelectionButton;
	
	[SerializeField]
	private TextMeshProUGUI loadingBar;

	private Player? playerSelection = null;

	void Awake() {
		gameObject.AddComponent<DigiBattle.Screen>();
	}

	void Start() {
#pragma warning disable UNT0023 // Coalescing assignment on Unity objects
		digimonSelectionSystem ??= gameObject.AddComponent<DigimonSelectionSystem>();
#pragma warning restore UNT0023 // Coalescing assignment on Unity objects

		startCpuCharacterSelectionButton.onClick.AddListener(() => {
			digimonSelectionSystem.StartTracking();
			startCpuCharacterSelectionButton.gameObject.SetActive(false);
		});

		(GameManager.Instance.gameMode switch {
			GameMode.SinglePlayer=> (Action)StartSinglePlayer,
			GameMode.MultiPlayer => StartMultiPlayer,
			_ => throw new NotImplementedException(),
		})();
	}

	void StartSinglePlayer() {
		digimonSelectionSystem.OnDigimonDetection = (result) => {
			var (digimon, card) = (result.digimon, result.card);
			Debug.Log($"Digimon detected: {digimon}");
			if (playerSelection == null) {
				Debug.Log($"Player digimon selected: {digimon.name}");
				playerSelection = new Player { digimon = digimon, card = card };
				digimonSelectionSystem.StopTracking();
				startCpuCharacterSelectionButton.gameObject.SetActive(true);
				return;
			}

			Debug.Log($"Enemy digimon selected: {digimon.name}");
			var enemy = new Enemy { digimon = digimon, card = card };

			StartBattle(playerSelection, enemy);
		};
	}

	async void StartMultiPlayer() {
		digimonSelectionSystem.OnDigimonDetection = (result) => {
			var (digimon, card) = (result.digimon, result.card);
			Debug.Log($"Digimon detected: {digimon}");
		};
	}

	void StartBattle(Player player, Enemy enemy) {
		digimonSelectionSystem.StopTracking();
		GameManager.Instance.player = player;
		GameManager.Instance.enemy = enemy;
		loadingBar.gameObject.SetActive(true);
		SceneManager.LoadScene(GameScene.Battle);
	}
}