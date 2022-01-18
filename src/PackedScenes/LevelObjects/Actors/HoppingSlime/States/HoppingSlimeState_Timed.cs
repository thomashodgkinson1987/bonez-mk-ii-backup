using Godot;

public abstract class HoppingSlimeState_Timed : HoppingSlimeState_Alive
{

	#region Fields

	private readonly RandomNumberGenerator m_rng;

	#endregion // Fields



	#region Properties

	public float TimeLimit { get; set; } = 3f;
	public float TimeLimitMin { get; set; } = 1f;
	public float TimeLimitMax { get; set; } = 3f;
	public float Timer { get; set; } = 0f;

	#endregion // Properties



	#region Constructors

	protected HoppingSlimeState_Timed (Actor actor, HoppingSlime hoppingSlime) : base(actor, hoppingSlime)
	{
		m_rng = new RandomNumberGenerator();
		m_rng.Randomize();
	}

	#endregion // Constructors



	#region Public methods

	public override void OnEnter ()
	{
		base.OnEnter();

		Timer = 0f;
		TimeLimit = m_rng.RandfRange(TimeLimitMin, TimeLimitMax);
	}

	public override void OnExit ()
	{
		base.OnExit();

		Timer = 0f;
	}

	public override void OnPhysicsProcess (float delta)
	{
		base.OnPhysicsProcess(delta);

		Timer += delta;

		if (Timer >= TimeLimit) m_hoppingSlime.SetState(m_hoppingSlime.RandomState);
	}

	public override void OnProcess (float delta)
	{
		base.OnProcess(delta);
	}

	#endregion // Public methods

}
