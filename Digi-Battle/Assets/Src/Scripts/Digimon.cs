using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using Extensions;

#nullable enable

[CreateAssetMenu(fileName = "Digimon", menuName = "Digimon/Create new digimon")]
public class Digimon : ScriptableObject {
	[SerializeField]
	private DigimonBase _base;

	// Includes the model and the animator
	//public DigimonModel model;
	public GameObject prefab;

	public List<DigiCard> cards;

	public AnimationClip enterAnimation;
	public AnimationClip victoryAnimation;

	public AnimationClip idleAnimation;
	public AnimationClip hitAnimation;

	//public AnimationClip runAnimation;
	public AnimationClip basicAttackAnimation;
	public AnimationClip specialAttackAnimation;
	public AnimationClip? jumpBackBasicAttackAnimation = null;
	public AnimationClip? jumpBackSpecialAttackAnimation = null;

	public DigimonBase Base {
		get => _base;
	}

	public DigimonID ID {
		get => _base.ID;
	}

	public string Name {
		get => _base.Name;
	}

	public Stats Stats {
		get => _base.Stats;
	}

	public AttackInfo AttackInfo {
		get => _base.BasicAttack;
	}

	public AttackInfo SpecialAttackInfo {
		get => _base.SpecialAttack;
	}

	public uint MinApConst {
		get => math.min(_base.BasicAttack.apCost, _base.SpecialAttack.apCost);
	}

	public Animator Animator {
		get => prefab.GetComponent<Animator>();
	}

	void Awake() {
		//if (Application.isEditor) {
		//	this.ValidateInspectorField(enterAnimation);
		//	this.ValidateInspectorField(victoryAnimation);
		//	this.ValidateInspectorField(idleAnimation);
		//	this.ValidateInspectorField(basicAttackAnimation);
		//	this.ValidateInspectorField(specialAttackAnimation);
		//}
	}

	//public void OnCollisionEnter(Collision collision) {
	//	if(collision.collider.tag == "Enemy") {
	//		animationController.PlayHit();
	//	}
	//}
}

public enum Attack {
	Basic,
	Special
}