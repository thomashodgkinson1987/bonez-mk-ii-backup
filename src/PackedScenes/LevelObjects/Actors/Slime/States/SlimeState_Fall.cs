using Godot;

public class SlimeState_Fall : SlimeState
{

	#region Properties

	public override string StateName => "fall";

	#endregion // Properties



	#region Constructors

	public SlimeState_Fall (Actor actor, Slime slime) : base(actor, slime) { }

	#endregion // Constructors



	#region Public methods

	public override void OnEnter ()
	{
		base.OnEnter();

		m_slime.Snap = Vector2.Zero;
	}

	public override void OnExit ()
	{
		base.OnExit();
	}

	public override void OnPhysicsProcess (float delta)
	{
		base.OnPhysicsProcess(delta);

		if (m_slime.Health == 0)
		{
			m_slime.SetState(m_slime.DieState);
		}
		else if (m_slime.IsOnFloor())
		{
			m_slime.SetState(m_slime.IdleState);
		}
	}

	public override void OnProcess (float delta)
	{
		base.OnProcess(delta);
	}

	#endregion // Public methods

}
