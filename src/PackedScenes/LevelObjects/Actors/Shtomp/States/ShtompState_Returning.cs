using Godot;

public class ShtompState_Returning : ShtompState
{

	#region Properties

	public float Speed { get; set; } = 32f;

	public override string StateName => "returning";

	#endregion // Properties



	#region Constructors

	public ShtompState_Returning (Actor actor, Shtomp shtomp) : base(actor, shtomp) { }

	#endregion // Constructors



	#region Public methods

	public override void OnEnter ()
	{
		base.OnEnter();

		m_shtomp.Velocity = new Vector2(0f, -Speed);
	}

	public override void OnExit ()
	{
		base.OnExit();
	}

	public override void OnPhysicsProcess (float delta)
	{
		base.OnPhysicsProcess(delta);

		m_shtomp.Move();
	}

	public override void OnProcess (float delta)
	{
		base.OnProcess(delta);

		if (m_shtomp.GlobalPosition.y < m_shtomp.ReturnGlobalPosition.y)
		{
			m_shtomp.SetState(m_shtomp.ReturnedState);
		}
	}

	#endregion // Public methods

}
