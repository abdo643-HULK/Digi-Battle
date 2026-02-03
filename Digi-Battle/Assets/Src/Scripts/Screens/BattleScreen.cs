using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;
using Vuforia;
using static UnityEngine.GraphicsBuffer;


#nullable enable

public class BattleScreen : MonoBehaviour {
	public const uint MIN_TRACKED_IMAGES = 2;

	private readonly string databasePath = $"Vuforia/Digimon.xml";

	public Camera arCamera;

	public BattleHud hud;

	public ResultHud resultHud;

	private uint trackedImages = 0;

	private BattleSystem battleSystem;

	private Coroutine? battleCoroutine = null;

	private ImageTargetBehaviour playerImageTarget;
	private ImageTargetBehaviour enemyImageTarget;

	private GameObject? playerDigimonInstance;
	private GameObject? enemyDigimonInstance;

	private NavMeshAgent testAgent;
	private GameObject movePlatform;

	void Awake() {
		gameObject.AddComponent<DigiBattle.Screen>();
		battleSystem = gameObject.AddComponent<BattleSystem>();
	}

	// Start is called before the first frame update
	void Start() {
		VuforiaConfiguration.Instance.Vuforia.MaxSimultaneousImageTargets = (int)MIN_TRACKED_IMAGES;
		//battleSystem.attackSelection = attackSelection;
		battleSystem.OnPlayerTurnStarted = () => hud.EnableButtons();
		battleSystem.OnEnemyHit = (newHp) => hud.SetEnemyHP(newHp);
		battleSystem.OnPlayerHit = (newHp) => hud.SetPlayerHP(newHp);
		battleSystem.OnEnemyApRegeneration = (newAp) => hud.SetEnemyAP(newAp);
		battleSystem.OnPlayerApRegeneration = (newAp) => hud.SetPlayerAP(newAp);

		battleSystem.OnBattleEnd = (result) => {
			resultHud.SetResultText(result == BattleResult.Victory ? "Win" : "Maybe Next Time");
			resultHud.Activate();
		};

#if UNITY_EDITOR
		var playerDigimon = GameManager.Instance.digimonDatabase.GetByID(new DigimonID(0))!;
		GameManager.Instance.player = new Player() {
			digimon = playerDigimon,
			card = GameManager.Instance.digimonDatabase.GetCardByDigimonID(playerDigimon.ID),
		};

		var enemyDigimon = GameManager.Instance.digimonDatabase.GetByID(new DigimonID(1))!;
		GameManager.Instance.enemy = new Enemy() {
			digimon = enemyDigimon,
			card = GameManager.Instance.digimonDatabase.GetCardByDigimonID(enemyDigimon.ID),
		};
#endif

		VuforiaApplication.Instance.OnVuforiaInitialized += OnVuforiaInitialized;

		//playerImageTarget = CreateTarget(GameManager.Instance.player.card!.name, "PlayerImageTarget");
		//enemyImageTarget = CreateTarget(GameManager.Instance.enemy.card!.name, "EnemyImageTarget");
	}

	private ImageTargetBehaviour CreateTarget(string target, string? gameObjectName = null) {
		Debug.Log("creating target: " + target);
		var imageTarget = VuforiaBehaviour.Instance.ObserverFactory.CreateImageTarget(databasePath, target);
		Debug.Log($"Created target: {imageTarget}");
		if (gameObjectName != null) {
			imageTarget.gameObject.name = gameObjectName;
		}
		imageTarget.transform.SetParent(arCamera.transform);
		imageTarget.transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
		imageTarget.transform.localScale = Vector3.one;
		imageTarget.OnTargetStatusChanged += OnTargetStatusChanged;
		return imageTarget;
	}

	private void OnVuforiaInitialized(VuforiaInitError error) {
		playerImageTarget = CreateTarget(GameManager.Instance.player.card!.name, "PlayerImageTarget");
		enemyImageTarget = CreateTarget(GameManager.Instance.enemy.card!.name, "EnemyImageTarget");
	}

	void OnTargetStatusChanged(ObserverBehaviour observer, TargetStatus status) {
		var isVisible = status.Status == Status.TRACKED || status.Status == Status.EXTENDED_TRACKED;

		if (isVisible && status.StatusInfo == StatusInfo.NORMAL) {
			trackedImages += 1;

			Debug.Log("Tracked Images: " + trackedImages);

			if (battleCoroutine == null && trackedImages == MIN_TRACKED_IMAGES) {
				var player = GameManager.Instance.player;
				var enemy = GameManager.Instance.enemy;

				playerDigimonInstance = Instantiate(player.digimon!.prefab, Vector3.zero, Quaternion.Euler(0, 90.0f, 0));
				playerDigimonInstance.GetComponent<Transform>().localScale = new Vector3(0.4f, 0.4f, 0.4f);
				playerDigimonInstance.GetComponent<Transform>().SetParent(playerImageTarget.transform, false);

				enemyDigimonInstance = Instantiate(enemy.digimon!.prefab, Vector3.zero, Quaternion.Euler(0, 270.0f, 0));
				enemyDigimonInstance.GetComponent<Transform>().localScale = new Vector3(0.4f, 0.4f, 0.4f);
				enemyDigimonInstance.GetComponent<Transform>().SetParent(enemyImageTarget.transform, false);

				// TODO: Move this to update so both of the characters are always facing each other
				//playerDigimonInstance.GetComponent<Transform>().LookAt(enemyDigimonInstance.transform);
				//enemyDigimonInstance.GetComponent<Transform>().LookAt(playerDigimonInstance.transform);

				playerDigimonInstance.SetActive(true);
				enemyDigimonInstance.SetActive(true);
				
				battleSystem.playerController = new PlayerController(hud, player.digimon!, playerDigimonInstance);

				battleSystem.enemyController = GameManager.Instance.gameMode switch {
					GameMode.SinglePlayer => new AiController(enemy.digimon!, enemyDigimonInstance),
					GameMode.MultiPlayer => new NetworkPlayerController(enemy.digimon!, enemyDigimonInstance),
					_ => throw new System.NotImplementedException(),
				};
				// We currently only handle fight start and any loss condition mid-fight
				battleCoroutine = StartCoroutine(battleSystem.StartBattle());
			}
		} else {
			trackedImages -= 1;
		}
	}

	private void Update() {
		//if (playerDigimonInstance != null && enemyDigimonInstance != null) {
		//	playerDigimonInstance.GetComponent<Transform>().LookAt(enemyDigimonInstance.transform);
		//}

		playerDigimonInstance?.GetComponent<Transform>().LookAt(enemyImageTarget.transform);
		enemyDigimonInstance?.GetComponent<Transform>().LookAt(playerImageTarget.transform);
	}

	Quaternion GetAngleBetweenMeAndEnemy(Transform player, Transform enemy) {
		Vector3 targetDir = player.transform.position - enemy.transform.position;
		Quaternion lookDir = Quaternion.LookRotation(targetDir);
		return lookDir;
	}
}

public class AttackSelection {
	internal Attack? attackSelection = null;

	public Attack SelectedAttack {
		get {
			var selected = attackSelection!.Value;
			attackSelection = null;
			return selected;
		}
	}

	public IEnumerator WaitForAttackSelection() {
		yield return new WaitWhile(() => attackSelection == null);
	}
}