using Godot;

public class BatState_Die : BatState
{

	#region Fields

	private float m_dieTime = 1f;
	private float m_dieTimer = 0f;
	private bool m_hasHitFloor = false;
	private float m_originalFlashRate = 0f;

	#endregion // Fields



	#region Properties

	public override string StateName => "die";

	#endregion // Properties



	#region Constructors

	public BatState_Die (Actor actor, Bat bat) : base(actor, bat) { }

	#endregion // Constructors



	#region Public methods

	public override void OnEnter ()
	{
		base.OnEnter();

		m_originalFlashRate = m_bat.FlashRate;

		m_bat.IsApplyGravity = true;
		m_bat.IsFlashing = true;
		m_bat.Snap = Vector2.Zero;
		m_bat.FacingDirection = new Vector2(m_bat.LastTargetPosition.x > m_bat.GlobalPosition.x ? -1f : 1f, 0f);
		m_bat.FlipV();

		m_bat.Velocity = new Vector2()
		{
			x = m_bat.DieKnockback.x * (m_bat.FacingDirection.x),
			y = -Mathf.Sqrt(2 * m_bat.Gravity * m_bat.DieKnockback.y)
		};

		m_bat.CollisionLayer = 0;
		m_bat.CollisionMask = 0;
		m_bat.SetCollisionMaskBit(0, true);
		m_bat.node_bodyCollisionShape.Disabled = true;
		m_bat.node_bodyCollisionShape.Disabled = false;

		m_bat.node_hurtArea.CollisionLayer = 0;
		m_bat.node_hurtArea.CollisionMask = 0;
		m_bat.node_hurtAreaCollisionShape.Disabled = true;

		m_bat.node_hitArea.CollisionLayer = 0;
		m_bat.node_hitArea.CollisionMask = 0;
		m_bat.node_hitAreaCollisionShape.Disabled = true;
	}

	public override void OnExit ()
	{
		base.OnExit();
	}

	public override void OnPhysicsProcess (float delta)
	{
		base.OnPhysicsProcess(delta);

		m_bat.ApplyGravity(delta / 2);
		m_bat.Move();
		m_bat.ApplyGravity(delta / 2);

		if (m_bat.IsOnFloor())
		{
			if (!m_hasHitFloor)
			{
				m_bat.IsMove = false;
			}
			m_hasHitFloor = true;
		}

		if (m_hasHitFloor)
		{
			m_dieTimer += delta;
			if (m_dieTimer > m_dieTime / 2)
			{
				m_bat.FlashRate = m_originalFlashRate / 2f;
			}
			if (m_dieTimer >= m_dieTime)
			{
				m_bat.Die();
			}
		}
	}

	public override void OnProcess (float delta)
	{
		base.OnProcess(delta);
	}

	#endregion // Public methods

}
