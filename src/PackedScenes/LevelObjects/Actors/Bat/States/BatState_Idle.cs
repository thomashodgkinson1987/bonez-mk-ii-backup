using Godot;

public class BatState_Idle : BatState
{

	public override string StateName => "idle";

	public BatState_Idle (Actor actor, Bat bat) : base(actor, bat) { }

	public override void OnEnter ()
	{
		int frame = m_bat.node_animatedSprite.Frame;

		base.OnEnter();

		m_bat.node_animatedSprite.Frame = frame;

		m_bat.IdleTimer = 0;
	}

	public override void OnExit ()
	{
		base.OnExit();

		m_bat.IdleTimer = 0;
	}

	public override void OnPhysicsProcess (float delta)
	{
		base.OnPhysicsProcess(delta);

		if (m_bat.Target == null)
		{
			m_bat.IdleTimer = 0;
		}

		m_bat.IdleTimer += delta;

		if (m_bat.IdleTimer >= m_bat.IdleTime)
		{
			m_bat.IdleTimer = 0;
			if (m_bat.Target != null)
			{
				m_bat.SetState(m_bat.MoveState);
			}
		}
	}

	public override void OnProcess (float delta)
	{
		base.OnProcess(delta);
	}

}
