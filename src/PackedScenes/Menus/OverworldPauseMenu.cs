using Godot;

public class OverworldPauseMenu : CanvasLayer
{

	#region Nodes

	public AudioStreamPlayer node_audioStreamPlayer { get; private set; }

	public Panel node_backgroundPanel { get; private set; }

	public Button node_resumeButton { get; private set; }
	public Button node_returnToTitleButton { get; private set; }
	public Button node_quitToDesktopButton { get; private set; }

	#endregion // Nodes



	#region Signals

	[Signal] public delegate void OnResumeButtonPressedSignal ();
	[Signal] public delegate void OnReturnToTitleButtonPressedSignal ();
	[Signal] public delegate void OnQuitToDesktopButtonPressedSignal ();

	#endregion // Signals



	#region Fields

	private bool m_isFirstRun = true;

	#endregion // Fields



	#region Godot methods

	public override void _Ready ()
	{
		base._Ready();

		node_audioStreamPlayer = GetNode<AudioStreamPlayer>("AudioStreamPlayer");

		node_backgroundPanel = GetNode<Panel>("BackgroundPanel");

		node_resumeButton = GetNode<Button>("BackgroundPanel/MenuPanel/ButtonsVBoxContainer/ResumeButton");
		node_returnToTitleButton = GetNode<Button>("BackgroundPanel/MenuPanel/ButtonsVBoxContainer/ReturnToTitleButton");
		node_quitToDesktopButton = GetNode<Button>("BackgroundPanel/MenuPanel/ButtonsVBoxContainer/QuitToDesktopButton");

		node_resumeButton.GrabFocus();
	}

	#endregion // Godot methods



	#region Public methods

	public void Show ()
	{
		node_backgroundPanel.Visible = true;
		node_resumeButton.GrabFocus();
	}

	public void Hide ()
	{
		node_backgroundPanel.Visible = false;
	}

	#endregion // Public methods



	#region Callback methods

	private void OnResumeButtonPressed ()
	{
		EmitSignal(nameof(OnResumeButtonPressedSignal));
	}
	private void OnReturnToTitleButtonPressed ()
	{
		EmitSignal(nameof(OnReturnToTitleButtonPressedSignal));
	}
	private void OnQuitToDesktopButtonPressed ()
	{
		EmitSignal(nameof(OnQuitToDesktopButtonPressedSignal));
	}

	private void OnButtonFocusEntered ()
	{
		if (m_isFirstRun)
		{
			m_isFirstRun = false;
			return;
		}

		node_audioStreamPlayer.Play();
	}

	#endregion // Callback methods

}
