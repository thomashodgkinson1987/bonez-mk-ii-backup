using Godot;

public abstract class HoppingSlimeState_Alive : HoppingSlimeState
{

	#region Constructors

	public HoppingSlimeState_Alive (Actor actor, HoppingSlime hoppingSlime) : base(actor, hoppingSlime) { }

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

		if (m_hoppingSlime.Health == 0)
		{
			m_hoppingSlime.SetState(m_hoppingSlime.DieState);
			return;
		}

		m_hoppingSlime.ApplyGravity(delta / 2);

		Vector2 oldVelocity = m_hoppingSlime.Velocity;

		m_hoppingSlime.Move();

		if (oldVelocity.x != 0 && m_hoppingSlime.Velocity.x == 0)
		{
			m_hoppingSlime.node_rayCast2D_leftWall.ForceRaycastUpdate();
			m_hoppingSlime.node_rayCast2D_rightWall.ForceRaycastUpdate();

			if (m_hoppingSlime.node_rayCast2D_leftWall.IsColliding() || m_hoppingSlime.node_rayCast2D_rightWall.IsColliding())
			{
				m_hoppingSlime.Velocity = new Vector2(oldVelocity.x * -1, m_hoppingSlime.Velocity.y);
			}
		}

		m_hoppingSlime.ApplyGravity(delta / 2);
	}

	public override void OnProcess (float delta)
	{
		base.OnProcess(delta);

		m_hoppingSlime.UpdateFacingDirectionX();
	}

	#endregion // Public methods

}
