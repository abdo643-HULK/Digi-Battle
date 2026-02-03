using System.Threading.Tasks;
using UnityEngine;

public class NetworkPlayerController : IController {
	public DigimonState digimonState = new();

	private readonly DigimonInstance digimonInstance;

	public DigimonState State => digimonState;
	public Stats DigimonStats => digimonInstance.digimon.Base.Stats;
	public DigimonBattleAnimationController Animator => digimonInstance.animator;
	public uint MinApConst => digimonInstance.digimon.MinApConst;

	public NetworkPlayerController(Digimon digimon, GameObject digimonObj) {
		digimonInstance = new DigimonInstance(digimonObj, digimon);
	}

	public AttackInfo Attack() {
		throw new System.NotImplementedException();
	}

	public void TakeDamage(float damage) {
		throw new System.NotImplementedException();
	}

	Task<(Attack, AttackInfo)> IController.SelectAttack() {
		throw new System.NotImplementedException();
	}

	float IController.TakeDamage(float damage) {
		throw new System.NotImplementedException();
	}

	uint IController.RegenerateAp(uint ap) {
		throw new System.NotImplementedException();
	}

	uint IController.ReduceAp(uint ap) {
		throw new System.NotImplementedException();
	}
}