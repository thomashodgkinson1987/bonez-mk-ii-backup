public abstract class KurtState_Alive : KurtState
{

	#region Constructors

	protected KurtState_Alive (Actor actor, Kurt kurt) : base(actor, kurt) { }

	#endregion // Constructors



	#region Public methods

	public override void OnEnter ()
	{
		base.OnEnter();
	}

	public override void OnExit ()
	{
		base.OnExit();
	}

	public override void OnPhysicsProcess (float delta)
	{
		base.OnPhysicsProcess(delta);

		if (m_kurt.Health == 0)
		{
			m_kurt.SetState(m_kurt.DieState);
		}
		else
		{
			m_kurt.ApplyGravity(delta / 2);
			m_kurt.Move();
			m_kurt.ApplyGravity(delta / 2);

			m_kurt.AttackTimeTimer += delta;

			if (m_kurt.AttackTimeTimer >= m_kurt.AttackTime)
			{
				m_kurt.AttackTimeTimer = 0f;
				m_kurt.Attack();
			}
		}
	}

	public override void OnProcess (float delta)
	{
		base.OnProcess(delta);
	}

	#endregion // Public methods

}
