public abstract class LevelState
{

	#region Fields

	protected readonly Level m_level;

	#endregion // Fields



	#region Properties

	public abstract string StateName { get; }

	#endregion // Properties



	#region Constructors

	protected LevelState (Level level) => m_level = level;

	#endregion // Constructors



	#region Public methods

	public abstract void OnEnter ();

	public abstract void OnExit ();

	public abstract void OnPhysicsProcess (float delta);

	public abstract void OnProcess (float delta);

	#endregion // Public methods

}
