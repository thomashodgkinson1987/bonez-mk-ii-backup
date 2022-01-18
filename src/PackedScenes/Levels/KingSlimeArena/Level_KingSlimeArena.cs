using Godot;

public class Level_KingSlimeArena : Level
{

	#region Nodes

	public LevelHUD_KingSlimeArena node_hud { get; private set; }

	public HoppingSlime node_kingSlime { get; private set; }

	public TileMap node_backgroundTilemap { get; private set; }

	public Node2D node_negLay1 { get; private set; }
	public Node2D node_negLay2 { get; private set; }
	public Node2D node_negLay3 { get; private set; }
	public Node2D node_negLay4 { get; private set; }
	public Node2D node_negLay5 { get; private set; }
	public Node2D node_negLay6 { get; private set; }

	#endregion // Nodes



	#region Signals

	[Signal] public delegate void OnKingSlimeKilledSignal ();

	#endregion // Signals



	#region Properties

	public bool HasKingSlimeBeenKilled { get; set; } = false;

	#endregion // Properties



	#region Godot methods

	public override void _Ready ()
	{
		base._Ready();

		node_hud = GetNode<LevelHUD_KingSlimeArena>("HUD");
		node_kingSlime = GetNode<HoppingSlime>("MainLayer/Actors/KingSlimes/KingSlime");
		node_backgroundTilemap = GetNode<TileMap>("ParallaxBackground/ParallaxLayer/TileMap");
		node_negLay1 = GetNode<Node2D>("NegativeLayers/Layer_-9/Left/Node2D2");
		node_negLay2 = GetNode<Node2D>("NegativeLayers/Layer_-9/Right/Node2D3");
		node_negLay3 = GetNode<Node2D>("NegativeLayers/Layer_-7/Center/Node2D3");
		node_negLay4 = GetNode<Node2D>("NegativeLayers/Layer_-5/Left/Node2D");
		node_negLay5 = GetNode<Node2D>("NegativeLayers/Layer_-5/Right/Node2D2");
		node_negLay6 = GetNode<Node2D>("NegativeLayers/Layer_-9/Center/Node2D4");

		node_kingSlime.Connect(nameof(Actor.OnHealthChangedSignal), this, nameof(OnKingSlimeHealthChanged));
		node_hud.KingSlimeHealth = node_kingSlime.Health;

		LevelSaver.Instance.LoadLevelData(this);

		if (HasKingSlimeBeenKilled) node_kingSlime.QueueFree();
	}

	#endregion // Godot methods



	#region Public methods

	public override bool HasLevelBeenCompleted () => HasKingSlimeBeenKilled;

	#endregion // Public methods



	#region Callback methods

	private void OnKingSlimeHealthChanged (HoppingSlime kingSlime, int oldHealth, int newHealth)
	{
		node_hud.KingSlimeHealth = newHealth;

		if (HasKingSlimeBeenKilled) return;

		float per = 1f - ((float)kingSlime.Health / (float)kingSlime.MaxHealth);

		Color color1 = new Color(1f, 1f, 1f, 1f);
		Color color2 = new Color(1f, 0f, 0f, 1f);
		Color color3 = color1.LinearInterpolate(color2, per);

		kingSlime.MiniHopHeight = Mathf.Lerp(16f, 64f, per);
		kingSlime.SmallHopHeight = Mathf.Lerp(32f, 128f, per);
		kingSlime.BigHopHeight = Mathf.Lerp(64f, 192f, per);

		kingSlime.SmallHopDistance = Mathf.Lerp(64f, 128f, per);
		kingSlime.BigHopDistance = Mathf.Lerp(96f, 256f, per);

		kingSlime.SlowWalkSpeed = Mathf.Lerp(32f, 128f, per);
		kingSlime.FastWalkSpeed = Mathf.Lerp(64f, 192f, per);

		kingSlime.Modulate = color3;

		node_backgroundTilemap.Modulate = color3;
		node_mainTileMaps.Modulate = color3;
		node_negLay1.Modulate = color3;
		node_negLay2.Modulate = color3;
		node_negLay3.Modulate = color3;
		node_negLay4.Modulate = color3;
		node_negLay5.Modulate = color3;
		node_negLay6.Modulate = color3;

		node_audioStreamPlayer.PitchScale = Mathf.Lerp(1.0f, 1.2f, per);

		if (newHealth <= 0)
		{
			HasKingSlimeBeenKilled = true;
			LevelSaver.Instance.SaveLevelData(this);
			ReturnToOverworld();
		}
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
