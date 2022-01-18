using Godot;

public class SlimeState_Die : SlimeState
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

	public SlimeState_Die (Actor actor, Slime slime) : base(actor, slime) { }

	#endregion // Constructors



	#region Public methods

	public override void OnEnter ()
	{
		base.OnEnter();

		m_originalFlashRate = m_slime.FlashRate;

		m_slime.IsApplyDeceleration = false;
		m_slime.IsUpdateFacingDirectionX = false;
		m_slime.IsFlashing = true;

		m_slime.Snap = Vector2.Zero;

		m_slime.Velocity = new Vector2()
		{
			x = m_slime.DieKnockback.x * m_slime.FacingDirection.x,
			y = -Mathf.Sqrt(2 * m_slime.Gravity * m_slime.DieKnockback.y)
		};

		m_slime.FlipV();

		m_slime.CollisionLayer = 0;
		m_slime.CollisionMask = 0;
		m_slime.SetCollisionMaskBit(0, true);
		m_slime.node_bodyCollisionShape.Disabled = true;
		m_slime.node_bodyCollisionShape.Disabled = false;

		m_slime.node_hurtArea.CollisionLayer = 0;
		m_slime.node_hurtArea.CollisionMask = 0;
		m_slime.node_hurtAreaCollisionShape.Disabled = true;

		m_slime.node_hitArea.CollisionLayer = 0;
		m_slime.node_hitArea.CollisionMask = 0;
		m_slime.node_hitAreaCollisionShape.Disabled = true;
	}

	public override void OnExit ()
	{
		base.OnExit();
	}

	public override void OnPhysicsProcess (float delta)
	{
		base.OnPhysicsProcess(delta);

		if (m_slime.IsOnFloor())
		{
			if (!m_hasHitFloor)
			{
				m_slime.IsMove = false;
			}
			m_hasHitFloor = true;
		}

		if (m_hasHitFloor)
		{
			m_dieTimer += delta;
			if (m_dieTimer > m_dieTime / 2)
			{
				m_slime.FlashRate = m_originalFlashRate / 2f;
			}
			if (m_dieTimer >= m_dieTime)
			{
				m_slime.Die();
			}
		}
	}

	public override void OnProcess (float delta)
	{
		base.OnProcess(delta);
	}

	#endregion // Public methods

}
