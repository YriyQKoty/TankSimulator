using UnityEngine;
using System.Collections;

public class PlayerTracksController : TankTracksController {
	
	public bool enableUserInput = true;
	
	public float steerG = 0.0f;
	public float accelG = 0.0f;
			
	
	void Update(){
		if(enableUserInput){
			accelG = Input.GetAxis("Vertical");
			steerG = Input.GetAxis("Horizontal");
		}		
		
	}
	
	void FixedUpdate(){
		
		//float accelerate = 0;
		//float steer = 0;
		
				
		UpdateWheels(accelG,steerG);
		
	}
	
	
}
