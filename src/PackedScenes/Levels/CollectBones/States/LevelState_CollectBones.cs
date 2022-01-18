public abstract class LevelState_CollectBones : LevelState
{

	#region Fields

	protected readonly Level_CollectBones m_collectBonesLevel;

	#endregion // Fields



	#region Constructors

	protected LevelState_CollectBones (Level level, Level_CollectBones collectBonesLevel)
		: base(level) => m_collectBonesLevel = collectBonesLevel;

	#endregion // Constructors

}
