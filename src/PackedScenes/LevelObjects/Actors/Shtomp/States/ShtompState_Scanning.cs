public class ShtompState_Scanning : ShtompState
{

	#region Properties

	public override string StateName => "scanning";

	#endregion // Properties



	#region Constructors

	public ShtompState_Scanning (Actor actor, Shtomp shtomp) : base(actor, shtomp) { }

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
	}

	public override void OnProcess (float delta)
	{
		base.OnProcess(delta);

		if (m_shtomp.AreAnyActorsInRange())
		{
			m_shtomp.SetState(m_shtomp.TargetFoundState);
		}
	}

	#endregion // Public methods

}
