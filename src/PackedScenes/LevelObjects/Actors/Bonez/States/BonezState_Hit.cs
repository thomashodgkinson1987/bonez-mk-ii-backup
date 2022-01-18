using Godot;

public class BonezState_Hit : BonezState
{

	#region Properties

	public override string StateName => "hit";

	public float HitTime { get; set; } = 1f;
	public float HitTimer { get; set; } = 0f;

	#endregion // Properties



	#region Constructors

	public BonezState_Hit (Actor actor, Bonez bonez) : base(actor, bonez) { }

	#endregion // Constructors



	#region State methods

	public override void OnEnter ()
	{
		base.OnEnter();

		HitTimer = 0f;

		m_bonez.node_audioPlayer_hit.Play();
		m_bonez.IsInvincible = true;
		m_bonez.IsApplyDecelerationX = false;
		m_bonez.IsPollInput = false;
		m_bonez.Snap = Vector2.Zero;

		m_bonez.Velocity = new Vector2()
		{
			x = m_bonez.HitKnockback.x * -m_bonez.FacingDirection.x,
			y = -Mathf.Sqrt(2 * m_bonez.Gravity * m_bonez.HitKnockback.y)
		};

		m_bonez.Move();
	}

	public override void OnExit ()
	{
		base.OnExit();

		HitTimer = 0f;
		m_bonez.IsApplyDecelerationX = true;
		m_bonez.IsPollInput = true;
	}

	public override void OnPhysicsProcess (float delta)
	{
		base.OnPhysicsProcess(delta);

		m_bonez.ApplyGravity(delta / 2);
		m_bonez.Move();
		m_bonez.ApplyGravity(delta / 2);

		if (m_bonez.IsOnFloor())
			m_bonez.IsApplyDecelerationX = true;

		HitTimer += delta;
		if (HitTimer >= HitTime)
			m_bonez.SetState(m_bonez.IdleState);
	}

	public override void OnProcess (float delta)
	{
		base.OnProcess(delta);
	}

	#endregion // State methods

}
