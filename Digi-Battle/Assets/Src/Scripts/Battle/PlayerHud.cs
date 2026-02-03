using TMPro;
using UnityEngine;

public class PlayerHud : MonoBehaviour {
	public TextMeshProUGUI digimonName;

	[InspectorName("HP Bar")]
	[RenameAttribute("HP Bar")]
	public HpBar hpBar;

	[InspectorName("AP Bar")]
	[RenameAttribute("AP Bar")]
	public ApBar apBar;
}