public abstract class ShtompState : ActorState
{

	#region Fields

	protected Shtomp m_shtomp;

	#endregion // Fields



	#region Constructors

	protected ShtompState (Actor actor, Shtomp shtomp) : base(actor) => m_shtomp = shtomp;

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
