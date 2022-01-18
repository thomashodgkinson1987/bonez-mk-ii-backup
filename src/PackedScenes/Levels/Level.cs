using Godot;

public class Level : Node2D
{

	#region Nodes

	public AudioStreamPlayer node_audioStreamPlayer { get; private set; }

	public Node2D node_negativeLayers { get; private set; }
	public CanvasLayer node_mainLayer { get; private set; }
	public Node2D node_positiveLayers { get; private set; }

	public Fader node_fader { get; private set; }

	public ReferenceRect node_mapBounds { get; private set; }
	public Node2D node_mainTileMaps { get; private set; }
	public TileMap node_tileMapMain { get; private set; }

	public Bonez node_bonez { get; private set; }
	public Camera2D node_camera { get; private set; }

	public Node2D node_spikes { get; private set; }
	public Node2D node_exits { get; private set; }
	public Node2D node_platforms { get; private set; }
	public Node2D node_plants { get; private set; }
	public Node2D node_water { get; private set; }
	public Node2D node_actors { get; private set; }
	public Node2D node_hearts { get; private set; }
	public Node2D node_bones { get; private set; }
	public Node2D node_breakableBlocks { get; private set; }

	public Node2D node_kingSlimes { get; private set; }
	public Node2D node_kurts { get; private set; }
	public Node2D node_shtomps { get; private set; }
	public Node2D node_slimes { get; private set; }
	public Node2D node_hoppingSlimes { get; private set; }
	public Node2D node_bats { get; private set; }

	public Node2D node_projectiles { get; private set; }

	public LevelPauseMenu node_pauseMenu { get; private set; }

	#endregion // Nodes



	#region Exports

	[Export] public string LevelID { get; private set; } = "level_0000";
	[Export] public string LevelName { get; private set; } = "Level 0";

	#endregion // Exports



	#region Fields

	private LevelState m_state;

	#endregion // Fields



	#region Constructors

	public Level () : base()
	{
		m_state = new LevelState_Null(this);
	}

	#endregion // Constructors



	#region Godot methods

	public override void _Ready ()
	{
		base._Ready();

		node_audioStreamPlayer = GetNode<AudioStreamPlayer>("AudioStreamPlayer");

		node_negativeLayers = GetNode<Node2D>("NegativeLayers");
		node_mainLayer = GetNode<CanvasLayer>("MainLayer");
		node_positiveLayers = GetNode<Node2D>("PositiveLayers");

		node_fader = GetNode<Fader>("Fader");

		node_mainTileMaps = node_mainLayer.GetNode<Node2D>("Main_TileMaps");
		node_tileMapMain = node_mainTileMaps.GetNode<TileMap>("TileMap_Main");
		node_mapBounds = node_mainLayer.GetNode<ReferenceRect>("MapBounds");

		node_spikes = node_mainLayer.GetNode<Node2D>("Spikes");
		node_exits = node_mainLayer.GetNode<Node2D>("Exits");
		node_platforms = node_mainLayer.GetNode<Node2D>("Platforms");
		node_plants = node_mainLayer.GetNode<Node2D>("Plants");
		node_water = node_mainLayer.GetNode<Node2D>("Water");
		node_actors = node_mainLayer.GetNode<Node2D>("Actors");
		node_hearts = node_mainLayer.GetNode<Node2D>("Hearts");
		node_bones = node_mainLayer.GetNode<Node2D>("Bones");
		node_breakableBlocks = node_mainLayer.GetNode<Node2D>("BreakableBlocks");

		node_kingSlimes = node_actors.GetNode<Node2D>("KingSlimes");
		node_kurts = node_actors.GetNode<Node2D>("Kurts");
		node_shtomps = node_actors.GetNode<Node2D>("Shtomps");
		node_slimes = node_actors.GetNode<Node2D>("Slimes");
		node_hoppingSlimes = node_actors.GetNode<Node2D>("HoppingSlimes");
		node_bats = node_actors.GetNode<Node2D>("Bats");

		node_bonez = node_mainLayer.GetNode<Bonez>("Bonez");
		node_camera = node_bonez.GetNode<Camera2D>("Camera2D");

		node_pauseMenu = GetNode<LevelPauseMenu>("PauseMenu");

		node_pauseMenu.Connect(nameof(LevelPauseMenu.OnResumeButtonPressedSignal), this, nameof(OnResumed));
		node_pauseMenu.Connect(nameof(LevelPauseMenu.OnReturnToOverworldButtonPressedSignal), this, nameof(ReturnToOverworld));
		node_pauseMenu.Connect(nameof(LevelPauseMenu.OnReturnToTitleButtonPressedSignal), this, nameof(ReturnToTitle));
		node_pauseMenu.Connect(nameof(LevelPauseMenu.OnQuitToDesktopButtonPressedSignal), this, nameof(QuitToDesktop));

		node_pauseMenu.Hide();

		node_fader.Connect(nameof(Fader.OnFadingToTransparentSignal), this, nameof(OnFadingToTransparent));
		node_fader.Connect(nameof(Fader.OnFadedToTransparentSignal), this, nameof(OnFadedToTransparent));
		node_fader.Connect(nameof(Fader.OnFadingToOpaqueSignal), this, nameof(OnFadingToOpaque));
		node_fader.Connect(nameof(Fader.OnFadedToOpaqueSignal), this, nameof(OnFadedToOpaque));

		node_bonez.Connect(nameof(Actor.OnHealthChangedSignal), this, nameof(OnPlayerHealthChanged));
		node_bonez.Connect(nameof(Actor.OnHealthReachedZeroSignal), this, nameof(OnPlayerHealthReachedZero));
		node_bonez.Connect(nameof(Actor.OnMaxHealthChangedSignal), this, nameof(OnPlayerMaxHealthChanged));
		node_bonez.Connect(nameof(Bonez.OnFireRateTimerChangedSignal), this, nameof(OnPlayerFireRateTimerChanged));

		for (int i = 0; i < node_exits.GetChildCount(); i++)
		{
			LevelExit levelExit = node_exits.GetChild<LevelExit>(i);
			levelExit.Connect(nameof(LevelExit.OnExitAreaEnteredSignal), this, nameof(OnPlayerTouchedExitArea));
		}

		node_audioStreamPlayer.VolumeDb = -30f;
		node_audioStreamPlayer.Play();
		node_fader.FadeToTransparent();

		node_camera.ForceUpdateScroll();

		GetTree().Paused = true;
	}

	public override void _PhysicsProcess (float delta)
	{
		base._PhysicsProcess(delta);

		m_state.OnPhysicsProcess(delta);
	}

	public override void _Process (float delta)
	{
		base._Process(delta);

		m_state.OnProcess(delta);

		if (Input.IsActionJustPressed("pause"))
		{
			OnPaused();
		}
	}

	#endregion // Godot methods



	#region Public methods

	public virtual bool HasLevelBeenCompleted () => true;

	public void SetState (LevelState state, bool forceApply = false)
	{
		if (m_state.StateName == state.StateName && !forceApply) return;

		m_state.OnExit();
		m_state = state;
		m_state.OnEnter();
	}

	public void UpdateLayers ()
	{
		void __UpdateLayers (Node2D negativeLayers, Node2D positiveLayers, Camera2D camera)
		{
			for (int i = negativeLayers.GetChildCount() - 1; i >= 0; i--)
			{
				ParallaxBackground parallaxBackground = negativeLayers.GetChild<ParallaxBackground>(i);
				parallaxBackground.Transform = new Transform2D(0f, new Vector2((float)camera.LimitRight / 2, 0f));
				parallaxBackground.Offset = parallaxBackground.Transform.origin;
				parallaxBackground.ScrollBaseOffset = new Vector2()
				{
					x = -((parallaxBackground.Transform.origin.x - (GetViewport().Size.x / 2)) * ((float)i / negativeLayers.GetChildCount())),
					y = 0f,
				};

				ParallaxLayer left = parallaxBackground.GetNode<ParallaxLayer>("Left");
				ParallaxLayer center = parallaxBackground.GetNode<ParallaxLayer>("Center");
				ParallaxLayer right = parallaxBackground.GetNode<ParallaxLayer>("Right");

				left.MotionOffset = new Vector2(Mathf.Abs(parallaxBackground.ScrollBaseOffset.x), 0f);
				center.MotionOffset = Vector2.Zero;
				right.MotionOffset = new Vector2(parallaxBackground.ScrollBaseOffset.x, 0f);
			}

			for (int i = 0; i < positiveLayers.GetChildCount(); i++)
			{
				ParallaxBackground parallaxBackground = positiveLayers.GetChild<ParallaxBackground>(i);
				parallaxBackground.Transform = new Transform2D(0f, new Vector2((float)camera.LimitRight / 2, 0f));
				parallaxBackground.Offset = parallaxBackground.Transform.origin;
				parallaxBackground.ScrollBaseOffset = new Vector2()
				{
					x = ((parallaxBackground.Transform.origin.x - (GetViewport().Size.x / 2)) * (1 + ((float)i / 10))),
					y = 0f,
				};

				ParallaxLayer left = parallaxBackground.GetNode<ParallaxLayer>("Left");
				ParallaxLayer center = parallaxBackground.GetNode<ParallaxLayer>("Center");
				ParallaxLayer right = parallaxBackground.GetNode<ParallaxLayer>("Right");

				left.MotionOffset = new Vector2(-parallaxBackground.ScrollBaseOffset.x, 0f);
				center.MotionOffset = Vector2.Zero;
				right.MotionOffset = new Vector2(parallaxBackground.ScrollBaseOffset.x, 0f);
			}
		}

		__UpdateLayers(
			GetNode<Node2D>("NegativeLayers"),
			GetNode<Node2D>("PositiveLayers"),
			node_bonez.GetNode<Camera2D>("Camera2D"));
	}

	#endregion // Public methods




	#region Protected methods

	protected void ReturnToOverworld ()
	{
		GetTree().Paused = true;

		if (!node_fader.IsConnected(nameof(Fader.OnFadedToOpaqueSignal), this, nameof(LoadOverworldScene)))
			node_fader.Connect(nameof(Fader.OnFadedToOpaqueSignal), this, nameof(LoadOverworldScene));

		node_fader.FadeToOpaque();
	}

	protected void ReturnToTitle ()
	{
		GetTree().Paused = true;

		if (!node_fader.IsConnected(nameof(Fader.OnFadedToOpaqueSignal), this, nameof(LoadTitleScene)))
			node_fader.Connect(nameof(Fader.OnFadedToOpaqueSignal), this, nameof(LoadTitleScene));

		node_fader.FadeToOpaque();
	}

	protected void QuitToDesktop ()
	{
		GetTree().Paused = true;

		if (!node_fader.IsConnected(nameof(Fader.OnFadedToOpaqueSignal), this, nameof(LoadQuitToDesktop)))
			node_fader.Connect(nameof(Fader.OnFadedToOpaqueSignal), this, nameof(LoadQuitToDesktop));

		node_fader.FadeToOpaque();
	}

	#endregion // Protected methods



	#region Private methods

	private void LoadOverworldScene ()
	{
		GetTree().ChangeScene("res://assets/scenes/overworld_scene.tscn");
	}
	private void LoadTitleScene ()
	{
		GetTree().ChangeScene("res://assets/scenes/intro_scene.tscn");
	}
	private void LoadQuitToDesktop ()
	{
		GetTree().Quit();
	}

	private void Save ()
	{
		LevelSaver.Instance.SaveLevelData(this);
	}

	#endregion // Private methods



	#region Callbacks

	protected virtual void OnPaused ()
	{
		GetTree().Paused = true;
		node_pauseMenu.Show();
	}

	protected virtual void OnResumed ()
	{
		GetTree().Paused = false;
		node_pauseMenu.Hide();
	}

	protected virtual void OnPlayerHealthChanged (Bonez player, int oldHealth, int newHealth) { }
	protected virtual void OnPlayerHealthReachedZero (Bonez player)
	{
		ReturnToOverworld();
	}
	protected virtual void OnPlayerMaxHealthChanged (Bonez player, int oldMaxHealth, int newMaxHealth) { }
	protected virtual void OnPlayerFireRateTimerChanged (Bonez player, float fireRate, float fireTimer) { }
	protected virtual void OnPlayerTouchedExitArea ()
	{
		Save();
		ReturnToOverworld();
	}

	protected virtual void OnFadingToTransparent (float per)
	{
		node_audioStreamPlayer.VolumeDb = -30f * (1f - per);
		if (per == 1) node_audioStreamPlayer.VolumeDb = 0f;
	}
	protected virtual void OnFadedToTransparent ()
	{
		GetTree().Paused = false;
	}
	protected virtual void OnFadingToOpaque (float per)
	{
		node_audioStreamPlayer.VolumeDb = -30f * per;
		if (per == 1) node_audioStreamPlayer.VolumeDb = -30f;
	}
	protected virtual void OnFadedToOpaque ()
	{
		GetTree().Paused = false;
	}

	#endregion // Callbacks

}
