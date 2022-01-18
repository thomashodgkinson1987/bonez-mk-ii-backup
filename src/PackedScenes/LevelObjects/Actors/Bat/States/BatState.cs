using Godot;

public abstract class BatState : ActorState
{

	protected readonly Bat m_bat;

	protected BatState (Actor actor, Bat bat) : base(actor) => m_bat = bat;

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

		if (m_bat.Health == 0) m_bat.SetState(m_bat.DieState);
	}

	public override void OnProcess (float delta)
	{
		base.OnProcess(delta);
	}

}
