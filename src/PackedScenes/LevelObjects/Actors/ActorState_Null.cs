public class ActorState_Null : ActorState
{

	#region Properties

	public override string StateName => "null";

	#endregion // Properties



	#region Constructors

	public ActorState_Null (Actor actor) : base(actor) { }

	#endregion // Constructors

}
