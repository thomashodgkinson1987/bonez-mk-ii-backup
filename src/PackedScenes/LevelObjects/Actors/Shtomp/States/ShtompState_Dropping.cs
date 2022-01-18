using Godot;

public class ShtompState_Dropping : ShtompState
{

	#region Properties

	public override string StateName => "dropping";

	#endregion // Properties



	#region Constructors

	public ShtompState_Dropping (Actor actor, Shtomp shtomp) : base(actor, shtomp) { }

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

		m_shtomp.ApplyGravity(delta / 2);
		m_shtomp.Move();
		m_shtomp.ApplyGravity(delta / 2);
	}

	public override void OnProcess (float delta)
	{
		base.OnProcess(delta);

		if (m_shtomp.IsOnFloor())
		{
			m_shtomp.SetState(m_shtomp.LandedState);
		}
	}

	#endregion // Public methods

}
