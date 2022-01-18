public abstract class BonezState_Alive : BonezState
{

	#region Constructors

	protected BonezState_Alive (Actor actor, Bonez bonez) : base(actor, bonez) { }

	#endregion // Constructors



	#region State methods

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

		if (m_bonez.Health == 0)
		{
			m_bonez.SetState(m_bonez.DieState);
		}
		else if (m_bonez.TookDamageThisFrame)
		{
			m_bonez.SetState(m_bonez.HitState);
		}
		else if (m_bonez.IsJump)
		{
			m_bonez.SetState(m_bonez.JumpState, true);
		}
		else
		{
			m_bonez.ApplyGravity(delta / 2);
			m_bonez.ApplyDeceleration(delta / 2);
			m_bonez.ResolveInput(delta);

			m_bonez.Move();

			m_bonez.ApplyGravity(delta / 2);
			m_bonez.ApplyDeceleration(delta / 2);
			m_bonez.ResolveInput(delta);

			m_bonez.UpdateFacingDirection();
		}
	}

	public override void OnProcess (float delta)
	{
		base.OnProcess(delta);

		m_bonez.PollInput();
	}

	#endregion // State methods

}
