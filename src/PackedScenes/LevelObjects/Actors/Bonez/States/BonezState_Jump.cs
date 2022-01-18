public class BonezState_Jump : BonezState_OffGround
{

	#region Properties

	public override string StateName => "jump";

	public int JumpCount { get; set; } = 0;
	public int JumpLimit { get; set; } = 2;
	public float JumpHeight { get; set; } = 46f;

	#endregion // Properties



	#region Constructors

	public BonezState_Jump (Actor actor, Bonez bonez) : base(actor, bonez) { }

	#endregion // Constructors



	#region State methods

	public override void OnEnter ()
	{
		base.OnEnter();

		m_bonez.Jump();
	}

	public override void OnExit ()
	{
		base.OnExit();
	}

	public override void OnPhysicsProcess (float delta)
	{
		base.OnPhysicsProcess(delta);

		if (m_bonez.Velocity.y >= 0)
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
