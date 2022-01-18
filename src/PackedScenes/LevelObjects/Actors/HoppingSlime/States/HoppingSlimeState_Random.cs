using Godot;

public class HoppingSlimeState_Random : HoppingSlimeState_Alive
{

	#region Fields

	private readonly RandomNumberGenerator m_rng;

	#endregion // Fields



	#region Properties

	public override string StateName => "default";

	#endregion // Properties



	#region Constructors

	public HoppingSlimeState_Random (Actor actor, HoppingSlime hoppingSlime) : base(actor, hoppingSlime)
	{
		m_rng = new RandomNumberGenerator();
		m_rng.Randomize();
	}

	#endregion // Constructors



	#region Public methods

	public override void OnEnter ()
	{
		base.OnEnter();

		int i = m_rng.RandiRange(0, 100);

		if (i >= 0 && i < 50)
		{
			m_hoppingSlime.SetState(m_hoppingSlime.WalkSlowState);
		}
		else if (i >= 50 && i < 85)
		{
			m_hoppingSlime.SetState(m_hoppingSlime.WalkFastState);
		}
		else if (i >= 85 && i <= 100)
		{
			m_hoppingSlime.SetState(m_hoppingSlime.HopMiniState);
		}
	}

	public override void OnExit ()
	{
		base.OnExit();
	}

	public override void OnPhysicsProcess (float delta)
	{
		base.OnPhysicsProcess(delta);
	}

	public override void OnProcess (float delta)
	{
		base.OnProcess(delta);
	}

	#endregion // Public methods

}
