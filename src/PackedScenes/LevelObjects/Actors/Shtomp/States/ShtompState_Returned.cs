using Godot;

public class ShtompState_Returned : ShtompState
{

	#region Properties

	public override string StateName => "returned";

	#endregion // Properties



	#region Constructors

	public ShtompState_Returned (Actor actor, Shtomp shtomp) : base(actor, shtomp) { }

	#endregion // Constructors



	#region Public methods

	public override void OnEnter ()
	{
		base.OnEnter();

		m_shtomp.Velocity = Vector2.Zero;
		m_shtomp.GlobalPosition = m_shtomp.ReturnGlobalPosition;
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
