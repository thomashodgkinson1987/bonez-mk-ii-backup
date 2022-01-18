using Godot;

public class OverworldLevelNode : Node2D
{

	#region Nodes

	public Sprite node_sprite;

	#endregion // Nodes



	#region Enums

	public enum EState { Locked, Unlocked, Complete }

	#endregion // Enums



	#region Exports

	[Export]
	public EState State { get; set; } = EState.Locked;

	[Export] public string LevelID { get; set; }

	[Export] public string LeftNodeName { get; set; }
	[Export] public string RightNodeName { get; set; }
	[Export] public string UpNodeName { get; set; }
	[Export] public string DownNodeName { get; set; }

	[Export] public Texture LockedTexture { get; set; }
	[Export] public Texture UnlockedTexture { get; set; }
	[Export] public Texture CompleteTexture { get; set; }

	#endregion // Exports



	#region Properties

	public OverworldLevelNode LeftNode { get; set; }
	public OverworldLevelNode RightNode { get; set; }
	public OverworldLevelNode UpNode { get; set; }
	public OverworldLevelNode DownNode { get; set; }

	#endregion // Properties



	#region Godot methods

	public override void _Ready ()
	{
		base._Ready();

		node_sprite = GetNode<Sprite>("Sprite");

		UpdateGraphicFromState();
	}

	#endregion // Godot methods



	#region Public methods

	public void UpdateGraphicFromState ()
	{
		switch (State)
		{
			case EState.Locked:
			node_sprite.Texture = LockedTexture;
			break;

			case EState.Unlocked:
			node_sprite.Texture = UnlockedTexture;
			break;

			case EState.Complete:
			node_sprite.Texture = CompleteTexture;
			break;

			default:
			node_sprite.Texture = LockedTexture;
			break;
		}
	}

	#endregion // Public methods

}
