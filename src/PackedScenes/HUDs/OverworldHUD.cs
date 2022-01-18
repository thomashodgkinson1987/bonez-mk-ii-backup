using Godot;

public class OverworldHUD : CanvasLayer
{

	#region Nodes

	public Label node_levelIDLabel { get; private set; }
	public Label node_levelNameLabel { get; private set; }
	public Label node_levelTypeLabel { get; private set; }

	#endregion // Nodes



	#region Backing fields

	private string _levelID = "level_0001";
	private string _levelName = "Level 1";
	private string _levelType = "Level";

	#endregion // Backing fields



	#region Properties

	public string LevelID
	{
		get => _levelID;
		set
		{
			_levelID = value;
			node_levelIDLabel.Text = _levelID;
		}
	}

	public string LevelName
	{
		get => _levelName;
		set
		{
			_levelName = value;
			node_levelNameLabel.Text = _levelName;
		}
	}

	public string LevelType
	{
		get => _levelType;
		set
		{
			_levelType = value;
			node_levelTypeLabel.Text = _levelType;
		}
	}

	#endregion // Properties



	#region Godot methods

	public override void _Ready ()
	{
		base._Ready();

		node_levelIDLabel = GetNode<Label>("OverworldPanel/HBoxContainer/VBoxContainer2/LevelIDLabel");
		node_levelNameLabel = GetNode<Label>("OverworldPanel/HBoxContainer/VBoxContainer2/LevelNameLabel");
		node_levelTypeLabel = GetNode<Label>("OverworldPanel/HBoxContainer/VBoxContainer2/LevelTypeLabel");
	}

	#endregion // Godot methods

}
