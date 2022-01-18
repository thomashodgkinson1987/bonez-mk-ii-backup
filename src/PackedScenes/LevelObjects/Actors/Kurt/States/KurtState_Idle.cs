using Godot;

public class KurtState_Idle : KurtState_Alive
{

	#region Properties

	public override string StateName => "idle";

	#endregion // Properties



	#region Constructors

	public KurtState_Idle (Actor actor, Kurt kurt) : base(actor, kurt) { }

	#endregion // Constructors



	#region Public methods

	public override void OnEnter ()
	{
		base.OnEnter();

		m_kurt.Velocity = new Vector2(0f, m_kurt.Velocity.y);
		m_kurt.Snap = Vector2.Down * 8f;
		m_kurt.IdleTimeTimer = 0f;
	}

	public override void OnExit ()
	{
		base.OnExit();

		m_kurt.IdleTimeTimer = 0f;
	}

	public override void OnPhysicsProcess (float delta)
	{
		base.OnPhysicsProcess(delta);

		m_kurt.IdleTimeTimer += delta;

		if (m_kurt.IdleTimeTimer >= m_kurt.IdleTime)
		{
			m_kurt.SetState(m_kurt.WalkState);
		}
	}

	public override void OnProcess (float delta)
	{
		base.OnProcess(delta);
	}

	#endregion // Public methods

}
