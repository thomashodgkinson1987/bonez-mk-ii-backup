public class ShtompState_WaitingToScan : ShtompState
{

	#region Properties

	public float TimeToWait { get; set; } = 1f;
	public float TimeToWaitTimer { get; set; } = 0f;

	public override string StateName => "waiting_to_scan";

	#endregion // Properties



	#region Constructors

	public ShtompState_WaitingToScan (Actor actor, Shtomp shtomp) : base(actor, shtomp) { }

	#endregion // Constructors



	#region Public methods

	public override void OnEnter ()
	{
		base.OnEnter();

		TimeToWaitTimer = 0f;
	}

	public override void OnExit ()
	{
		base.OnExit();

		TimeToWaitTimer = 0f;
	}

	public override void OnPhysicsProcess (float delta)
	{
		base.OnPhysicsProcess(delta);
	}

	public override void OnProcess (float delta)
	{
		base.OnProcess(delta);

		TimeToWaitTimer += delta;
		if (TimeToWaitTimer >= TimeToWait)
		{
			m_shtomp.SetState(m_shtomp.ScanningState);
		}
	}

	#endregion // Public methods

}
