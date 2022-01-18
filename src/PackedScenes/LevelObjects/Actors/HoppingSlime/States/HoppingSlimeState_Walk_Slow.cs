using Godot;

public class HoppingSlimeState_Walk_Slow : HoppingSlimeState_Timed
{

	#region Fields

	private readonly RandomNumberGenerator m_rng;

	#endregion // Fields



	#region Properties

	public override string StateName => "walk_slow";

	public float WalkSpeed { get; set; } = 16f;

	public float Direction { get; set; } = 1f;

	#endregion // Properties



	#region Constructors

	public HoppingSlimeState_Walk_Slow (Actor actor, HoppingSlime hoppingSlime) : base(actor, hoppingSlime)
	{
		m_rng = new RandomNumberGenerator();
		m_rng.Randomize();
	}

	#endregion // Constructors



	#region Public methods

	public override void OnEnter ()
	{
		base.OnEnter();

		WalkSpeed = m_hoppingSlime.SlowWalkSpeed;

		m_hoppingSlime.Snap = new Vector2(0f, 8f);

		if (m_hoppingSlime.CanSeeTarget)
		{
			if (m_hoppingSlime.Position.x < m_hoppingSlime.Target.Position.x)
			{
				Direction = 1;
			}
			else
			{
				Direction = -1;
			}
		}

		m_hoppingSlime.Velocity = new Vector2()
		{
			x = Direction * WalkSpeed,
			y = m_hoppingSlime.Velocity.y
		};
	}

	public override void OnExit ()
	{
		base.OnExit();
	}

	public override void OnPhysicsProcess (float delta)
	{
		base.OnPhysicsProcess(delta);

		if (!m_hoppingSlime.IsOnFloor()) m_hoppingSlime.SetState(m_hoppingSlime.FallState);
	}

	public override void OnProcess (float delta)
	{
		base.OnProcess(delta);
	}

	#endregion // Public methods

}
