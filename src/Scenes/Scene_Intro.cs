using Godot;

public class Scene_Intro : Control
{

	#region Nodes

	public RichTextLabel node_skipNode { get; private set; }

	#endregion // Nodes



	#region Fields

	private bool m_isSkipShown = false;
	private bool m_canSkip = false;

	private float m_waitBeforeSkip = 0.25f;
	private float m_waitTimer = 0f;

	private float m_timeToFadeAway = 2f;
	private float m_timeToFadeAwayTimer = 0f;

	#endregion // Fields



	#region Godot methods

	public override void _Ready ()
	{
		base._Ready();

		node_skipNode = GetNode<RichTextLabel>("Skip");

		node_skipNode.Visible = false;
	}

	public override void _Process (float delta)
	{
		base._Process(delta);

		if (m_isSkipShown)
		{
			m_waitTimer += delta;
			if (m_waitTimer >= m_waitBeforeSkip)
			{
				m_canSkip = true;
			}

			m_timeToFadeAwayTimer += delta;
			if (m_timeToFadeAwayTimer >= m_timeToFadeAway)
			{
				m_timeToFadeAwayTimer = 0f;
				m_canSkip = false;
				m_waitTimer = 0f;
				m_isSkipShown = false;
				node_skipNode.Visible = false;
			}
		}
	}

	public override void _Input (InputEvent @event)
	{
		base._Input(@event);

		if (@event.IsActionPressed("jump")
			|| @event.IsActionPressed("attack")
			|| @event.IsActionPressed("pause")
			|| @event.IsActionPressed("start"))
		{
			m_timeToFadeAwayTimer = 0f;

			if (!m_isSkipShown)
			{
				m_isSkipShown = true;
				node_skipNode.Visible = true;
			}
			else if (m_canSkip)
			{
				m_timeToFadeAwayTimer = 0f;
				m_canSkip = false;
				m_waitTimer = 0f;
				m_isSkipShown = false;
				node_skipNode.Visible = false;
				ExitToTitleScene();
			}
		}
	}

	#endregion // Godot methods



	#region Public methods

	public void ExitToTitleScene ()
	{
		GetTree().ChangeScene("res://assets/scenes/main_menu_scene.tscn");
	}

	#endregion // Public methods

}
