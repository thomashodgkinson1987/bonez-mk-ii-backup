using Godot;

public class HoppingSlimeState_Die : HoppingSlimeState
{

	#region Fields

	private float m_dieTime = 1f;
	private float m_dieTimer = 0f;
	private float m_originalFlashRate = 0f;

	#endregion // Fields



	#region Properties

	public override string StateName => "die";

	#endregion // Properties



	#region Constructors

	public HoppingSlimeState_Die (Actor actor, HoppingSlime hoppingSlime) : base(actor, hoppingSlime) { }

	#endregion // Constructors



	#region Public methods

	public override void OnEnter ()
	{
		base.OnEnter();

		m_originalFlashRate = m_hoppingSlime.FlashRate;

		m_hoppingSlime.IsUpdateFacingDirectionX = false;
		m_hoppingSlime.IsFlashing = true;
		m_hoppingSlime.Snap = Vector2.Zero;

		m_hoppingSlime.Velocity = new Vector2()
		{
			x = m_hoppingSlime.DieKnockback.x * m_hoppingSlime.FacingDirection.x,
			y = -Mathf.Sqrt(2 * m_hoppingSlime.Gravity * m_hoppingSlime.DieKnockback.y)
		};

		m_hoppingSlime.FlipV();

		m_hoppingSlime.node_hurtAreaCollisionShape.Disabled = true;
		m_hoppingSlime.node_hitAreaCollisionShape.Disabled = true;
	}

	public override void OnExit ()
	{
		base.OnExit();
	}

	public override void OnPhysicsProcess (float delta)
	{
		base.OnPhysicsProcess(delta);

		m_hoppingSlime.ApplyGravity(delta / 2);
		m_hoppingSlime.Move();
		m_hoppingSlime.ApplyGravity(delta / 2);

		if (m_hoppingSlime.IsOnFloor())
		{
			m_hoppingSlime.Velocity = Vector2.Zero;
		}

		m_dieTimer += delta;
		if (m_dieTimer > m_dieTime / 2)
		{
			m_hoppingSlime.FlashRate = m_originalFlashRate / 2f;
		}
		if (m_dieTimer >= m_dieTime)
		{
			m_hoppingSlime.Die();
		}
	}

	public override void OnProcess (float delta)
	{
		base.OnProcess(delta);
	}

	#endregion // Public methods

}
