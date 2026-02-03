using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

#nullable enable

public class BattleHud : MonoBehaviour {

	[SerializeField]
	private PlayerHud player;
	[SerializeField]
	private EnemyHud enemy;

	[SerializeField]
	private Button basicAttack;
	[SerializeField]
	private Button specialAttack;

	private Attack? selectedAttack = null;

	//public Action<Attack> OnAttackSelection {
	//	set {

	//	}
	//}

	private void Start() {
		var playerDigimon = GameManager.Instance.player.digimon!;
		var enemyDigimon = GameManager.Instance.enemy.digimon!;

		player.digimonName.text = playerDigimon.Name;
		enemy.digimonName.text = enemyDigimon.Name;

		player.hpBar.maxHp = playerDigimon.Stats.hp;
		enemy.hpBar.maxHp = enemyDigimon.Stats.hp;

		basicAttack.GetComponentInChildren<TextMeshProUGUI>().text = playerDigimon.Base.BasicAttack.name;
		specialAttack.GetComponentInChildren<TextMeshProUGUI>().text = playerDigimon.Base.SpecialAttack.name;

		basicAttack.onClick.AddListener(() => selectedAttack = Attack.Basic);
		specialAttack.onClick.AddListener(() => selectedAttack = Attack.Special);
	}

	public async Task<Attack> WaitForAttackPress() {
		while (selectedAttack == null) {
			await Task.Yield();
		}

		var attack = selectedAttack.Value;
		selectedAttack = null;
		return attack;
	}

	public void EnableButtons() {
		basicAttack.gameObject.SetActive(true);
		specialAttack.gameObject.SetActive(true);
	}

	public void DisableButtons() {
		basicAttack.gameObject.SetActive(false);
		specialAttack.gameObject.SetActive(false);
	}

	public void SetPlayerHP(float hp) {
		player.hpBar.SetHP(hp);
	}

	public void SetEnemyHP(float hp) {
		enemy.hpBar.SetHP(hp);
	}

	public void SetPlayerAP(uint ap) {
		player.apBar.SetPoints(ap);
	}

	public void SetEnemyAP(uint ap) {
		enemy.apBar.SetPoints(ap);
	}
}
