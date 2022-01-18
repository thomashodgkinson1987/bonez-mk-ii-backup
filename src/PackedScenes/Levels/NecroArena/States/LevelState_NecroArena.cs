public abstract class LevelState_NecroArena : LevelState
{

	#region Fields

	protected readonly Level_NecroArena m_necroArenaLevel;

	#endregion // Fields



	#region Constructors

	protected LevelState_NecroArena (Level level, Level_NecroArena necroArenaLevel)
		: base(level) => m_necroArenaLevel = necroArenaLevel;

	#endregion // Constructors

}
