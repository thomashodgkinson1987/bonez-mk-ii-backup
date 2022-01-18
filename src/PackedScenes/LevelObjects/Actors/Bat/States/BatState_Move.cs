using Godot;

public class BatState_Move : BatState
{

	public override string StateName => "move";

	public BatState_Move (Actor actor, Bat bat) : base(actor, bat) { }

	public override void OnEnter ()
	{
		int frame = m_bat.node_animatedSprite.Frame;

		base.OnEnter();

		m_bat.node_animatedSprite.Frame = frame;

		m_bat.MoveTimer = 0;
		m_bat.LastTargetPosition = m_bat.GlobalPosition;
	}

	public override void OnExit ()
	{
		base.OnExit();

		m_bat.MoveTimer = 0;
	}

	public override void OnPhysicsProcess (float delta)
	{
		base.OnPhysicsProcess(delta);

		m_bat.MoveTimer += delta;

		if (m_bat.Target != null)
		{
			m_bat.LastTargetPosition = m_bat.Target.GlobalPosition;
		}

		m_bat.GlobalPosition = m_bat.GlobalPosition.MoveToward(m_bat.LastTargetPosition, m_bat.MoveSpeed * delta);

		if (m_bat.MoveTimer >= m_bat.MoveTime)
		{
			m_bat.SetState(m_bat.IdleState);
		}
	}

	public override void OnProcess (float delta)
	{
		base.OnProcess(delta);
	}

}
