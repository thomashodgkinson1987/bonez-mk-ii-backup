using Godot;

public abstract class BonezState : ActorState
{

	#region Fields

	protected readonly Bonez m_bonez;

	#endregion // Fields



	#region Constructors

	protected BonezState (Actor actor, Bonez bonez) : base(actor) => m_bonez = bonez;

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
