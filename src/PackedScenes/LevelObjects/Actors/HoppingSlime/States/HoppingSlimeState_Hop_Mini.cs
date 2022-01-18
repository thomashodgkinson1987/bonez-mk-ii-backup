using Godot;

public class HoppingSlimeState_Hop_Mini : HoppingSlimeState_Hop
{

	#region Fields

	private readonly RandomNumberGenerator m_rng;

	#endregion // Fields



	#region Properties

	public override string StateName => "hop_mini";

	#endregion // Properties



	#region Constructors

	public HoppingSlimeState_Hop_Mini (Actor actor, HoppingSlime hoppingSlime) : base(actor, hoppingSlime)
	{
		m_rng = new RandomNumberGenerator();
		m_rng.Randomize();
	}

	#endregion // Constructors



	#region Public methods

	public override void OnEnter ()
	{
		HopHeight = m_hoppingSlime.MiniHopHeight;
		HopDistance = m_hoppingSlime.MiniHopDistance;
		HopDirection = 0;

		base.OnEnter();
	}

	public override void OnExit ()
	{
		base.OnExit();
	}

	public override void OnPhysicsProcess (float delta)
	{
		base.OnPhysicsProcess(delta);

		if (m_hoppingSlime.IsOnFloor())
		{
			int choice = m_rng.RandiRange(0, 1);

			switch (choice)
			{
				case 0:
				m_hoppingSlime.SetState(m_hoppingSlime.HopSmallState);
				break;

				case 1:
				m_hoppingSlime.SetState(m_hoppingSlime.HopBigState);
				break;

				default:
				break;
			}
		}
	}

	public override void OnProcess (float delta)
	{
		base.OnProcess(delta);
	}

	#endregion // Public methods

}
