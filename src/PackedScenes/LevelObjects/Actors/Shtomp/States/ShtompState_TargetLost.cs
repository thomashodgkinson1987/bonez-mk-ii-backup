public class ShtompState_TargetLost : ShtompState
{

	#region Properties

	public override string StateName => "target_lost";

	#endregion // Properties



	#region Constructors

	public ShtompState_TargetLost (Actor actor, Shtomp shtomp) : base(actor, shtomp) { }

	#endregion // Constructors



	#region Public methods

	public override void OnEnter ()
	{
		base.OnEnter();

		m_shtomp.SetState(m_shtomp.WaitingToScanState);
	}

	public override void OnExit ()
	{
		base.OnExit();
	}

	public override void OnPhysicsProcess (float delta)
	{
		base.OnPhysicsProcess(delta);
	}

	public override void OnProcess (float delta)
	{
		base.OnProcess(delta);
	}

	#endregion // Public methods

}
