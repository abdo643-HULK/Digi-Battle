using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class AiController : IController {
	public DigimonState digimonState = new();

	private readonly DigimonInstance digimonInstance;

	public DigimonState State => digimonState;
	public Stats DigimonStats => digimonInstance.digimon.Base.Stats;
	public DigimonBattleAnimationController Animator => digimonInstance.animator;
	public uint MinApConst => digimonInstance.digimon.MinApConst;

	public AiController(Digimon digimon, GameObject digimonObj) {
		digimonInstance = new DigimonInstance(digimonObj, digimon);
	}

	public void Setup() {
		digimonState = new DigimonState {
			hp = DigimonStats.hp,
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

	public Task<(Attack, AttackInfo)> SelectAttack() {
		if (digimonState.ap >= digimonInstance.digimon.Base.SpecialAttack.apCost) {
			return Task.FromResult((global::Attack.Special, digimonInstance.digimon.Base.SpecialAttack));
		} else {
			return Task.FromResult((global::Attack.Basic, digimonInstance.digimon.Base.BasicAttack));
		}
	}
}