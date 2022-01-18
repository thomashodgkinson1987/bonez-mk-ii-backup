using Godot;

public class LevelHUD_CollectBones : BaseLevelHUD
{

	#region Nodes

	public Label node_bonesCollectedLabel { get; private set; }
	public Label node_bonesTotalLabel { get; private set; }

	#endregion // Nodes



	#region Backing fields

	private int _bonesCollected = 0;
	private int _bonesTotal = 0;

	#endregion // Backing fields



	#region Properties

	public int BonesCollected
	{
		get => _bonesCollected;
		set
		{
			_bonesCollected = value;
			node_bonesCollectedLabel.Text = _bonesCollected.ToString();
		}
	}

	public int BonesTotal
	{
		get => _bonesTotal;
		set
		{
			_bonesTotal = value;
			node_bonesTotalLabel.Text = _bonesTotal.ToString();
		}
	}

	#endregion // Properties



	#region Godot methods

	public override void _Ready ()
	{
		base._Ready();

		node_bonesCollectedLabel = GetNode<Label>("BonePanel/HBoxContainer/BonesCollectedLabel");
		node_bonesTotalLabel = GetNode<Label>("BonePanel/HBoxContainer/BonesTotalLabel");
	}

	#endregion // Godot methods

}
