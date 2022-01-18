public class ShtompState_Init : ShtompState
{

	#region Properties

	public override string StateName => "init";

	#endregion // Properties



	#region Constructors

	public ShtompState_Init (Actor actor, Shtomp shtomp) : base(actor, shtomp) { }

	#endregion // Constructors



	#region Public methods

	public override void OnEnter ()
	{
		base.OnEnter();

		m_shtomp.ReturnGlobalPosition = m_shtomp.GlobalPosition;
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

		m_shtomp.SetState(m_shtomp.ScanningState);
	}

	#endregion // Public methods

}
