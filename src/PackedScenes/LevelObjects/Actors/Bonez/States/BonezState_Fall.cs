public class BonezState_Fall : BonezState_OffGround
{

	#region Properties

	public override string StateName => "fall";

	#endregion // Properties



	#region Constructors

	public BonezState_Fall (Actor actor, Bonez bonez) : base(actor, bonez) { }

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

		if (m_bonez.IsJump)
		{
			m_bonez.SetState(m_bonez.JumpState, true);
		}
	}

	public override void OnProcess (float delta)
	{
		base.OnProcess(delta);
	}

	#endregion // State methods

}
