using System.Collections;
using TMPro;
using UnityEngine;

public class DebugDialogBox : MonoBehaviour {
	public TextMeshProUGUI text;

	private Coroutine delayCoroutine;

	public string Text {
		get => text.text;
		set {
			text.text = value;
			if (delayCoroutine != null) StopCoroutine(delayCoroutine);
			Open();
			delayCoroutine = StartCoroutine(DelayClose());
		}
	}

	private void Start() {
		Close();
	}

	private void Awake() {
		Close();
	}

	public void Open() {
		gameObject.SetActive(true);
	}

	public void Close() {
		gameObject.SetActive(false);
	}

	private IEnumerator DelayClose() {
		yield return new WaitForSeconds(3);
		Close();
	}
}