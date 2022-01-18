using Godot;

public class LevelData_KingSlimeArena : LevelData
{

	[Export] public bool HasKingSlimeBeenKilled { get; set; } = false;

	public override bool HasBeenCompleted
	{
		get => HasKingSlimeBeenKilled;
	}

}
