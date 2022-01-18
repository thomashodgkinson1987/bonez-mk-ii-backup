public abstract class SlimeState : ActorState
{

	#region Fields

	protected readonly Slime m_slime;

	#endregion // Fields



	#region Constructors

	protected SlimeState (Actor actor, Slime slime) : base(actor) => m_slime = slime;

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

		m_slime.ApplyGravity(delta / 2);
		m_slime.Move();
		m_slime.ApplyGravity(delta / 2);
	}

	public override void OnProcess (float delta)
	{
		base.OnProcess(delta);

		m_slime.UpdateFacingDirectionX();
	}

	#endregion // Public methods

}
