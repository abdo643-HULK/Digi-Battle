using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Digimon", menuName = "Digimon/Create new digimon base")]
public class DigimonBase : ScriptableObject {
	[SerializeField]
	private uint id;

	[SerializeField]
	private new string name;

	[SerializeField]
	Stats stats;

	[SerializeField]
	AttackInfo basicAttack;

	[SerializeField]
	AttackInfo specialAttack;

	#region Getters 
	public DigimonID ID {
		get => new DigimonID(id);
	}

	public string Name {
		get => name;
	}

	public AttackInfo BasicAttack {
		get => basicAttack;
	}

	public AttackInfo SpecialAttack {
		get => specialAttack;
	}

	public Stats Stats {
		get => stats;
	}
	#endregion
}

[System.Serializable]
public struct DigimonID {
	public uint id;
	public DigimonID(uint id) {
		this.id = id;
	}

	public static implicit operator uint(DigimonID id) => id.id;

	//public static implicit operator DigimonID(uint id) => new() { id = id };


	public static bool operator !=(DigimonID a, DigimonID b) => a.id != b.id;
	public static bool operator ==(DigimonID a, DigimonID b) => a.id == b.id;

	public static int operator -(DigimonID a, DigimonID b) => (int)(a.id - b.id);

	public override readonly string ToString() => id.ToString();
	public override readonly int GetHashCode() => id.GetHashCode();

	public override readonly bool Equals(object obj) => obj is DigimonID id && this.id == id.id;
}


[System.Serializable]
public struct Stats {
	public uint hp;
	public uint attack;
	public uint defense;
	public uint speed;
}

[System.Serializable]
public enum AttackDistance {
	Physical,
	Range,
}

[System.Serializable]
public struct AttackInfo {
	public AttackDistance distance;
	public string name;
	public uint damage;
	public uint apCost;
}