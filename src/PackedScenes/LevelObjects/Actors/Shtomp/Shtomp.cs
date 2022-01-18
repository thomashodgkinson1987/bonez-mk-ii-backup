using Godot;

public class Shtomp : Actor
{

	#region Nodes

	public Node2D node_rayCasters { get; private set; }

	#endregion // Nodes



	#region Properties

	public Vector2 ReturnGlobalPosition { get; set; } = Vector2.Zero;

	#endregion // Properties



	#region Exports

	[Export]
	public float WaitingToScanTime
	{
		get => WaitingToScanState.TimeToWait;
		set => WaitingToScanState.TimeToWait = value;
	}
	[Export]
	public float WaitingToDropTime
	{
		get => WaitingToDropState.TimeToWait;
		set => WaitingToDropState.TimeToWait = value;
	}
	[Export]
	public float WaitingToReturnTime
	{
		get => WaitingToReturnState.TimeToWait;
		set => WaitingToReturnState.TimeToWait = value;
	}

	[Export]
	public float ReturnSpeed
	{
		get => ReturningState.Speed;
		set => ReturningState.Speed = value;
	}

	#endregion // Exports



	#region States

	public ShtompState_Init InitState { get; private set; }

	public ShtompState_WaitingToScan WaitingToScanState { get; private set; }
	public ShtompState_Scanning ScanningState { get; private set; }
	public ShtompState_TargetFound TargetFoundState { get; private set; }
	public ShtompState_WaitingToDrop WaitingToDropState { get; private set; }
	public ShtompState_TargetLost TargetLostState { get; private set; }
	public ShtompState_Dropping DroppingState { get; private set; }
	public ShtompState_Landed LandedState { get; private set; }
	public ShtompState_WaitingToReturn WaitingToReturnState { get; private set; }
	public ShtompState_Returning ReturningState { get; private set; }
	public ShtompState_Returned ReturnedState { get; private set; }

	#endregion // States



	#region Constructors

	public Shtomp () : base()
	{
		InitState = new ShtompState_Init(this, this);

		WaitingToScanState = new ShtompState_WaitingToScan(this, this);
		ScanningState = new ShtompState_Scanning(this, this);
		TargetFoundState = new ShtompState_TargetFound(this, this);
		WaitingToDropState = new ShtompState_WaitingToDrop(this, this);
		TargetLostState = new ShtompState_TargetLost(this, this);
		DroppingState = new ShtompState_Dropping(this, this);
		LandedState = new ShtompState_Landed(this, this);
		WaitingToReturnState = new ShtompState_WaitingToReturn(this, this);
		ReturningState = new ShtompState_Returning(this, this);
		ReturnedState = new ShtompState_Returned(this, this);
	}

	#endregion // Constructors



	#region Godot methods

	public override void _Ready ()
	{
		base._Ready();

		node_rayCasters = GetNode<Node2D>("RayCasters");

		m_state = InitState;
		m_state.OnEnter();
	}

	#endregion // Godot methods



	#region Public methods

	public bool IsAnythingBelow ()
	{
		for (int i = 0; i < node_rayCasters.GetChildCount(); i++)
		{
			RayCast2D rayCast2D = (RayCast2D)node_rayCasters.GetChild(i);
			if (rayCast2D.IsColliding()) return true;
		}

		return false;
	}

	public bool AreAnyActorsInRange ()
	{
		for (int i = 0; i < node_rayCasters.GetChildCount(); i++)
		{
			RayCast2D rayCast2D = (RayCast2D)node_rayCasters.GetChild(i);
			if (rayCast2D.IsColliding())
			{
				if (rayCast2D.GetCollider() is Area2D area)
				{
					if (area.Name == "HurtArea") return true;
				}
			}
		}

		return false;
	}

	#endregion // Public methods

}
