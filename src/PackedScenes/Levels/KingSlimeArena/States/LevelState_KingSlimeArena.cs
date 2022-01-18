public abstract class LevelState_KingSlimeArena : LevelState
{

	#region Fields

	protected readonly Level_KingSlimeArena m_kingSlimeArenaLevel;

	#endregion // Fields



	#region Constructors

	protected LevelState_KingSlimeArena (Level level, Level_KingSlimeArena kingSlimeArenaLevel)
		: base(level) => m_kingSlimeArenaLevel = kingSlimeArenaLevel;

	#endregion // Constructors

}
