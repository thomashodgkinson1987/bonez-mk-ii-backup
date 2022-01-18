using Godot;

public class NecroState_Idle : NecroState
{

	#region Properties

	public override string StateName => "idle";

	#endregion // Properties



	#region Constructors

	public NecroState_Idle (Actor actor, Necro necro) : base(actor, necro) { }

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

		if (m_necro.Health == 0) m_necro.SetState(m_necro.DieState);
	}

	public override void OnProcess (float delta)
	{
		base.OnProcess(delta);
	}

	#endregion // Public methods

}
