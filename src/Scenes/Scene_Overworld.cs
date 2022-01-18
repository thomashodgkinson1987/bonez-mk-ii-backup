using Godot;

public class Scene_Overworld : Node2D
{

	#region Nodes

	public AudioStreamPlayer node_audioStreamPlayer { get; private set; }

	public Node2D node_levelNodes { get; private set; }
	public Node2D node_currentLevelNodeIndicator { get; private set; }

	public OverworldHUD node_hud { get; private set; }
	public Fader node_fader { get; private set; }

	public OverworldPauseMenu node_pauseMenu { get; private set; }

	public Camera2D node_camera { get; private set; }

	#endregion // Nodes



	#region Exports

	[Export] public float IndicatorMoveTime { get; set; } = 0.5f;

	#endregion // Exports



	#region Properties

	private string _currentLevelID = "level_0001";
	public string CurrentLevelID
	{
		get => _currentLevelID;
		set
		{
			_currentLevelID = value;
			node_hud.LevelID = _currentLevelID;
		}
	}
	private string _currentLevelName = "Level 1";
	public string CurrentLevelName
	{
		get => _currentLevelName;
		set
		{
			_currentLevelName = value;
			node_hud.LevelName = _currentLevelName;
		}
	}
	private string _currentLevelType = "level";
	public string CurrentLevelType
	{
		get => _currentLevelType;
		set
		{
			_currentLevelType = value;
			node_hud.LevelType = _currentLevelType;
		}
	}

	public OverworldLevelNode CurrentLevelNode { get; set; }

	public Vector2 CurrentLevelNodeIndicatorLastPosition { get; set; }
	public Vector2 CurrentLevelNodeIndicatorNextPosition { get; set; }
	public float CurrentLevelNodeIndicatorMoveTimeTimer { get; set; } = 0f;
	public bool IsCurrentLevelNodeIndicatorMoving { get; set; } = false;

	#endregion // Properties



	#region Godot methods

	public override void _Ready ()
	{
		base._Ready();

		// Grab nodes

		node_audioStreamPlayer = GetNode<AudioStreamPlayer>("AudioStreamPlayer");
		node_levelNodes = GetNode<Node2D>("LevelNodes");
		node_currentLevelNodeIndicator = GetNode<Node2D>("CurrentLevelIndicator");
		node_hud = GetNode<OverworldHUD>("HUD");
		node_fader = GetNode<Fader>("Fader");
		node_pauseMenu = GetNode<OverworldPauseMenu>("PauseMenu");
		node_camera = GetNode<Camera2D>("CurrentLevelIndicator/Camera2D");

		node_pauseMenu.Connect(nameof(OverworldPauseMenu.OnResumeButtonPressedSignal), this, nameof(OnResumed));
		node_pauseMenu.Connect(nameof(OverworldPauseMenu.OnReturnToTitleButtonPressedSignal), this, nameof(ReturnToTitle));
		node_pauseMenu.Connect(nameof(OverworldPauseMenu.OnQuitToDesktopButtonPressedSignal), this, nameof(QuitToDesktop));

		node_pauseMenu.Hide();

		// Process level nodes

		for (int i = 0; i < node_levelNodes.GetChildCount(); i++)
		{
			OverworldLevelNode node = node_levelNodes.GetChild<OverworldLevelNode>(i);

			node.LeftNode = GetLevelNode(node.LeftNodeName);
			node.RightNode = GetLevelNode(node.RightNodeName);
			node.UpNode = GetLevelNode(node.UpNodeName);
			node.DownNode = GetLevelNode(node.DownNodeName);
		}

		CurrentLevelNode = node_levelNodes.GetChild<OverworldLevelNode>(0);

		LevelSaver.Instance.LoadOverworld(this);

		for (int i = 0; i < node_levelNodes.GetChildCount(); i++)
		{
			OverworldLevelNode node = node_levelNodes.GetChild<OverworldLevelNode>(i);

			if (node.State == OverworldLevelNode.EState.Complete)
			{
				void __ProcessNode (OverworldLevelNode __node)
				{
					if (__node != null && __node.State == OverworldLevelNode.EState.Locked)
					{
						__node.State = OverworldLevelNode.EState.Unlocked;
					}
				}

				__ProcessNode(node.LeftNode);
				__ProcessNode(node.RightNode);
				__ProcessNode(node.UpNode);
				__ProcessNode(node.DownNode);
			}
		}

		for (int i = 0; i < node_levelNodes.GetChildCount(); i++)
		{
			OverworldLevelNode levelNode = node_levelNodes.GetChild<OverworldLevelNode>(i);

			levelNode.UpdateGraphicFromState();
			if (levelNode.LevelID == CurrentLevelID) CurrentLevelNode = levelNode;
		}

		CurrentLevelNodeIndicatorLastPosition = CurrentLevelNode.GlobalPosition;
		CurrentLevelNodeIndicatorNextPosition = CurrentLevelNode.GlobalPosition;
		node_currentLevelNodeIndicator.GlobalPosition = CurrentLevelNode.GlobalPosition;

		node_fader.Connect(nameof(Fader.OnFadingToTransparentSignal), this, nameof(OnFadingToTransparent));
		node_fader.Connect(nameof(Fader.OnFadedToTransparentSignal), this, nameof(OnFadedToTransparent));
		node_fader.Connect(nameof(Fader.OnFadingToOpaqueSignal), this, nameof(OnFadingToOpaque));
		node_fader.Connect(nameof(Fader.OnFadedToOpaqueSignal), this, nameof(OnFadedToOpaque));

		node_audioStreamPlayer.VolumeDb = -30f;
		node_audioStreamPlayer.Play();
		node_fader.FadeToTransparent();

		//node_camera.ForceUpdateTransform();
		node_camera.ForceUpdateScroll();

		GetTree().Paused = true;
	}

	public override void _Process (float delta)
	{
		base._Process(delta);

		if (IsCurrentLevelNodeIndicatorMoving)
			MoveIndicatorToNextNode(delta);
		else
			HandleInput();
	}

	#endregion // Godot methods



	#region Public methods

	public void LoadLevel ()
	{
		Save();
		node_fader.Connect(nameof(Fader.OnFadedToOpaqueSignal), this, nameof(OnLoadLevel));
		node_fader.FadeToOpaque();
		GetTree().Paused = true;
	}

	public OverworldLevelNode GetLevelNode (string levelNodeName)
	{
		for (int i = 0; i < node_levelNodes.GetChildCount(); i++)
		{
			OverworldLevelNode levelNode = node_levelNodes.GetChild<OverworldLevelNode>(i);
			if (levelNode.Name == levelNodeName) return levelNode;
		}

		return null;
	}

	#endregion // Public methods



	#region Private methods

	private void MoveToLevelNode (OverworldLevelNode levelNode)
	{
		if (levelNode != null)
		{
			switch (levelNode.State)
			{
				case OverworldLevelNode.EState.Locked:
				break;

				case OverworldLevelNode.EState.Unlocked:
				CurrentLevelNodeIndicatorLastPosition = CurrentLevelNode.GlobalPosition;
				CurrentLevelNode = levelNode;
				CurrentLevelID = CurrentLevelNode.LevelID;

				{
					var packedScene = ResourceLoader.Load<PackedScene>($"res://assets/packed_scenes/levels/{CurrentLevelID}.tscn");
					var instance = packedScene.Instance();
					CurrentLevelName = ((Level)instance).LevelName;

					if (instance is Level_CollectBones)
						CurrentLevelType = "Collect Bones";
					else if (instance is Level_KingSlimeArena)
						CurrentLevelType = "King Slime Arena";
					else if (instance is Level_NecroArena)
						CurrentLevelType = "Necro Arena";
					else
						CurrentLevelType = "Default";

					instance.QueueFree();
				}

				CurrentLevelNodeIndicatorNextPosition = CurrentLevelNode.GlobalPosition;
				IsCurrentLevelNodeIndicatorMoving = true;
				break;

				case OverworldLevelNode.EState.Complete:
				CurrentLevelNodeIndicatorLastPosition = CurrentLevelNode.GlobalPosition;
				CurrentLevelNode = levelNode;
				CurrentLevelID = CurrentLevelNode.LevelID;

				{
					var packedScene = ResourceLoader.Load<PackedScene>($"res://assets/packed_scenes/levels/{CurrentLevelID}.tscn");
					var instance = packedScene.Instance();
					CurrentLevelName = ((Level)instance).LevelName;

					if (instance is Level_CollectBones)
						CurrentLevelType = "Collect Bones";
					else if (instance is Level_KingSlimeArena)
						CurrentLevelType = "King Slime Arena";
					else if (instance is Level_NecroArena)
						CurrentLevelType = "Necro Arena";
					else
						CurrentLevelType = "Default";

					instance.QueueFree();
				}

				CurrentLevelNodeIndicatorNextPosition = CurrentLevelNode.GlobalPosition;
				IsCurrentLevelNodeIndicatorMoving = true;
				break;

				default:
				break;
			}
		}
	}

	private void HandleInput ()
	{
		if (Input.IsActionJustPressed("pause"))
		{
			OnPaused();
			return;
		}

		if (Input.IsActionJustPressed("left"))
			MoveToLevelNode(CurrentLevelNode.LeftNode);
		else if (Input.IsActionJustPressed("right"))
			MoveToLevelNode(CurrentLevelNode.RightNode);
		else if (Input.IsActionJustPressed("up"))
			MoveToLevelNode(CurrentLevelNode.UpNode);
		else if (Input.IsActionJustPressed("down"))
			MoveToLevelNode(CurrentLevelNode.DownNode);
		else if (
			Input.IsActionJustPressed("ui_accept"))
		{
			LoadLevel();
		}
	}

	private void MoveIndicatorToNextNode (float delta)
	{
		CurrentLevelNodeIndicatorMoveTimeTimer += delta;

		if (CurrentLevelNodeIndicatorMoveTimeTimer < IndicatorMoveTime)
		{
			float perMoved = CurrentLevelNodeIndicatorMoveTimeTimer / IndicatorMoveTime;
			node_currentLevelNodeIndicator.GlobalPosition =
				CurrentLevelNodeIndicatorLastPosition.LinearInterpolate(CurrentLevelNodeIndicatorNextPosition, perMoved);
		}
		else
		{
			node_currentLevelNodeIndicator.GlobalPosition = CurrentLevelNodeIndicatorNextPosition;
			IsCurrentLevelNodeIndicatorMoving = false;
			CurrentLevelNodeIndicatorMoveTimeTimer = 0f;

			CurrentLevelID = CurrentLevelNode.LevelID;
			var packedScene = ResourceLoader.Load<PackedScene>($"res://assets/packed_scenes/levels/{CurrentLevelID}.tscn");
			var instance = packedScene.Instance();
			CurrentLevelName = ((Level)instance).LevelName;
			instance.QueueFree();
		}
	}

	#endregion // Private methods



	#region

	private void ReturnToTitle ()
	{
		GetTree().Paused = true;
		node_fader.Connect(nameof(Fader.OnFadedToOpaqueSignal), this, nameof(LoadTitleScene));
		node_fader.FadeToOpaque();
	}

	private void QuitToDesktop ()
	{
		GetTree().Paused = true;
		node_fader.Connect(nameof(Fader.OnFadedToOpaqueSignal), this, nameof(LoadQuitToDesktop));
		node_fader.FadeToOpaque();
	}

	private void OnLoadLevel ()
	{
		GetTree().ChangeScene($"res://assets/packed_scenes/levels/{CurrentLevelID}.tscn");
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
		LevelSaver.Instance.SaveOverworld(this);
	}

	#endregion



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

	private void OnFadedToOpaque ()
	{
		GetTree().Paused = false;
	}
	private void OnFadedToTransparent ()
	{
		GetTree().Paused = false;
	}
	private void OnFadingToTransparent (float per)
	{
		node_audioStreamPlayer.VolumeDb = -30f * (1f - per);
		if (per == 1) node_audioStreamPlayer.VolumeDb = 0f;
	}
	private void OnFadingToOpaque (float per)
	{
		node_audioStreamPlayer.VolumeDb = -30f * per;
		if (per == 1) node_audioStreamPlayer.VolumeDb = -30f;
	}

	#endregion // Callbacks

}
