using Godot;

public abstract class HoppingSlimeState : ActorState
{

	#region Fields

	protected readonly HoppingSlime m_hoppingSlime;

	#endregion // Fields



	#region Constructors

	protected HoppingSlimeState (Actor actor, HoppingSlime hoppingSlime) : base(actor) => m_hoppingSlime = hoppingSlime;

	#endregion // Constructors



	#region Public methods

	public override void OnEnter ()
	{
		base.OnEnter();
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
