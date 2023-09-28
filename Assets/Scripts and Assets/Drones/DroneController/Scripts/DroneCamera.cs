using DroneController.CameraMovement;

public class DroneCamera : CameraScript {

	public override void Awake()
	{
		base.Awake(); //I would suggest you to put code below this line
	}

	override public void Start()
    {
        base.Start(); //I would suggest you to put code below this line
    }

	void FixedUpdate()
    {
        FPVTPSCamera();
        ScrollMath();
    }

}
