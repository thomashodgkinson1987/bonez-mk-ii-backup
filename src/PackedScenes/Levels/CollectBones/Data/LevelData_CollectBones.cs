using Godot;
using Godot.Collections;

public class LevelData_CollectBones : LevelData
{

	[Export] public int NumberOfBonesInLevelTotal { get; set; } = 0;
	[Export] public int NumberOfBonesInLevelLeft { get; set; } = 0;
	[Export] public int NumberOfBonesInLevelCollected { get; set; } = 0;

	[Export] public Array<string> NamesOfBonesInLevelTotal { get; set; } = new Array<string>();
	[Export] public Array<string> NamesOfBonesInLevelLeft { get; set; } = new Array<string>();
	[Export] public Array<string> NamesOfBonesInLevelCollected { get; set; } = new Array<string>();

	public override bool HasBeenCompleted
	{
		get => NumberOfBonesInLevelCollected == NumberOfBonesInLevelTotal;
	}

}
