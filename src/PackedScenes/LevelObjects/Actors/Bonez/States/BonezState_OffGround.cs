using Godot;

public abstract class BonezState_OffGround : BonezState_Alive
{

	#region Constructors

	protected BonezState_OffGround (Actor actor, Bonez bonez) : base(actor, bonez) { }

	#endregion // Constructors



	#region State methods

	public override void OnEnter ()
	{
		base.OnEnter();

		m_actor.Snap = Vector2.Zero;
	}

	public override void OnExit ()
	{
		base.OnExit();
	}

	public override void OnPhysicsProcess (float delta)
	{
		base.OnPhysicsProcess(delta);

		if (m_bonez.IsOnFloor())
		{
			if (m_bonez.Velocity.x == 0)
				m_bonez.SetState(m_bonez.IdleState);
			else
				m_bonez.SetState(m_bonez.WalkState);
		}
	}

	public override void OnProcess (float delta)
	{
		base.OnProcess(delta);
	}

	#endregion // State methods

}
