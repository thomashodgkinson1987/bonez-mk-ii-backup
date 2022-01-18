using Godot;

public class HoppingSlimeState_Hop_Big : HoppingSlimeState_Hop
{

	#region Fields

	private readonly RandomNumberGenerator m_rng;

	#endregion // Fields



	#region Properties

	public override string StateName => "hop_big";

	#endregion // Properties



	#region Constructors

	public HoppingSlimeState_Hop_Big (Actor actor, HoppingSlime hoppingSlime) : base(actor, hoppingSlime)
	{
		m_rng = new RandomNumberGenerator();
		m_rng.Randomize();
	}

	#endregion // Constructors



	#region Public methods

	public override void OnEnter ()
	{
		HopHeight = m_hoppingSlime.BigHopHeight;
		HopDistance = m_hoppingSlime.BigHopDistance;

		if (m_hoppingSlime.Target != null && m_hoppingSlime.CanSeeTarget)
		{
			if (m_hoppingSlime.Position.x < m_hoppingSlime.Target.Position.x)
			{
				HopDirection = 1;
			}
			else
			{
				HopDirection = -1;
			}
		}
		else
		{
			HopDirection = m_rng.RandiRange(0, 1) == 0 ? -1 : 1;
		}

		base.OnEnter();

		m_hoppingSlime.SetState(m_hoppingSlime.HoppingState);
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
