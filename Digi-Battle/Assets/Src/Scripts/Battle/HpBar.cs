using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HpBar : MonoBehaviour {
	private static Color orange = new(1.0f, 0.5333334f, 1.0f);

	[HideInInspector]
	public uint maxHp = 0;

	[SerializeField]
	private Image bar;

	public void SetHP(float hp) {
		var currentHp = hp / maxHp;
		bar.fillAmount = currentHp;
		bar.color = currentHp switch {
			float n when n > 0.7f => Color.green,
			float n when n > 0.3f => orange,
			_ => Color.red
		};
	}
}