using Godot;
using Godot.Collections;

public class LevelSaver
{

	#region Static instance

	private static LevelSaver _instance;
	public static LevelSaver Instance
	{
		get
		{
			if (_instance == null)
				_instance = new LevelSaver();
			return _instance;
		}
	}

	#endregion // Static instance



	#region Properties

	public int ActiveSaveSlot { get; set; } = 0;

	#endregion // Properties



	#region Constructors

	private LevelSaver () { }

	#endregion // Constructors



	#region Public methods

	public void SaveLevelData (Level level)
	{
		string filePath = $"user://save_{ActiveSaveSlot}.tres";
		Directory dir = new Directory();

		if (!dir.FileExists(filePath))
		{
			SaveFile newSaveFile = ResourceLoader.Load<SaveFile>("res://save_default.tres", null, true);
			newSaveFile.Reset();
			newSaveFile.SaveName = $"000{ActiveSaveSlot}";
			newSaveFile.CurrentLevelID = level.LevelID;
			ResourceSaver.Save(filePath, newSaveFile);
		}

		SaveFile saveData = ResourceLoader.Load<SaveFile>(filePath, null, true);

		if (!saveData.LevelDataDictionaries.ContainsKey(level.LevelID))
		{
			Dictionary<string, object> newLevelData = new Dictionary<string, object>();

			newLevelData.Add("level_id", level.LevelID);
			newLevelData.Add("level_name", level.LevelName);
			newLevelData.Add("has_been_completed", level.HasLevelBeenCompleted());

			if (level is Level_CollectBones level_collectBones)
			{
				newLevelData.Add("level_type", "collect_bones");

				newLevelData.Add("number_of_bones_total", level_collectBones.NumberOfBonesInLevelTotal);
				newLevelData.Add("number_of_bones_collected", level_collectBones.NumberOfBonesInLevelCollected);
				newLevelData.Add("number_of_bones_left", level_collectBones.NumberOfBonesInLevelLeft);

				newLevelData.Add("names_of_bones_total", new Array<string>(level_collectBones.NamesOfBonesInLevelTotal));
				newLevelData.Add("names_of_bones_collected", new Array<string>(level_collectBones.NamesOfBonesInLevelCollected));
				newLevelData.Add("names_of_bones_left", new Array<string>(level_collectBones.NamesOfBonesInLevelLeft));
			}
			else if (level is Level_KingSlimeArena level_kingSlimeArena)
			{
				newLevelData.Add("level_type", "king_slime_arena");

				newLevelData.Add("has_king_slime_been_killed", level_kingSlimeArena.HasKingSlimeBeenKilled);
			}

			saveData.LevelDataDictionaries.Add(level.LevelID, newLevelData);
		}
		else
		{
			Dictionary<string, object> levelData = saveData.LevelDataDictionaries[level.LevelID];

			levelData["level_id"] = level.LevelID;
			levelData["level_name"] = level.LevelName;
			levelData["has_been_completed"] = level.HasLevelBeenCompleted();

			if (level is Level_CollectBones level_collectBones)
			{
				levelData["level_type"] = "collect_bones";

				levelData["number_of_bones_total"] = level_collectBones.NumberOfBonesInLevelTotal;
				levelData["number_of_bones_collected"] = level_collectBones.NumberOfBonesInLevelCollected;
				levelData["number_of_bones_left"] = level_collectBones.NumberOfBonesInLevelLeft;

				levelData["names_of_bones_total"] = new Array<string>(level_collectBones.NamesOfBonesInLevelTotal);
				levelData["names_of_bones_collected"] = new Array<string>(level_collectBones.NamesOfBonesInLevelCollected);
				levelData["names_of_bones_left"] = new Array<string>(level_collectBones.NamesOfBonesInLevelLeft);
			}
			else if (level is Level_KingSlimeArena level_kingSlimeArena)
			{
				levelData["level_type"] = "king_slime_arena";

				levelData["has_king_slime_been_killed"] = level_kingSlimeArena.HasKingSlimeBeenKilled;
			}
		}

		ResourceSaver.Save(filePath, saveData);
	}

	public void LoadLevelData (Level level)
	{
		string filePath = $"user://save_{ActiveSaveSlot}.tres";
		Directory dir = new Directory();
		if (!dir.FileExists(filePath)) return;

		SaveFile saveData = ResourceLoader.Load<SaveFile>(filePath, null, true);
		if (!saveData.LevelDataDictionaries.ContainsKey(level.LevelID)) return;

		Dictionary<string, object> levelData = saveData.LevelDataDictionaries[level.LevelID];

		if (level is Level_CollectBones level_collectBones)
		{
			int numberOfBonesTotal = (int)levelData["number_of_bones_total"];
			int numberOfBonesCollected = (int)levelData["number_of_bones_collected"];
			int numberOfBonesLeft = (int)levelData["number_of_bones_left"];

			Array<string> namesOfBonesTotal = new Array<string>();
			Array _namesOfBonesTotal = (Array)levelData["names_of_bones_total"];
			for (int i = 0; i < _namesOfBonesTotal.Count; i++)
			{
				string name = (string)_namesOfBonesTotal[i];
				namesOfBonesTotal.Add(name);
			}

			Array<string> namesOfBonesCollected = new Array<string>();
			Array _namesOfBonesCollected = (Array)levelData["names_of_bones_collected"];
			for (int i = 0; i < _namesOfBonesCollected.Count; i++)
			{
				string name = (string)_namesOfBonesCollected[i];
				namesOfBonesCollected.Add(name);
			}

			Array<string> namesOfBonesLeft = new Array<string>();
			Array _namesOfBonesLeft = (Array)levelData["names_of_bones_left"];
			for (int i = 0; i < _namesOfBonesLeft.Count; i++)
			{
				string name = (string)_namesOfBonesLeft[i];
				namesOfBonesLeft.Add(name);
			}

			////

			level_collectBones.NumberOfBonesInLevelTotal = numberOfBonesTotal;
			level_collectBones.NumberOfBonesInLevelCollected = numberOfBonesCollected;
			level_collectBones.NumberOfBonesInLevelLeft = numberOfBonesLeft;

			level_collectBones.NamesOfBonesInLevelTotal = new Array<string>(namesOfBonesTotal);
			level_collectBones.NamesOfBonesInLevelCollected = new Array<string>(namesOfBonesCollected);
			level_collectBones.NamesOfBonesInLevelLeft = new Array<string>(namesOfBonesLeft);

			for (int i = 0; i < level_collectBones.node_bones.GetChildCount(); i++)
			{
				Bone bone = level_collectBones.node_bones.GetChild<Bone>(i);
				for (int j = 0; j < level_collectBones.NamesOfBonesInLevelCollected.Count; j++)
				{
					string nameOfBoneCollected = level_collectBones.NamesOfBonesInLevelCollected[j];
					if (bone.Name == nameOfBoneCollected)
					{
						bone.QueueFree();
						break;
					}
				}
			}
		}
		else if (level is Level_KingSlimeArena level_kingSlimeArena)
		{
			bool hasKingSlimeBeenKilled = (bool)levelData["has_king_slime_been_killed"];

			////

			level_kingSlimeArena.HasKingSlimeBeenKilled = hasKingSlimeBeenKilled;
		}
	}

	public void SaveOverworld (Scene_Overworld overworld)
	{
		string filePath = $"user://save_{ActiveSaveSlot}.tres";
		Directory dir = new Directory();

		if (!dir.FileExists(filePath))
		{
			SaveFile newSaveData = ResourceLoader.Load<SaveFile>("res://save_default.tres", null, true);
			newSaveData.Reset();
			newSaveData.SaveName = $"000{ActiveSaveSlot}";
			newSaveData.CurrentLevelID = overworld.CurrentLevelID;
			ResourceSaver.Save(filePath, newSaveData);
		}

		SaveFile saveData = ResourceLoader.Load<SaveFile>(filePath, null, true);
		saveData.CurrentLevelID = overworld.CurrentLevelID;
		ResourceSaver.Save(filePath, saveData);
	}

	public void LoadOverworld (Scene_Overworld overworld)
	{
		string filePath = $"user://save_{ActiveSaveSlot}.tres";
		Directory dir = new Directory();

		if (!dir.FileExists(filePath))
		{
			SaveFile newSaveFile = ResourceLoader.Load<SaveFile>("res://save_default.tres", null, true);
			newSaveFile.Reset();
			newSaveFile.SaveName = $"000{ActiveSaveSlot}";
			newSaveFile.CurrentLevelID = overworld.CurrentLevelID;
			ResourceSaver.Save(filePath, newSaveFile);
		}

		SaveFile saveData = ResourceLoader.Load<SaveFile>(filePath, null, true);

		overworld.CurrentLevelID = saveData.CurrentLevelID;

		for (int i = 0; i < overworld.node_levelNodes.GetChildCount(); i++)
		{
			OverworldLevelNode levelNode = overworld.node_levelNodes.GetChild<OverworldLevelNode>(i);

			if (levelNode.LevelID == overworld.CurrentLevelID)
			{
				overworld.CurrentLevelNode = levelNode;
			}

			PackedScene levelPackedScene = ResourceLoader.Load<PackedScene>
				($"res://assets/packed_scenes/levels/{levelNode.LevelID}.tscn");

			if (levelPackedScene != null)
			{
				Level level = (Level)levelPackedScene.Instance();

				if (level.LevelID == overworld.CurrentLevelID)
				{
					overworld.CurrentLevelName = level.LevelName;

					if (level is Level_CollectBones)
						overworld.CurrentLevelType = "Collect Bones";
					else if (level is Level_KingSlimeArena)
						overworld.CurrentLevelType = "King Slime Arena";
					else if (level is Level_NecroArena)
						overworld.CurrentLevelType = "Necro Arena";
					else
						overworld.CurrentLevelType = "Default";
				}

				if (saveData.LevelDataDictionaries.ContainsKey(level.LevelID))
				{
					Dictionary<string, object> levelData = saveData.LevelDataDictionaries[level.LevelID];
					if ((bool)levelData["has_been_completed"])
					{
						levelNode.State = OverworldLevelNode.EState.Complete;
					}
				}
				level.QueueFree();
			}
		}
	}

	public bool IsSaveSlotInUse (int slot)
	{
		return new Directory().FileExists($"user://save_{slot}.tres");
	}

	public void DeleteSaveSlot (int slot)
	{
		Directory dir = new Directory();

		if (dir.FileExists($"user://save_{slot}.tres"))
			dir.Remove($"user://save_{slot}.tres");

	}

	#endregion // Public methods

}