public class BonezState_Idle : BonezState_OnGround
{

	#region Properties

	public override string StateName => "idle";

	#endregion // Properties



	#region Constructors

	public BonezState_Idle (Actor actor, Bonez bonez) : base(actor, bonez) { }

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

		if (m_bonez.Velocity.x != 0)
		{
			m_bonez.SetState(m_bonez.WalkState);
		}
	}

	public override void OnProcess (float delta)
	{
		base.OnProcess(delta);
	}

	#endregion // State methods

}
