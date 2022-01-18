using Godot;

public class TestScene : Node2D
{

	private bool m_wasDoFrame = false;
	private bool m_isDoFrame = false;

	private float m_frameCounter = 0;

	[Signal] private delegate void TimeoutSignal ();

	public override void _Ready ()
	{
		base._Ready();

		GetTree().Paused = true;
	}

	public override void _PhysicsProcess (float delta)
	{
		base._PhysicsProcess(delta);

		if (m_isDoFrame)
		{
			m_isDoFrame = false;
			m_wasDoFrame = true;

			GetTree().Paused = false;

			m_frameCounter++;
			GD.Print("Frame: " + m_frameCounter.ToString());
		}
		else if (m_wasDoFrame)
		{
			m_wasDoFrame = false;

			GetTree().Paused = true;
		}
	}

	public override void _Input (InputEvent @event)
	{
		base._Input(@event);

		if (@event.IsActionReleased("jump"))
		{
			m_isDoFrame = true;
		}
	}

}
