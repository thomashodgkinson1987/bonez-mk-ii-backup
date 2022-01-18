using Godot;

public class Scene_MainMenu : CanvasLayer
{

	#region Nodes

	public AudioStreamPlayer node_audioStreamPlayer { get; private set; }

	public Button node_save1Button { get; private set; }
	public Button node_save2Button { get; private set; }
	public Button node_save3Button { get; private set; }

	public Button node_save1DeleteButton { get; private set; }
	public Button node_save2DeleteButton { get; private set; }
	public Button node_save3DeleteButton { get; private set; }

	public Button node_creditsButton { get; private set; }
	public Button node_exitButton { get; private set; }

	public Fader node_fader { get; private set; }

	#endregion // Nodes



	#region Godot methods

	public override void _Ready ()
	{
		base._Ready();

		node_audioStreamPlayer = GetNode<AudioStreamPlayer>("AudioStreamPlayer");

		node_save1Button = GetNode<Button>("Save1Button");
		node_save2Button = GetNode<Button>("Save2Button");
		node_save3Button = GetNode<Button>("Save3Button");

		node_save1DeleteButton = GetNode<Button>("Save1DeleteButton");
		node_save2DeleteButton = GetNode<Button>("Save2DeleteButton");
		node_save3DeleteButton = GetNode<Button>("Save3DeleteButton");

		node_creditsButton = GetNode<Button>("CreditsButton");
		node_exitButton = GetNode<Button>("ExitButton");

		node_fader = GetNode<Fader>("Fader");

		node_fader.Connect(nameof(Fader.OnFadingToTransparentSignal), this, nameof(OnFadingToTransparent));
		node_fader.Connect(nameof(Fader.OnFadedToTransparentSignal), this, nameof(OnFadedToTransparent));
		node_fader.Connect(nameof(Fader.OnFadingToOpaqueSignal), this, nameof(OnFadingToOpaque));
		node_fader.Connect(nameof(Fader.OnFadedToOpaqueSignal), this, nameof(OnFadedToOpaque));

		node_save1Button.GrabFocus();

		UpdateButtons();

		node_audioStreamPlayer.VolumeDb = -30f;
		node_audioStreamPlayer.Play();
		node_fader.FadeToTransparent();
	}

	#endregion // Godot methods



	#region Private methods

	private void UpdateButtons ()
	{
		if (LevelSaver.Instance.IsSaveSlotInUse(1))
		{
			SaveFile saveData = ResourceLoader.Load<SaveFile>("user://save_1.tres");

			node_save1Button.Text = saveData.SaveName;
			node_save1DeleteButton.Disabled = false;
		}
		else
		{
			node_save1Button.Text = "New Save";
			node_save1DeleteButton.Disabled = true;
		}

		if (LevelSaver.Instance.IsSaveSlotInUse(2))
		{
			SaveFile saveData = ResourceLoader.Load<SaveFile>("user://save_2.tres");
			node_save2Button.Text = saveData.SaveName;
			node_save2DeleteButton.Disabled = false;
		}
		else
		{
			node_save2Button.Text = "New Save";
			node_save2DeleteButton.Disabled = true;
		}

		if (LevelSaver.Instance.IsSaveSlotInUse(3))
		{
			SaveFile saveData = ResourceLoader.Load<SaveFile>("user://save_3.tres");
			node_save3Button.Text = saveData.SaveName;
			node_save3DeleteButton.Disabled = false;
		}
		else
		{
			node_save3Button.Text = "New Save";
			node_save3DeleteButton.Disabled = true;
		}
	}

	private void OnFadedToTransparent ()
	{

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
	private void OnFadedToOpaque ()
	{
		GetTree().ChangeScene("res://assets/scenes/overworld_scene.tscn");
	}

	#endregion // Private methods



	#region Button callbacks

	private void OnSave1ButtonPressed ()
	{
		LevelSaver.Instance.ActiveSaveSlot = 1;
		node_fader.FadeToOpaque();
	}
	private void OnSave2ButtonPressed ()
	{
		LevelSaver.Instance.ActiveSaveSlot = 2;
		node_fader.FadeToOpaque();
	}
	private void OnSave3ButtonPressed ()
	{
		LevelSaver.Instance.ActiveSaveSlot = 3;
		node_fader.FadeToOpaque();
	}

	private void OnSave1DeleteButtonPressed ()
	{
		LevelSaver.Instance.DeleteSaveSlot(1);
		node_save1Button.Text = "New Save";
		node_save1DeleteButton.Disabled = true;
		node_save1Button.GrabFocus();
	}
	private void OnSave2DeleteButtonPressed ()
	{
		LevelSaver.Instance.DeleteSaveSlot(2);
		node_save2Button.Text = "New Save";
		node_save2DeleteButton.Disabled = true;
		node_save2Button.GrabFocus();
	}
	private void OnSave3DeleteButtonPressed ()
	{
		LevelSaver.Instance.DeleteSaveSlot(3);
		node_save3Button.Text = "New Save";
		node_save3DeleteButton.Disabled = true;
		node_save3Button.GrabFocus();
	}

	private async void OnCreditsButtonPressed ()
	{
		node_fader.Disconnect(nameof(Fader.OnFadedToOpaqueSignal), this, nameof(OnFadedToOpaque));
		node_fader.FadeToOpaque();
		await ToSignal(node_fader, nameof(Fader.OnFadedToOpaqueSignal));
		GetTree().ChangeScene("res://assets/scenes/credits_scene.tscn");
	}

	private void OnExitButtonPressed ()
	{
		GetTree().Quit();
	}

	#endregion // Button callbacks

}
