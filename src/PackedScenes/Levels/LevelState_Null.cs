public class LevelState_Null : LevelState
{

	public LevelState_Null (Level level) : base(level) { }

	public override string StateName => "null";

	public override void OnEnter () { }
	public override void OnExit () { }
	public override void OnPhysicsProcess (float delta) { }
	public override void OnProcess (float delta) { }

}
