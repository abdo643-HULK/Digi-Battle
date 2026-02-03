using System;
using System.Collections;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

#nullable enable

public class DigimonBattleAnimationController {
	private enum State {
		Idle,
		BasicAttack,
		SpecialAttack,
		JumpBack,
		Hit
	}

	const int BASE_LAYER = 0;
	const int ENTRANCE_LAYER = 1;

	public float enterDuration = 0.0f;
	public float basicAttackDuration = 0.0f;
	public float specialAttackDuration = 0.0f;
	public float jumpBackDuration = 0.0f;

	public float basicAttackJumpBackDuration = 0.0f;
	public float specialAttackJumpBackDuration = 0.0f;
	public float victoryDuration = 0.0f;

	public float hitDuration = 0.0f;

	private readonly Animator animator;

	public DigimonBattleAnimationController(Animator animator, Digimon digimon) {
		this.animator = animator;

		Debug.Log($"Digimon: {digimon.name}");

		enterDuration = digimon.enterAnimation.length;
		victoryDuration = digimon.victoryAnimation.length;
		hitDuration = digimon.hitAnimation.length;

		basicAttackDuration = digimon.basicAttackAnimation.length;
		specialAttackDuration = digimon.specialAttackAnimation.length;

		if (digimon.jumpBackBasicAttackAnimation) {
			basicAttackJumpBackDuration = digimon.jumpBackBasicAttackAnimation.length;
		}

		if (digimon.jumpBackSpecialAttackAnimation) {
			specialAttackJumpBackDuration = digimon.jumpBackSpecialAttackAnimation.length;
		}

		// For some reason switch expressions do not work here
		//basicAttackJumpBackDuration = digimon.jumpBackBasicAttackAnimation switch {
		//	AnimationClip clip => clip.length,
		//	_ => 0.0f
		//};
		//specialAttackJumpBackDuration = digimon.jumpBackSpecialAttackAnimation switch {
		//	AnimationClip clip => clip.length,
		//	_ => 0.0f
		//};
	}


	public IEnumerator PlayEnter() {
		//animator.SetTrigger("EnterTrigger");
		animator.Play("Entrance", ENTRANCE_LAYER);
		yield return new WaitForSeconds(enterDuration);
	}

	public void PlayHit() {
		animator.SetTrigger("HitTrigger");

		//var length = animator.GetCurrentAnimatorClipInfo(BASE_LAYER)[0].clip.length;
		//yield return new WaitForSeconds(length);
	}


	public IEnumerator WaitForHit() {
		yield return new WaitForSeconds(hitDuration);
	}

	public IEnumerator PlayBasicAttack() {
		animator.SetTrigger("BasicAttackTrigger");

		jumpBackDuration = basicAttackJumpBackDuration;
		yield return new WaitForSeconds(basicAttackDuration);
	}

	public IEnumerator PlaySpecialAttack() {
		animator.SetTrigger("SpecialAttackTrigger");

		jumpBackDuration = specialAttackJumpBackDuration;
		yield return new WaitForSeconds(specialAttackDuration);
		//var length = animator.GetCurrentAnimatorStateInfo(BASE_LAYER).length;
		//jumpBackDuration = animator.GetNextAnimatorStateInfo(BASE_LAYER).length;
		//yield return new WaitForSeconds(length);
	}

	//public async void PlaySpecialAttack() {
	//	animator.SetTrigger("SpecialAttackTrigger");
	//	jumpBackDuration = animator.GetNextAnimatorStateInfo(BASE_LAYER).length;
	//	await Task.Delay(BaseLayerClipLength);
	//}

	public IEnumerator WaitForJumpBack() {
		yield return new WaitForSeconds(jumpBackDuration);
	}

	public IEnumerator PlayJumpBack() {
		//animator.SetBool("JumpBack", true);

		yield return new WaitForSeconds(jumpBackDuration);
		//yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(BASE_LAYER).length);
	}

	public IEnumerator PlayVictory() {
		animator.SetTrigger("VictoryTrigger");

		//var length = animator.GetNextAnimatorStateInfo(BASE_LAYER).length;
		yield return new WaitForSeconds(victoryDuration);
	}

	private TimeSpan BaseLayerStateClipLength => LayerStateClipLength(BASE_LAYER);
	private TimeSpan BaseLayerNextStateClipLength => LayerStateClipLength(BASE_LAYER);
	private TimeSpan EntranceLayerClipLength => LayerStateClipLength(ENTRANCE_LAYER);

	private TimeSpan LayerStateClipLength(int layer) => TimeSpan.FromSeconds((double)animator.GetCurrentAnimatorStateInfo(layer).length);

	private TimeSpan LayerStateNextClipLength(int layer) => TimeSpan.FromSeconds((double)animator.GetNextAnimatorStateInfo(layer).length);

}