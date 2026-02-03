#nullable enable

using System.Threading.Tasks;
using System;
using UnityEngine;


public class DigimonInstance {
	public GameObject digimonObj;
	public Digimon digimon;
	public DigimonBattleAnimationController animator;

	public DigimonInstance(GameObject digimonObj, Digimon digimon) {
		this.digimonObj = digimonObj;
		this.digimon = digimon;
		this.animator = new DigimonBattleAnimationController(digimonObj.GetComponent<Animator>(), digimon);
	}
}
public class DigimonState {
	public float hp = 0f;
	public float buffs = 0f;
	public uint ap = Constants.MAX_AP;
}

public interface IController {
	public DigimonState State {
		get;
	}

	public DigimonBattleAnimationController Animator {
		get;
	}

	public Stats DigimonStats {
		get;
	}

	public uint MinApConst {
		get;
	}


	public void Setup() { }

	public void Start() { }

	public Task<(Attack, AttackInfo)> SelectAttack();

	public float TakeDamage(float damage);

	public uint RegenerateAp(uint ap = 1);

	public uint ReduceAp(uint ap = 1);
}

