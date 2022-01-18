using Godot;

public class BaseLevelHUD : BaseHUD
{

	#region Nodes

	public TextureProgress node_playerHeartTextureProgress { get; private set; }
	public TextureProgress node_playerFireTimerTextureProgress { get; private set; }

	#endregion // Nodes



	#region Backing fields

	private float _playerHealth = 1;
	private float _playerFireRateTimer = 1f;

	#endregion // Backing fields



	#region Properties

	public float PlayerHealth
	{
		get => _playerHealth;
		set
		{
			_playerHealth = Mathf.Clamp(value, 0f, 1f);
			node_playerHeartTextureProgress.Value = _playerHealth;
		}
	}
	public float PlayerFireRateTimer
	{
		get => _playerFireRateTimer;
		set
		{

			_playerFireRateTimer = Mathf.Clamp(value, 0f, 1f);
			node_playerFireTimerTextureProgress.Value = _playerFireRateTimer;
		}
	}

	#endregion // Properties



	#region Godot methods

	public override void _Ready ()
	{
		base._Ready();

		node_playerHeartTextureProgress = GetNode<TextureProgress>("HeartPanel/CenterContainer/HeartTextureProgress");
		node_playerFireTimerTextureProgress = GetNode<TextureProgress>("FireTimerPanel/CenterContainer/BoneTextureProgress");
	}

	#endregion // Godot methods

}
