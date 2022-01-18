public class BonezState_Die : BonezState
{

	#region Properties

	public override string StateName => "die";

	#endregion // Properties



	#region Constructors

	public BonezState_Die (Actor actor, Bonez bonez) : base(actor, bonez) { }

	#endregion // Constructors



	#region State methods

	public override void OnEnter ()
	{
		base.OnEnter();
		m_bonez.node_audioPlayer_die.Play();
	}

	public override void OnExit ()
	{
		base.OnExit();
	}

	public override void OnPhysicsProcess (float delta)
	{
		base.OnPhysicsProcess(delta);

		m_bonez.ApplyGravity(delta / 2);
		m_bonez.ApplyDeceleration(delta / 2);
		m_bonez.Move();
		m_bonez.ApplyGravity(delta / 2);
		m_bonez.ApplyDeceleration(delta / 2);
	}

	public override void OnProcess (float delta)
	{
		base.OnProcess(delta);
	}

	#endregion // State methods

}
