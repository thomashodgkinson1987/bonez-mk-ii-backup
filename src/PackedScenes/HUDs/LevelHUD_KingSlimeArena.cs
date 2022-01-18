using Godot;

public class LevelHUD_KingSlimeArena : BaseLevelHUD
{

	#region Nodes

	public Label node_kingSlimeHealthLabel { get; private set; }

	#endregion // Nodes



	#region Backing fields

	private int _kingSlimeHealth = 10;

	#endregion // Backing fields



	#region Properties

	public int KingSlimeHealth
	{
		get => _kingSlimeHealth;
		set
		{
			_kingSlimeHealth = value;
			node_kingSlimeHealthLabel.Text = _kingSlimeHealth.ToString();
		}
	}

	#endregion // Properties



	#region Godot methods

	public override void _Ready ()
	{
		base._Ready();

		node_kingSlimeHealthLabel = GetNode<Label>("KingSlimePanel/KingSlimeHealthLabel");
	}

	#endregion // Godot methods

}
