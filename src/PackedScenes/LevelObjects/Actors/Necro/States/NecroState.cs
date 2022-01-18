public abstract class NecroState : ActorState
{

	#region Fields

	protected readonly Necro m_necro;

	#endregion // Fields



	#region Constructors

	protected NecroState (Actor actor, Necro necro) : base(actor)
	{
		m_necro = necro;
	}

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
