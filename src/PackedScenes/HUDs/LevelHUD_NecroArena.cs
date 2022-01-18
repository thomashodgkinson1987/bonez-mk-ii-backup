using Godot;

public class LevelHUD_NecroArena : BaseLevelHUD
{

	#region Nodes

	public Label node_necroHealthLabel { get; private set; }

	#endregion // Nodes



	#region Backing fields

	private int _necroHealth = 10;

	#endregion // Backing fields



	#region Properties

	public int NecroHealth
	{
		get => _necroHealth;
		set
		{
			_necroHealth = value;
			node_necroHealthLabel.Text = _necroHealth.ToString();
		}
	}

	#endregion // Properties



	#region Godot methods

	public override void _Ready ()
	{
		base._Ready();

		node_necroHealthLabel = GetNode<Label>("NecroPanel/NecroHealthLabel");
	}

	#endregion // Godot methods

}
