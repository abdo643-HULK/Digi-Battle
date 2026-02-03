

#nullable enable
using System;
using System.Threading.Tasks;
using UnityEngine;

public class PlayerController : IController {

	private DigimonState digimonState = new();

	private readonly DigimonInstance digimonInstance;

	private readonly BattleHud hud;

	public DigimonState State => digimonState;
	public Stats DigimonStats => digimonInstance.digimon.Base.Stats;
	public DigimonBattleAnimationController Animator => digimonInstance.animator;
	public uint MinApConst => digimonInstance.digimon.MinApConst;

	public PlayerController(BattleHud hud, Digimon digimon, GameObject digimonObj) {
		this.hud = hud;
		digimonInstance = new DigimonInstance(digimonObj, digimon);
	}

	public void Setup() {
		digimonState = new DigimonState {
			hp = digimonInstance.digimon.Base.Stats.hp,
		};
	}

	public void Start() {
		digimonState = new DigimonState {
			hp = digimonInstance.digimon.Base.Stats.hp,
		};
	}

	public float TakeDamage(float damage) {
		return digimonState.hp -= damage;
	}

	public uint RegenerateAp(uint ap = 1) {
		digimonState.ap = Math.Min(digimonState.ap + ap, Constants.MAX_AP);
		return digimonState.ap;
	}

	public uint ReduceAp(uint ap = 1) {
		digimonState.ap = Math.Max(digimonState.ap - ap, 0);
		return digimonState.ap;
	}

	public async Task<(Attack, AttackInfo)> SelectAttack() {
		hud.EnableButtons();
		var attack = await hud.WaitForAttackPress();
		var attackInfo = attack switch {
			Attack.Basic => digimonInstance.digimon.Base.BasicAttack,
			Attack.Special => digimonInstance.digimon.Base.SpecialAttack,
			_ => throw new NotImplementedException(),
		};
		hud.DisableButtons();
		return (attack, attackInfo);
	}

}