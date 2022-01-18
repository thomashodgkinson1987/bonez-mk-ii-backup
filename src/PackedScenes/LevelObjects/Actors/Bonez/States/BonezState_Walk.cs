public class BonezState_Walk : BonezState_OnGround
{

	#region Properties

	public override string StateName => "walk";

	public float Speed { get; set; } = 32f;

	#endregion // Properties



	#region Constructors

	public BonezState_Walk (Actor actor, Bonez bonez) : base(actor, bonez) { }

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

		if (m_bonez.Velocity.x == 0)
		{
			m_bonez.SetState(m_bonez.IdleState);
		}
	}

	public override void OnProcess (float delta)
	{
		base.OnProcess(delta);
	}

	#endregion // State methods

}
