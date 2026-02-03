using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using Vuforia;


#nullable enable

public class DigimonSelectionSystem : MonoBehaviour {
#if AR_FOUNDATION
	[SerializeField]
	XRReferenceImageLibrary imageLibrary;

	private ARTrackedImageManager mTrackedImageManager;
#endif

	bool isTracking = false;

	public Action<DetectionResult>? OnDigimonDetection;

	void Awake() {
		VuforiaConfiguration.Instance.Vuforia.MaxSimultaneousImageTargets = 1;
	}

	void Start() {
#if AR_FOUNDATION
		mTrackedImageManager = gameObject.AddComponent<ARTrackedImageManager>();
		mTrackedImageManager.referenceLibrary = imageLibrary;
		mTrackedImageManager.trackedImagePrefab = imagePrefab;
		mTrackedImageManager.requestedMaxNumberOfMovingImages = 1;
		mTrackedImageManager.enabled = true;
#else

		Debug.Log("Vuforia started");
		var imagetargets = GameObject.Find("ImageTargets");
		foreach (var target in imagetargets.GetComponentsInChildren<ObserverBehaviour>()) {
			Debug.Log($"Found ObserverBehaviour: {target.TargetName}");
			target.OnTargetStatusChanged += OnTargetStatusChanged;
		}
#endif
		StartTracking();

		Debug.Log("DigimonSelectionSystem started");
	}

	public void StartTracking() {
		isTracking = true;
#if AR_FOUNDATION
		mTrackedImageManager.trackedImagesChanged += OnChanged;
#endif
	}

	public void StopTracking() {
		isTracking = false;
#if AR_FOUNDATION
		mTrackedImageManager.trackedImagesChanged -= OnChanged;
#endif
	}

	void OnEnable() => StartTracking();

	void OnDisable() => StopTracking();
#if AR_FOUNDATION
	void OnChanged(ARTrackedImagesChangedEventArgs eventArgs) {
		if (!isTracking) return;

		Debug.Log($"OnChanged: {eventArgs.added.Count} added, {eventArgs.updated.Count} updated, {eventArgs.removed.Count} removed");

		foreach (var detectedImage in eventArgs.added) {
			Debug.Log($"Image detected: {detectedImage.name}");

			var digimon = FindDigimonByName(detectedImage.name);
			if (digimon == null) continue;

			OnDigimonDetection?.Invoke(new DetectionResult {
				cardName = observer.TargetName,
				digimon = digimon,
			});
			break;

			//if (detectedImage.referenceImage.textureGuid != Guid.Empty) {
			//	// digimons.Find(digimon => digimon.Card.
			//	// Handle added event
			//}
		}
	}
#endif

	void OnTargetStatusChanged(ObserverBehaviour observer, TargetStatus status) {
		if (!isTracking) return;

		var isVisible = status.Status == Status.TRACKED || status.Status == Status.EXTENDED_TRACKED;

		if (isVisible && status.StatusInfo == StatusInfo.NORMAL) {
			Debug.Log("Tracking: " + observer.TargetName);

			var card = GameManager.Instance.digimonDatabase.GetCardByName(observer.TargetName);
			if (card == null) {
				Debug.LogError($"DigimonSelectionSystem: Card with name {observer.TargetName} not found");
				return;
			}

			Debug.Log($"Found Card: {card}");
			var digimon = GameManager.Instance.digimonDatabase.GetByID(card.DigimonID)!;

			OnDigimonDetection?.Invoke(new DetectionResult {
				card = card,
				digimon = digimon,
			});
		}
	}
}

public struct DetectionResult {
	public Digimon digimon;
	public DigiCard card;
}