using Godot;
using Godot.Collections;

public class LevelData : Resource
{

	[Export] public string LevelID { get; set; } = "level_0000";
	[Export] public string LevelName { get; set; } = "Level 0";

	[Export] public virtual bool HasBeenCompleted { get; set; } = true;

	[Export] public Dictionary<string, Dictionary<string, object>> Data { get; set; } = new Dictionary<string, Dictionary<string, object>>();

}
