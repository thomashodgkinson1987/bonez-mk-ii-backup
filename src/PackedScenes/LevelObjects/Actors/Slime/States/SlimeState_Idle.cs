using Godot;

public class SlimeState_Idle : SlimeState
{

	#region Properties

	public override string StateName => "idle";

	#endregion // Properties



	#region Constructors

	public SlimeState_Idle (Actor actor, Slime slime) : base(actor, slime) { }

	#endregion // Constructors



	#region Public methods

	public override void OnEnter ()
	{
		base.OnEnter();

		m_slime.Velocity = Vector2.Zero;
		m_slime.Snap = Vector2.Down * 16f;
		m_slime.IdleTimeTimer = 0f;
	}

	public override void OnExit ()
	{
		base.OnExit();

		m_slime.IdleTimeTimer = 0f;
	}

	public override void OnPhysicsProcess (float delta)
	{
		base.OnPhysicsProcess(delta);

		if (m_slime.Health == 0)
		{
			m_slime.SetState(m_slime.DieState);
		}
		else if (!m_slime.IsOnFloor())
		{
			m_slime.SetState(m_slime.FallState);
		}
		else
		{
			m_slime.IdleTimeTimer += delta;

			if (m_slime.IdleTimeTimer >= m_slime.IdleTime)
			{
				m_slime.SetState(m_slime.WalkState);
			}
		}
	}

	public override void OnProcess (float delta)
	{
		base.OnProcess(delta);
	}

	#endregion // Public methods

}
