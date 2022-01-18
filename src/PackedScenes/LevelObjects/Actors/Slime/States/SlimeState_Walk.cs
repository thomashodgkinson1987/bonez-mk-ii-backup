using Godot;

public class SlimeState_Walk : SlimeState
{

	#region Properties

	public override string StateName => "walk";

	#endregion // Properties



	#region Constructors

	public SlimeState_Walk (Actor actor, Slime slime) : base(actor, slime) { }

	#endregion // Constructors



	#region Public methods

	public override void OnEnter ()
	{
		base.OnEnter();

		m_slime.Velocity = (Vector2.Right * m_slime.WalkDirection) * m_slime.WalkSpeed;
		m_slime.Snap = Vector2.Down * 16f;
	}

	public override void OnExit ()
	{
		base.OnExit();

		m_slime.WalkDirection *= -1;
	}

	public override void OnPhysicsProcess (float delta)
	{
		base.OnPhysicsProcess(delta);

		if (m_slime.Health == 0)
		{
			m_slime.SetState(m_slime.DieState);
		}
		else if (!m_slime.IsOnFloor())
		{
			m_slime.SetState(m_slime.FallState);
		}
		else
		{
			if (m_slime.WalkDirection == -1 && (m_slime.IsWallLeft() || !m_slime.IsFloorLeft()))
			{
				m_slime.SetState(m_slime.IdleState);
			}
			else if (m_slime.WalkDirection == 1 && (m_slime.IsWallRight() || !m_slime.IsFloorRight()))
			{
				m_slime.SetState(m_slime.IdleState);
			}
			else
			{
				m_slime.Velocity = (Vector2.Right * m_slime.WalkDirection) * m_slime.WalkSpeed;
			}
		}
	}

	public override void OnProcess (float delta)
	{
		base.OnProcess(delta);
	}

	#endregion // Public methods

}
