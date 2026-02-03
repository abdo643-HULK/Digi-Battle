using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AttackDamage {
	Normal,
	Critical,
};

public enum BattleResult {
	Victory,
	Defeat,
};

public enum BattleState {
	Start,
	PlayerAction,
	PlayerMove,
	EnemyMove,
};

public class BattleSystem : MonoBehaviour {
	struct DamageDealer {
		public float attack;
		public float buffs;
		public float attackDamage;
	};

	struct DamageReceiver {
		public float defense;
	};

	public PlayerController playerController;
	public IController enemyController;


	public Action OnBattleStart;
	public Action<uint> OnEnemyApRegeneration;
	public Action<uint> OnPlayerApRegeneration;
	public Action OnPlayerTurnStarted;
	public Action OnPlayerPhysicalAttackStart;
	public Action OnPlayerPhysicalAttackEnd;
	public Action<float> OnEnemyHit;
	public Action<float> OnPlayerHit;
	public Action<BattleResult> OnBattleEnd;

	//public AttackSelection attackSelection;

	public bool battleEnded = false;

	private DigimonState EnemyState { get => enemyController.State; }
	private DigimonState PlayerState { get => playerController.State; }


	public IEnumerator StartBattle() {
		playerController.Setup();
		enemyController.Setup();

		playerController.Start();
		enemyController.Start();

		yield return WaitForEnterAnimations();

		var startingPlayer = UnityEngine.Random.Range(0, 1);
		Debug.Log("Battle Started");
		if (startingPlayer == 0) {
			yield return OnPlayerTurn();
		} else {
			yield return OnEnemyTurn();
		}
	}

	private IEnumerator WaitForEnterAnimations() {
		var a = StartCoroutine(playerController.Animator.PlayEnter());
		var b = StartCoroutine(enemyController.Animator.PlayEnter());

		yield return a;
		yield return b;
	}

	private IEnumerator OnPlayerTurn() {
		Debug.Log("Player: Turn started");
		var controller = playerController;
		var animator = controller.Animator;

		#region Calculations
		Debug.Log("Player: Regenerating AP");
		var curentAP = playerController.RegenerateAp();
		OnPlayerApRegeneration(curentAP);
		if (curentAP < controller.MinApConst) {
			// Print: Not enough AP skipping turn
			yield return new WaitForSeconds(1.0f);
		}
		OnPlayerTurnStarted();
		Debug.Log("Player: Waiting for attack selection");
		var attackTask = controller.SelectAttack();
		yield return new WaitUntil(() => attackTask.IsCompleted);
		var (attack, attackInfo) = attackTask.Result;
		Debug.Log("Player: Selected attack: " + attack);
		controller.ReduceAp(attackInfo.apCost);

		var newHp = enemyController.TakeDamage(CalculateDamage(new DamageDealer {
			attack = controller.DigimonStats.attack,
			buffs = controller.State.buffs,
			attackDamage = attackInfo.damage,
		}, new DamageReceiver {
			defense = enemyController.DigimonStats.defense,
		}));
		#endregion

		#region Animations
		// Run Attack
		Debug.Log("Player: Running attack animation");
		yield return attack switch {
			Attack.Basic => animator.PlayBasicAttack(),
			Attack.Special => animator.PlaySpecialAttack(),
			_ => throw new NotImplementedException()
		};
		Debug.Log("Player: Attack animation finished");

		// Run enemy damage animation
		enemyController.Animator.PlayHit();
		OnEnemyHit(newHp);

		if (attackInfo.distance == AttackDistance.Physical) {
			//yield return playerDigimonController.animator.PlayJumpBack();
			yield return animator.WaitForJumpBack();
		}
		#endregion

		if (EnemyState.hp <= 0) {
			yield return OnBattleEnded(BattleResult.Victory);
		} else {
			yield return OnEnemyTurn();
		}
	}

	private IEnumerator OnEnemyTurn() {
		Debug.Log("Enemy: Turn started");
		var controller = enemyController;
		var animator = enemyController.Animator;

		#region Calculations
		var currentAP = controller.RegenerateAp();
		OnEnemyApRegeneration(currentAP);
		if (currentAP < controller.MinApConst) {
			yield return new WaitForSeconds(1.0f);
		}

		Debug.Log("Enemy: Waiting for attack selection");
		var attackTask = controller.SelectAttack();
		yield return new WaitUntil(() => attackTask.IsCompleted);
		var (attack, attackInfo) = attackTask.Result;
		Debug.Log("Enemy: Selected attack: " + attack);
		controller.ReduceAp(attackInfo.apCost);

		var newHp = playerController.TakeDamage(CalculateDamage(new DamageDealer {
			attack = controller.DigimonStats.attack,
			buffs = controller.State.buffs,
			attackDamage = attackInfo.damage,
		}, new DamageReceiver {
			defense = playerController.DigimonStats.defense,
		}));
		#endregion

		#region Animations
		Debug.Log("Enemy: Running attack animation");
		yield return attack switch {
			Attack.Basic => animator.PlayBasicAttack(),
			Attack.Special => animator.PlaySpecialAttack(),
			_ => throw new NotImplementedException(),
		};
		Debug.Log("Enemy: Attack animation finished");

		playerController.Animator.PlayHit();
		OnPlayerHit(newHp);

		if (attackInfo.distance == AttackDistance.Physical) {
			//yield return enemyDigimonController.animator.PlayJumpBack();
			yield return animator.PlayJumpBack();
		}
		#endregion

		if (PlayerState.hp <= 0) {
			yield return OnBattleEnded(BattleResult.Defeat);
		} else {
			yield return OnPlayerTurn();
		}
	}

	private IEnumerator OnBattleEnded(BattleResult result) {
		// Run victory animation and wait for it to finish
		var animator = result == BattleResult.Victory ? playerController.Animator : enemyController.Animator;
		yield return animator.PlayVictory();
		//yield return new WaitForSeconds(digimon.victoryAnimation.length);
		// Return to overworld
		OnBattleEnd?.Invoke(result);
	}

	float CalculateDamage(DamageDealer attacker, DamageReceiver receiver) {
		var totalAttack = attacker.attack * (1.0 + attacker.buffs) * attacker.attackDamage;
		return (float)(totalAttack / receiver.defense);
	}

	IEnumerator MoveTowards(Transform objectToMove, Vector3 toPosition, float duration) {
		float counter = 0;

		while (counter < duration) {
			counter += Time.deltaTime;
			Vector3 currentPos = objectToMove.position;

			float time = Vector3.Distance(currentPos, toPosition) / (duration - counter) * Time.deltaTime;

			objectToMove.position = Vector3.MoveTowards(currentPos, toPosition, time);

			Debug.Log(counter + " / " + duration);
			yield return null;
		}
	}
}