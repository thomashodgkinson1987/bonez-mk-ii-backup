public abstract class KurtState : ActorState
{

	#region Fields

	protected readonly Kurt m_kurt;

	#endregion // Fields



	#region Constructors

	protected KurtState (Actor actor, Kurt kurt) : base(actor)
	{
		m_kurt = kurt;
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
