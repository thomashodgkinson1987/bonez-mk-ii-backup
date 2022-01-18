public abstract class ActorState
{

	#region Fields

	protected readonly Actor m_actor;

	#endregion // Fields



	#region Properties

	public abstract string StateName { get; }

	#endregion // Properties



	#region Constructors

	protected ActorState (Actor actor) => m_actor = actor;

	#endregion // Constructors



	#region Public methods

	public virtual void OnEnter ()
	{
		m_actor.node_animatedSprite.Play(StateName);
	}

	public virtual void OnExit ()
	{
		m_actor.node_animatedSprite.Stop();
	}

	public virtual void OnPhysicsProcess (float delta) { }

	public virtual void OnProcess (float delta) { }

	#endregion // Public methods

}
