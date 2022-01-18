using Godot;
using Godot.Collections;

public class SaveFile : Resource
{

	[Export] public string SaveName { get; set; } = "0000";

	[Export] public string CurrentLevelID { get; set; } = "level_0000";

	[Export] public Dictionary<string, Dictionary<string, object>> LevelDataDictionaries { get; set; } = new Dictionary<string, Dictionary<string, object>>();

	public void Reset ()
	{
		SaveName = "0000";
		CurrentLevelID = "level_0000";
		LevelDataDictionaries.Clear();
		LevelDataDictionaries = new Dictionary<string, Dictionary<string, object>>();
	}

	public override string ToString ()
	{
		string message = "";

		message += $"SaveName: {SaveName}\n";
		message += $"CurrentLevelID: {CurrentLevelID}\n";

		message += "LevelData:\n";

		foreach (System.Collections.Generic.KeyValuePair<string, Dictionary<string, object>> levelDataDictionary in LevelDataDictionaries)
		{
			string key = levelDataDictionary.Key;
			Dictionary<string, object> value = levelDataDictionary.Value;

			string levelID = (string)value["level_id"];
			string levelName = (string)value["level_name"];
			bool hasBeenCompleted = (bool)value["has_been_completed"];
			string levelType = (string)value["level_type"];

			message += $" > LevelID: {levelID}\n";
			message += $" > LevelName: {levelName}\n";
			message += $" > HasBeenCompleted: {hasBeenCompleted}";
			message += $" > LevelType: {levelType}\n";

			if (levelType == "collect_bones")
			{
				int numberOfBonesTotal = (int)value["number_of_bones_total"];
				int numberOfBonesCollected = (int)value["number_of_bones_collected"];
				int numberOfBonesLeft = (int)value["number_of_bones_left"];

				Array<string> namesOfBonesTotal = (Array<string>)value["names_of_bones_total"];
				Array<string> namesOfBonesCollected = (Array<string>)value["names_of_bones_collected"];
				Array<string> namesOfBonesLeft = (Array<string>)value["names_of_bones_left"];

				////

				message += $" > NumberOfBonesTotal: {numberOfBonesTotal}\n";
				message += $" > NumberOfBonesCollected: {numberOfBonesCollected}\n";
				message += $" > NumberOfBonesLeft: {numberOfBonesLeft}\n";

				message += $"NamesOfBonesTotal ({namesOfBonesTotal.Count}):\n > ";
				for (int i = 0; i < namesOfBonesTotal.Count; i++)
					message += $"{namesOfBonesTotal[i]}, ";
				message += "\n";

				message += $"NamesOfBonesCollcted ({namesOfBonesCollected.Count}):\n > ";
				for (int i = 0; i < namesOfBonesCollected.Count; i++)
					message += $"{namesOfBonesCollected[i]}, ";
				message += "\n";

				message += $"NamesOfBonesLeft ({namesOfBonesLeft.Count}):\n > ";
				for (int i = 0; i < namesOfBonesLeft.Count; i++)
					message += $"{namesOfBonesLeft[i]}, ";
				message += "\n";
			}
			else if (levelType == "king_slime_arena")
			{
				bool hasKingSlimeBeenKilled = (bool)value["has_king_slime_been_killed"];

				////

				message += $" > HasKingSlimeBeenKilled: {hasKingSlimeBeenKilled}\n";
			}
		}

		return message;
	}
}
