using Godot;
using Godot.Collections;

public class Level_CollectBones : Level
{

	#region Nodes

	public LevelHUD_CollectBones node_hud { get; private set; }

	#endregion // Nodes



	#region Signals

	[Signal] public delegate void OnPlayerCollectedBoneSignal (Bone bone);

	#endregion // Signals



	#region Exports

	#endregion // Exports



	#region Properties

	private int _numberOfBonesInLevelTotal = 0;
	public int NumberOfBonesInLevelTotal
	{
		get => _numberOfBonesInLevelTotal;
		set
		{
			_numberOfBonesInLevelTotal = value;
			node_hud.BonesTotal = _numberOfBonesInLevelTotal;
		}
	}
	public int NumberOfBonesInLevelLeft { get; set; } = 0;
	private int _numberOfBonesInLevelCollected = 0;
	public int NumberOfBonesInLevelCollected
	{
		get => _numberOfBonesInLevelCollected;
		set
		{
			_numberOfBonesInLevelCollected = value;
			node_hud.BonesCollected = _numberOfBonesInLevelCollected;
		}
	}

	public Array<string> NamesOfBonesInLevelTotal { get; set; } = new Array<string>();
	public Array<string> NamesOfBonesInLevelLeft { get; set; } = new Array<string>();
	public Array<string> NamesOfBonesInLevelCollected { get; set; } = new Array<string>();

	#endregion // Properties



	#region Godot methods

	public override void _Ready ()
	{
		base._Ready();

		node_hud = GetNode<LevelHUD_CollectBones>("HUD");

		NumberOfBonesInLevelTotal = node_bones.GetChildCount();
		NumberOfBonesInLevelLeft = NumberOfBonesInLevelTotal;
		NumberOfBonesInLevelCollected = 0;

		for (int i = 0; i < node_bones.GetChildCount(); i++)
			NamesOfBonesInLevelTotal.Add(node_bones.GetChild(i).Name);

		for (int i = 0; i < node_bones.GetChildCount(); i++)
			NamesOfBonesInLevelLeft.Add(node_bones.GetChild(i).Name);

		LevelSaver.Instance.LoadLevelData(this);

		for (int i = 0; i < node_bones.GetChildCount(); i++)
		{
			Bone bone = node_bones.GetChild<Bone>(i);
			bone.Connect(nameof(Bone.OnCollectedSignal), this, nameof(OnPlayerCollectedBone));
		}
	}

	#endregion // Godot methods



	#region Public methods

	public override bool HasLevelBeenCompleted () => NumberOfBonesInLevelCollected == NumberOfBonesInLevelTotal;

	#endregion // Public methods



	#region Callback methods

	private void OnPlayerCollectedBone (Bone bone)
	{
		NamesOfBonesInLevelLeft.Remove(bone.Name);
		NamesOfBonesInLevelCollected.Add(bone.Name);
		NumberOfBonesInLevelLeft--;
		NumberOfBonesInLevelCollected++;
	}

	protected override void OnPlayerHealthChanged (Bonez player, int oldHealth, int newHealth)
	{
		base.OnPlayerHealthChanged(player, oldHealth, newHealth);

		node_hud.PlayerHealth = (float)newHealth / (float)player.MaxHealth;
	}

	protected override void OnPlayerFireRateTimerChanged (Bonez player, float fireRate, float fireRateTimer)
	{
		base.OnPlayerFireRateTimerChanged(player, fireRate, fireRateTimer);

		node_hud.PlayerFireRateTimer = fireRateTimer / fireRate;
	}

	#endregion // Callback methods

}
