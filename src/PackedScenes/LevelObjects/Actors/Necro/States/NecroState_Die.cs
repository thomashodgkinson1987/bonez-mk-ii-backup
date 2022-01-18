using Godot;
using Godot.Collections;

public class NecroState_Die : NecroState
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

	public NecroState_Die (Actor actor, Necro necro) : base(actor, necro) { }

	#endregion // Constructors



	#region Public methods

	public override void OnEnter ()
	{
		base.OnEnter();

		Array projectiles = m_necro.GetTree().GetNodesInGroup("projectiles");
		for (int i = 0; i < projectiles.Count; i++)
		{
			((Projectile)projectiles[i]).QueueFree();
		}

		m_originalFlashRate = m_necro.FlashRate;

		m_necro.IsApplyGravity = true;
		m_necro.IsFlashing = true;
		m_necro.Snap = Vector2.Zero;
		m_necro.FlipV();

		m_necro.Velocity = new Vector2()
		{
			x = m_necro.DieKnockback.x * (m_necro.FacingDirection.x),
			y = -Mathf.Sqrt(2 * m_necro.Gravity * m_necro.DieKnockback.y)
		};

		m_necro.CollisionLayer = 0;
		m_necro.CollisionMask = 0;
		m_necro.SetCollisionMaskBit(0, true);
		m_necro.node_bodyCollisionShape.Disabled = true;
		m_necro.node_bodyCollisionShape.Disabled = false;

		m_necro.node_hurtArea.CollisionLayer = 0;
		m_necro.node_hurtArea.CollisionMask = 0;
		m_necro.node_hurtAreaCollisionShape.Disabled = true;

		m_necro.node_hitArea.CollisionLayer = 0;
		m_necro.node_hitArea.CollisionMask = 0;
		m_necro.node_hitAreaCollisionShape.Disabled = true;
	}

	public override void OnExit ()
	{
		base.OnExit();
	}

	public override void OnPhysicsProcess (float delta)
	{
		base.OnPhysicsProcess(delta);

		m_necro.ApplyGravity(delta / 2);
		m_necro.Move();
		m_necro.ApplyGravity(delta / 2);

		if (m_necro.IsOnFloor())
		{
			if (!m_hasHitFloor)
			{
				m_necro.IsMove = false;
			}
			m_hasHitFloor = true;
		}

		if (m_hasHitFloor)
		{
			m_dieTimer += delta;
			if (m_dieTimer > m_dieTime / 2)
			{
				m_necro.FlashRate = m_originalFlashRate / 2f;
			}
			if (m_dieTimer >= m_dieTime)
			{
				m_necro.Die();
			}
		}
	}

	public override void OnProcess (float delta)
	{
		base.OnProcess(delta);
	}

	#endregion // Public methods

}
