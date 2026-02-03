using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DigiCard", menuName = "Digimon/Create new digimon card")]
public class DigiCard : ScriptableObject {
	[SerializeField]
	private uint digimonID;

	public new string name;
	public string description = "";

	public Texture2D image;

	public DigimonID DigimonID {
		get => new(digimonID);
	}
}

public struct Variant {
	public string name;
}
