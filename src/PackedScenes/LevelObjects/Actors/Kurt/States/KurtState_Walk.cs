using Godot;

public class KurtState_Walk : KurtState_Alive
{

	#region Properties

	public override string StateName => "walk";

	#endregion // Properties



	#region Constructors

	public KurtState_Walk (Actor actor, Kurt kurt) : base(actor, kurt) { }

	#endregion // Constructors



	#region Public methods

	public override void OnEnter ()
	{
		base.OnEnter();

		m_kurt.Velocity = new Vector2(m_kurt.WalkDirection * m_kurt.WalkSpeed, m_kurt.Velocity.y);
		m_kurt.Snap = Vector2.Down * 16f;
		m_kurt.WalkTimeTimer = 0f;
	}

	public override void OnExit ()
	{
		base.OnExit();

		m_kurt.WalkDirection *= -1;
		m_kurt.WalkTimeTimer = 0f;
	}

	public override void OnPhysicsProcess (float delta)
	{
		base.OnPhysicsProcess(delta);

		m_kurt.Velocity = new Vector2(m_kurt.WalkDirection * m_kurt.WalkSpeed, m_kurt.Velocity.y);

		m_kurt.WalkTimeTimer += delta;

		if (m_kurt.WalkTimeTimer >= m_kurt.WalkTime)
		{
			m_kurt.SetState(m_kurt.IdleState);
		}
	}

	public override void OnProcess (float delta)
	{
		base.OnProcess(delta);
	}

	#endregion // Public methods

}
