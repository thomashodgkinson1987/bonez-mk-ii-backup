using Godot;

public abstract class HoppingSlimeState_Hop : HoppingSlimeState_Alive
{

	#region Properties

	public float HopHeight { get; set; } = 8f;
	public float HopDistance { get; set; } = 32f;
	public int HopDirection { get; set; } = 1;

	#endregion // Properties



	#region Constructors

	public HoppingSlimeState_Hop (Actor actor, HoppingSlime hoppingSlime) : base(actor, hoppingSlime) { }

	#endregion // Constructors



	#region Public methods

	public override void OnEnter ()
	{
		base.OnEnter();

		m_hoppingSlime.Snap = Vector2.Zero;

		m_hoppingSlime.Velocity = new Vector2()
		{
			x = HopDistance * HopDirection,
			y = -Mathf.Sqrt(2 * m_hoppingSlime.Gravity * HopHeight)
		};
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
