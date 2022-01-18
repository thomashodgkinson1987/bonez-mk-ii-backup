using Godot;

public class HoppingSlimeState_Hopping : HoppingSlimeState_Alive
{

	#region Fields

	private readonly RandomNumberGenerator m_rng;

	#endregion // Fields



	#region Properties

	public override string StateName => "hopping";

	#endregion // Properties



	#region Constructors

	public HoppingSlimeState_Hopping (Actor actor, HoppingSlime hoppingSlime) : base(actor, hoppingSlime)
	{
		m_rng = new RandomNumberGenerator();
		m_rng.Randomize();
	}

	#endregion // Constructors



	#region Public methods

	public override void OnEnter ()
	{
		base.OnEnter();

		m_hoppingSlime.Snap = Vector2.Zero;
	}

	public override void OnExit ()
	{
		base.OnExit();
	}

	public override void OnPhysicsProcess (float delta)
	{
		base.OnPhysicsProcess(delta);

		if (m_hoppingSlime.IsOnFloor()) m_hoppingSlime.SetState(m_hoppingSlime.RandomState);
	}

	public override void OnProcess (float delta)
	{
		base.OnProcess(delta);
	}

	#endregion // Public methods

}
