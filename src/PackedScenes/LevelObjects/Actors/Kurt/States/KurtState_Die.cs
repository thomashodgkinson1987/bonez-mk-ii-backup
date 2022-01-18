using Godot;

public class KurtState_Die : KurtState
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

	public KurtState_Die (Actor actor, Kurt kurt) : base(actor, kurt) { }

	#endregion // Constructors



	#region Public methods

	public override void OnEnter ()
	{
		base.OnEnter();

		m_originalFlashRate = m_kurt.FlashRate;

		m_kurt.IsApplyDeceleration = false;
		m_kurt.IsUpdateFacingDirectionX = false;
		m_kurt.IsFlashing = true;
		m_kurt.Snap = Vector2.Zero;
		m_kurt.IsApplyGravity = true;

		m_kurt.Velocity = new Vector2()
		{
			x = m_kurt.DieKnockback.x * (m_kurt.FacingDirection.x),
			y = -Mathf.Sqrt(2 * m_kurt.Gravity * m_kurt.DieKnockback.y)
		};

		m_kurt.FlipV();

		m_kurt.CollisionLayer = 0;
		m_kurt.CollisionMask = 0;
		m_kurt.SetCollisionMaskBit(0, true);
		m_kurt.node_bodyCollisionShape.Disabled = true;
		m_kurt.node_bodyCollisionShape.Disabled = false;

		m_kurt.node_backBody.CollisionLayer = 0;
		m_kurt.node_backBody.CollisionMask = 0;
		m_kurt.node_backBodyCollisionShape2D.Disabled = true;

		m_kurt.node_hurtArea.CollisionLayer = 0;
		m_kurt.node_hurtArea.CollisionMask = 0;
		m_kurt.node_hurtAreaCollisionShape.Disabled = true;

		m_kurt.node_hitArea.CollisionLayer = 0;
		m_kurt.node_hitArea.CollisionMask = 0;
		m_kurt.node_hitAreaCollisionShape.Disabled = true;
	}

	public override void OnExit ()
	{
		base.OnExit();
	}

	public override void OnPhysicsProcess (float delta)
	{
		base.OnPhysicsProcess(delta);

		m_kurt.ApplyGravity(delta / 2);
		m_kurt.Move();
		m_kurt.ApplyGravity(delta / 2);

		if (m_kurt.IsOnFloor())
		{
			if (!m_hasHitFloor)
			{
				m_kurt.IsMove = false;
			}
			m_hasHitFloor = true;
		}

		if (m_hasHitFloor)
		{
			m_dieTimer += delta;
			if (m_dieTimer > m_dieTime / 2)
			{
				m_kurt.FlashRate = m_originalFlashRate / 2f;
			}
			if (m_dieTimer >= m_dieTime)
			{
				m_kurt.Die();
			}
		}
	}

	public override void OnProcess (float delta)
	{
		base.OnProcess(delta);
	}

	#endregion // Public methods

}
