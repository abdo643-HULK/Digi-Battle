using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using System.Linq;

#nullable enable

// Maybe make it a ScriptableObject
public class GameManager : MonoBehaviour {
	// Code for ScriptableObject Instance
	//private static GameManager? sInstance = null;
	//public static GameManager Instance {
	//	get {
	//		if (sInstance == null) {
	//			GameManager obj = Resources.Load<GameManager>("GameManager") ?? Create();
	//			sInstance = UnityEngine.Object.Instantiate(obj);
	//		}
	//		return sInstance;
	//	}
	//	internal set {
	//		sInstance = value;
	//	}
	//}

	// static GameManager Create() {
	// GameManager manager = ScriptableObject.CreateInstance<GameManager>();
	// return manager;
	// }

	public static GameManager _instance;

	/// <summary>Static reference to the instance of our DataManager</summary>
	public static GameManager Instance {
		get {
			if (_instance == null) {
				Debug.LogError("GameManager is null");
			}

			return _instance;
		}
	}

	public DigimonDatabase digimonDatabase = new(3);

	public Player player = new();

	public Enemy enemy = new();

	public GameMode gameMode = GameMode.SinglePlayer;

	void Awake() {
		Debug.Log("GameManager Awake");
		// If the instance reference has not been set, yet, 
		if (_instance) {
			Destroy(gameObject);
		} else {
			_instance = this;
			_instance.digimonDatabase.cards = Resources.LoadAll<DigiCard>("DigimonCards").ToList();
			_instance.digimonDatabase.AddDigimonRange(Resources.LoadAll<DigimonBase>("DigimonBase"));
		}

		// Do not destroy this object, when we load a new scene.
		DontDestroyOnLoad(gameObject);
	}
}

public enum GameMode {
	SinglePlayer,
	MultiPlayer,
}

public struct GameScene {
	private readonly uint id;

	public static GameScene MainMenu = new(0);
	public static GameScene DigimonSelection = new(1);
	public static GameScene Battle = new(2);

	public static implicit operator uint(GameScene scene) => scene.id;
	public static implicit operator int(GameScene scene) => (int)scene.id;

	private GameScene(uint id) {
		this.id = id;
	}
}

public class Player {
	public Digimon? digimon = null;
	public DigiCard? card = null;
}

public class Enemy {
	public Digimon? digimon = null;
	public DigiCard? card = null;
}

// This is just for prototyping purposes.
// In a real game, we would use an actual database like SQLite.
// Digimons should be loaded when actually needed and not loaded all at once. 
// But for now, this is fine.
public class DigimonDatabase {
	//private static DigimonDatabase? _instance = null;

	//public static DigimonDatabase Instance {
	//	get {
	//		if (_instance == null) {
	//			_instance = new DigimonDatabase();
	//		}

	//		return _instance;
	//	}
	//}

	//public static DigimonDatabase Instance { get; private set; }

	public List<DigiCard> cards = new();
	public List<DigimonBase> digimon = new(3);

	internal DigimonDatabase(int capacity) {
		digimon = new List<DigimonBase>(capacity);
	}

	public void Add(Digimon digimon) {
		Debug.Log($"DigimonDatabase: Adding {digimon.Name}");
		this.digimon.Add(digimon.Base);
		this.digimon.Sort((a, b) => a.ID - b.ID);
	}

	public void AddDigimonRange(IEnumerable<DigimonBase> digimonToAdd) {
		digimon.AddRange(digimonToAdd);
		digimon.Sort((a, b) => a.ID - b.ID);
		Debug.Log($"DigimonDatabase: {digimon.Count} digimons loaded");
	}

	public void AddDigiCardsRange(IEnumerable<DigiCard> cardsToAdd) {
		cards.AddRange(cardsToAdd);
	}

	public Digimon? GetByID(DigimonID digimonID) {
		Debug.Log($"DigimonDatabase: Searching for {digimonID}");
		var digimon = this.digimon.Find(digimon => digimon.ID == digimonID);
		Debug.Log($"DigimonDatabase: {digimon.Name}");
		if (digimon == null) {
			Debug.LogError($"DigimonDatabase: Digimon with ID {digimonID} not found");
			return null;
		}

		return LoadDigimon(digimon);
	}

	public DigiCard? GetCardByName(string name) {
		var card = cards.Find(card => card.name == name);
		if (card == null) {
			Debug.LogError($"DigimonDatabase: Card with name {name} not found");
			return null;
		}

		return card;
	}

	public DigiCard? GetCardByDigimonID(DigimonID digimonID) {
		var card = cards.Find(card => card.DigimonID == digimonID);
		if (card == null) {
			Debug.LogError($"DigimonDatabase: Card with DigimonID {digimonID} not found");
			return null;
		}

		return card;
	}

	private Digimon LoadDigimon(DigimonBase digimon) {
		var path = "Digimon/" + digimon.Name.Replace(" ", "");
		Debug.Log($"Loading: {path}");
		return Resources.Load<Digimon>(path);
	}

	public Digimon? GetByName(string name) {
		Debug.Log($"DigimonDatabase: Searching for {name}");
		var digimon = this.digimon.Find(digimon => digimon.Name == name);
		var result = digimon != null ? $"Found {digimon.name}" : "Not Found";
		Debug.Log($"DigimonDatabase: {result}");
		return digimon == null ? null : LoadDigimon(digimon);
	}

	public int Count() => digimon.Count;

	//void Awake() {
	//	// If the instance reference has not been set, yet, 
	//	if (Instance == null) {
	//		// Set this instance as the instance reference.
	//		Instance = this;
	//	} else if (Instance != this) {
	//		// If the instance reference has already been set, and this is not the
	//		// the instance reference, destroy this game object.
	//		Destroy(gameObject);
	//	}

	//	// Do not destroy this object, when we load a new scene.
	//	DontDestroyOnLoad(gameObject);
	//}
}