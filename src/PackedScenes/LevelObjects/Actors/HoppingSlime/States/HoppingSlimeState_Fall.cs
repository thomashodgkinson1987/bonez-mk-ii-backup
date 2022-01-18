using Godot;

public class HoppingSlimeState_Fall : HoppingSlimeState_Alive
{

	#region Properties

	public override string StateName => "fall";

	#endregion // Properties



	#region Constructors

	public HoppingSlimeState_Fall (Actor actor, HoppingSlime hoppingSlime) : base(actor, hoppingSlime) { }

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
