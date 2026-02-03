using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;
using static Constants;

public class ApBar : MonoBehaviour {
	private uint points = MAX_AP;

	[SerializeField]
	private List<Image> pointImages = new((int)MAX_AP);

	public void SetPoints(uint pointsToSet) {
		points = math.min(pointsToSet, MAX_AP);

		for (int i = 0; i < MAX_AP; i++) {
			pointImages[i].gameObject.SetActive(i < points);
		}
	}

	public void AddPoints(uint pointsToAdd = 1) {
		points = math.min(points + pointsToAdd, MAX_AP);

		for (int i = 0; i < MAX_AP; i++) {
			pointImages[i].gameObject.SetActive(i < points);
		}
	}

	public void RemovePoints(int pointsToRemove = 1) {
		points = (uint)math.max((int)points - pointsToRemove, 0);

		for (int i = 0; i < MAX_AP; i++) {
			pointImages[i].gameObject.SetActive(i < points);
		}
	}

	public void Reset() {
		points = MAX_AP;

		for (int i = 0; i < MAX_AP; i++) {
			pointImages[i].enabled = true;
		}
	}
}
