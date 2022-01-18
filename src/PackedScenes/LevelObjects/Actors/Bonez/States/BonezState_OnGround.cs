using Godot;

public abstract class BonezState_OnGround : BonezState_Alive
{

	#region Constructors

	protected BonezState_OnGround (Actor actor, Bonez bonez) : base(actor, bonez) { }

	#endregion // Constructors



	#region State methods

	public override void OnEnter ()
	{
		base.OnEnter();

		m_bonez.Snap = Vector2.Down * 4f;
		m_bonez.JumpCount = 0;
	}

	public override void OnExit ()
	{
		base.OnExit();
	}

	public override void OnPhysicsProcess (float delta)
	{
		base.OnPhysicsProcess(delta);

		if (!m_bonez.IsOnFloor() && m_bonez.Velocity.y > 0)
		{
			m_bonez.SetState(m_bonez.FallState);
		}
	}

	public override void OnProcess (float delta)
	{
		base.OnProcess(delta);
	}

	#endregion // State methods

}
