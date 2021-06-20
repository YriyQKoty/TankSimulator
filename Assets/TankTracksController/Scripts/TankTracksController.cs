using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent (typeof (Rigidbody))]

public class TankTracksController : MonoBehaviour {
	
	public GameObject wheelCollider;
	private WheelCollider colliderFromPrefab;
	
	public bool autoCOM = true;
	public Transform COM;
	
	
	public float wheelsOffset = 0.4f;
	public float bonesOffset = 0.4f;
	public float noGroundedColOffset = 0.05f;
	
	public WheelsAxisSettings wheelsAndBonesAxisSettings;
	
	public float trackTextureSpeed = 1.1111f;
		
	public TrackTextureDirectionSettings trackTextireAnimationSettings;
		
	public GameObject leftTrack;
	public Transform[] leftTrackUpperWheels;
	public Transform[] leftTrackWheels;
	public Transform[] leftTrackBones;
	
	public GameObject rightTrack;
	public Transform[] rightTrackUpperWheels;
	public Transform[] rightTrackWheels;
	public Transform[] rightTrackBones;
		
	public VAconfig accelerationConfiguration;
	
	
	[System.Serializable]		
	public class VAconfig{
		
		
		//Dynamics that affect the acceleration and max speed
	    //        Motor Torque
		//        |
		//        |
		//        |
		//        |_________________speed km/ph	
		public AnimationCurve acceleration = AnimationCurve.Linear(0.0f,750.0f,80.0f,0.0f);	
		
		//Dynamics that affect the brake force when vertical axis is not active
	    //        Brake Torque
		//        |
		//        |
		//        |
		//        |_________________speed km/ph	
		public AnimationCurve brake = AnimationCurve.Linear(0.0f,1000.0f,80.0f,1100.0f);
		
	}
	
	public HAconfig rotationOnStayConfiguration;
	
	public HAconfig rotationOnAccelerationConfiguration;
		
	
	[System.Serializable]		
	public class HAconfig{
		//Dynamics that affect the rotate speed
	    //        Rotate Vector Y coord
		//        |
		//        |
		//        |
		//        |_________________speed km/ph	
		public AnimationCurve rotateSpeed = AnimationCurve.Linear(0.0f,6.5f,80.0f,5.5f);
		//Dynamics that affect the brake when rotate
	    //        Brake Torque
		//        |
		//        |
		//        |
		//        |_________________speed km/ph	
		public AnimationCurve brake = AnimationCurve.Linear(0.0f,0.0f,80.0f,1000.0f);
	}
	
		//Dynamics that affect the rotate damper
	    //        Rotate Vector Z coord
		//        |
		//        |
		//        |
		//        |_________________speed km/ph	
	public AnimationCurve rotationDamper = AnimationCurve.Linear(0.0f,0.0f,80.0f,20.0f);
	
		
	
	public float sidewaysFrictionExtremumFactor = 0.1f;
	public float sidewaysFrictionAsymptoteFactor = 0.08f;
	
	public bool showDebugInfo = false;
	public GUIStyle debugInfoStyle;
	
		
	public enum Axis{
		X,
		Y,
		Z,		
	};
	
	public enum TexAxis{
		X,
		Y,		
	};
	
	
	[System.Serializable]
	public class WheelsAxisSettings{
		public Axis wheelsPositionAxis = Axis.Y;
		public bool inverseWheelsPosition = false;
	
		public Axis bonesPositionAxis = Axis.Y;
		public bool inverseBonesPosition = false;
	
		public Axis wheelsRotationAxis = Axis.X;
		public bool inverseWheelsRotation = false;
		
		
		
		
		
		private int WheelRotationAxisPointer = 0;
		
		public int WRAxisPointer{
			get{return WheelRotationAxisPointer;}	
			set{WheelRotationAxisPointer=value;}				
		}
		
		private int wheelsPositionAxisPointer = 1;
		
		public int WPAxisPointer{
			get{return wheelsPositionAxisPointer;}	
			set{wheelsPositionAxisPointer=value;}				
		}
		
		private int bonesPositionAxisPointer = 1;
		
		public int BPAxisPointer{
			get{return bonesPositionAxisPointer;}	
			set{bonesPositionAxisPointer=value;}				
		}
		
		
		public static int SwitchAxis(Axis axis){
			int pointer = 0;
			switch(axis){
				case Axis.X: 
					pointer = 0;	
				break;
				
				case Axis.Y: 
					pointer = 1;
				break;
	
				
				case Axis.Z: 
					pointer = 2;
				break;					
			}
			
			return pointer;
			
		}
	}
	
	
	[System.Serializable]
	public class TrackTextureDirectionSettings{
		public TexAxis trackTextureDirection = TexAxis.Y;
		public bool inverseTextureDirection = false;	
		
		private int trackTextureAxisPointer = 2;
		
		public int TTAxisPointer{
			get{return trackTextureAxisPointer;}	
			set{trackTextureAxisPointer=value;}				
		}
		
		public static int SwitchAxis(TexAxis axis){
			int pointer = 0;
			switch(axis){
				case TexAxis.X: 
					pointer = 0;	
				break;
				
				case TexAxis.Y: 
					pointer = 1;
				break;	
							
			}
			
			return pointer;
			
		}
		
	}
	
	
	public class WheelData {
		public Transform wheelTransform;
		public Vector3 wheelStartPos;
		//public float rotation = 0.0f;
		public Vector3 wheelRotationAngles;		
	}
	
	public class WheelDataExt: WheelData{
		public Transform boneTransform;
		public WheelCollider col;
		public Vector3 boneStartPos;
	}
	
	
	protected WheelDataExt[] leftTrackWheelData;
	protected WheelDataExt[] rightTrackWheelData;
	
	
	protected WheelData[] leftTrackUpperWD;
	protected WheelData[] rightTrackUpperWD;
	
	
	
	protected Vector2 leftTrackTextureOffset = Vector2.zero;
	protected Vector2 rightTrackTextureOffset = Vector2.zero;
	
		
	public float CurrentSpeed {
		get { return GetComponent<Rigidbody>().velocity.magnitude * 3.6f /*Mathf.PI*/; }
	}
		
	
	
	private float leftTrackMiddleRPM = 0.0f;
	private float rightTrackMiddleRPM = 0.0f;
	
	private int wheelsCount = 0;
	
	void Awake() {
		
		colliderFromPrefab = wheelCollider.GetComponent<WheelCollider>();
		
		wheelsCount = leftTrackWheels.Length + rightTrackWheels.Length;
		
		leftTrackWheelData = new WheelDataExt[leftTrackWheels.Length];
		rightTrackWheelData = new WheelDataExt[rightTrackWheels.Length];
			
		for(int i=0;i<leftTrackWheels.Length;i++){
			leftTrackWheelData[i] = SetupWheels(leftTrackWheels[i],leftTrackBones[i]);
		}
		
		for(int i=0;i<rightTrackWheels.Length;i++){
			rightTrackWheelData[i] = SetupWheels(rightTrackWheels[i],rightTrackBones[i]);
		}
		
		
		
		leftTrackUpperWD = new WheelData[leftTrackUpperWheels.Length];
		rightTrackUpperWD = new WheelData[rightTrackUpperWheels.Length];
		
		
		for(int i=0;i<leftTrackUpperWheels.Length;i++){
			leftTrackUpperWD[i] = SetupUpperWheels(leftTrackUpperWheels[i]);			
		}
		
		for(int i=0;i<rightTrackUpperWheels.Length;i++){
			rightTrackUpperWD[i] = SetupUpperWheels(rightTrackUpperWheels[i]);			
		}
		
		
		
		Vector3 offset = transform.position;
		offset.z +=0.01f;
		transform.position = offset;	
		
		/*if(useRecommendedCurveSettings)
			setRecommendedCurveSettings();*/
		
		SetupAxis();
	}
	

	
	
	void Start(){
		if(!autoCOM)
			GetComponent<Rigidbody>().centerOfMass = COM.localPosition;			
		
	}
	
	
	private void SetupAxis(){
		
		wheelsAndBonesAxisSettings.WRAxisPointer =
			WheelsAxisSettings.SwitchAxis(wheelsAndBonesAxisSettings.wheelsRotationAxis);
		
		wheelsAndBonesAxisSettings.WPAxisPointer =
			WheelsAxisSettings.SwitchAxis(wheelsAndBonesAxisSettings.wheelsPositionAxis);
		
		wheelsAndBonesAxisSettings.BPAxisPointer =
			WheelsAxisSettings.SwitchAxis(wheelsAndBonesAxisSettings.bonesPositionAxis);
		
		trackTextireAnimationSettings.TTAxisPointer = 
			TrackTextureDirectionSettings.SwitchAxis(trackTextireAnimationSettings.trackTextureDirection);
			
	}
		
	private WheelDataExt SetupWheels(Transform wheel, Transform bone){
		WheelDataExt result = new WheelDataExt();
		
		GameObject go = new GameObject("Collider_"+wheel.name);//(GameObject)Instantiate(wheelCollider,wheel.position,Quaternion.identity);//
		go.transform.parent = transform; 
		go.transform.position = wheel.position; 
		go.transform.localRotation = Quaternion.Euler(0,wheel.localRotation.y,0); 
		
		WheelCollider col = (WheelCollider) go.AddComponent(typeof(WheelCollider));
		
		col.mass = colliderFromPrefab.mass;
		col.center = colliderFromPrefab.center;
		col.radius = colliderFromPrefab.radius;
		col.suspensionDistance = colliderFromPrefab.suspensionDistance;
		col.suspensionSpring = colliderFromPrefab.suspensionSpring;
		col.forwardFriction = colliderFromPrefab.forwardFriction;
		col.sidewaysFriction = colliderFromPrefab.sidewaysFriction;
						
		result.wheelTransform = wheel;
		result.boneTransform = bone;
		result.col = col;
		result.wheelStartPos = wheel.transform.localPosition;
		result.boneStartPos = bone.transform.localPosition;
		result.wheelRotationAngles = wheel.localEulerAngles;
		
		return result;
	}
	
	
	private WheelData SetupUpperWheels(Transform wheel){
		WheelData result = new WheelData();
		
		result.wheelTransform = wheel;
		result.wheelStartPos = wheel.transform.localPosition;
		result.wheelRotationAngles = wheel.localEulerAngles;
		
		return result;
		
	}
	
	
	private float RPMtoKMPH(float radius, float rpm){
		float length = 2.0f*Mathf.PI*radius;
		
		float result = rpm*length*60.0f/1000.0f; //km/ph;
				
		return result;
	}
	
	
	// Use this for initialization
	
		
	private Vector3 CalculateWheelOrBonePosition(Transform w,WheelCollider col,Vector3 startPos, bool isWheel){
		WheelHit hit;
		
		Vector3 lp = w.localPosition;
		if (col.GetGroundHit(out hit)) {
			lp[wheelsAndBonesAxisSettings.WPAxisPointer] -= Vector3.Dot(w.position - hit.point, transform.up);
			
			if(isWheel){
				lp[wheelsAndBonesAxisSettings.WPAxisPointer] += wheelsOffset;
				
				if(wheelsAndBonesAxisSettings.inverseWheelsPosition)
					lp[wheelsAndBonesAxisSettings.WPAxisPointer] *=-1.0f;
				
				
			}else{
				lp[wheelsAndBonesAxisSettings.BPAxisPointer] += bonesOffset;
				
				if(wheelsAndBonesAxisSettings.inverseBonesPosition)
					lp[wheelsAndBonesAxisSettings.BPAxisPointer] *=-1.0f;
			}
			
		}else {
			
			if(isWheel){
				lp[wheelsAndBonesAxisSettings.WPAxisPointer] = startPos[wheelsAndBonesAxisSettings.WPAxisPointer] - noGroundedColOffset;
				
				if(wheelsAndBonesAxisSettings.inverseWheelsPosition)
					lp[wheelsAndBonesAxisSettings.WPAxisPointer] *=-1.0f;
				
			}else{
				lp[wheelsAndBonesAxisSettings.BPAxisPointer] = startPos[wheelsAndBonesAxisSettings.BPAxisPointer] - noGroundedColOffset;				
				
				if(wheelsAndBonesAxisSettings.inverseBonesPosition)
					lp[wheelsAndBonesAxisSettings.BPAxisPointer] *=-1.0f;
			}
						
		}
		
		return lp;	
				
	}
	
		
	private float CalculateSmoothRpm(WheelDataExt[] w){
		float rpm = 0.0f;
		
		List<int> grWheelsInd = new List<int>();
		
		
		for(int i = 0;i<w.Length;i++){
			if(w[i].col.isGrounded){
				grWheelsInd.Add(i);
			}
		}
		
		if(grWheelsInd.Count == 0){
			foreach(WheelDataExt wd in w){
				rpm +=wd.col.rpm;				
			}
			
			rpm /= w.Length;
						
		}else{
									
			for(int i = 0;i<grWheelsInd.Count;i++){
				rpm +=w[grWheelsInd[i]].col.rpm;	
			}
			
			rpm /= grWheelsInd.Count;
		}
		
		return rpm;
	}
	
	
	public class RFRD{
		
		public float rotationForce = 0.0f;
		public float rotationDamper= 0.0f;
		
		public RFRD(){
			rotationForce = 0.0f;
			rotationDamper= 0.0f;
		}
		
		public RFRD(float rf, float rd){
			rotationForce = rf;
			rotationDamper = rd;
		}
		
		public static RFRD operator +(RFRD m1,RFRD m2){
			return new RFRD(m1.rotationForce + m2.rotationForce, m1.rotationDamper + m2.rotationDamper);
			
		}
			
		
			
	}
			
	private RFRD CalculateMotorForce(WheelCollider col, float accel, float steer){
		WheelFrictionCurve fc = colliderFromPrefab.sidewaysFriction;
		
		RFRD rfrd = new RFRD();
				
		float wheelSpeed = Mathf.Abs(RPMtoKMPH(col.radius,col.rpm));
		
		float motorTorque = 0.0f;
		float brakeTorque = 0.0f;
		
		if(accel == 0 && steer == 0){
			brakeTorque = accelerationConfiguration.brake.Evaluate(wheelSpeed);
			motorTorque =0.0f;
			
			rfrd.rotationForce = 0.0f;
			rfrd.rotationDamper = 0.0f;
		}else if( accel == 0.0f){
			
			if(!col.isGrounded){
				motorTorque = steer*accelerationConfiguration.acceleration.Evaluate(wheelSpeed);
				
				rfrd.rotationForce = 0.0f;
				rfrd.rotationDamper = 0.0f;
			}else{
				
				rfrd.rotationForce = rotationOnStayConfiguration.rotateSpeed.Evaluate(wheelSpeed) / wheelsCount;
				rfrd.rotationDamper = rotationDamper.Evaluate(wheelSpeed) / wheelsCount;
				
				motorTorque = 0.0f;			
				
				fc.asymptoteValue *= sidewaysFrictionAsymptoteFactor;
				fc.extremumValue *= sidewaysFrictionExtremumFactor;
			}
			 
			brakeTorque = rotationOnStayConfiguration.brake.Evaluate(wheelSpeed);			
			
		}else{
		
			if(steer!=0.0f)		
				if(!col.isGrounded){
					
					rfrd.rotationForce = 0.0f;
					rfrd.rotationDamper = 0.0f;
				}else{
					
					rfrd.rotationForce = rotationOnAccelerationConfiguration.rotateSpeed.Evaluate(wheelSpeed) / wheelsCount;
					rfrd.rotationDamper = rotationDamper.Evaluate(wheelSpeed) / wheelsCount;
					
					fc.asymptoteValue *= sidewaysFrictionAsymptoteFactor;
					fc.extremumValue *= sidewaysFrictionExtremumFactor;
				}
							
			motorTorque = accel*accelerationConfiguration.acceleration.Evaluate(wheelSpeed);
			
			if(col.rpm > 0 && accel < 0){
				brakeTorque = accelerationConfiguration.brake.Evaluate(wheelSpeed);
			}else if(col.rpm < 0 && accel > 0){
				brakeTorque = accelerationConfiguration.brake.Evaluate(wheelSpeed);
			}else{		
				if(steer!=0.0f)
					brakeTorque = rotationOnAccelerationConfiguration.brake.Evaluate(wheelSpeed);
				else
					brakeTorque = 0.0f;				
			}
				
			
			
		}
		
		//col.suspensionSpring = js;
		
		col.motorTorque = motorTorque;
		col.brakeTorque = brakeTorque;
		
		
		col.sidewaysFriction = fc;
		
		return rfrd;
		
		
	}
	
	private Vector3 rotationVector = Vector3.zero;
	
	public void UpdateWheels(float accel,float steer){
		RFRD rfrd = new RFRD();
		
		rfrd =TrackUpdate(accel,steer,leftTrackWheelData,leftTrack,ref leftTrackTextureOffset,leftTrackUpperWD,ref leftTrackMiddleRPM);
		rfrd +=TrackUpdate(accel,-steer,rightTrackWheelData,rightTrack,ref rightTrackTextureOffset,rightTrackUpperWD,ref rightTrackMiddleRPM);
		
		rotationVector.y = steer*rfrd.rotationForce;
		rotationVector.z = -steer*rfrd.rotationDamper;
			
		if(steer!=0.0f){
			
			GetComponent<Rigidbody>().AddRelativeTorque(rotationVector,ForceMode.Acceleration);			//-steer*rfrd.rotationDamper
		}
		
		//Debug.DrawRay(transform.position,rigidbody.angularVelocity,Color.red);
		//Debug.Log(rigidbody.angularVelocity);
	}	
	
	
	
	private RFRD TrackUpdate(float accel,float steer,WheelDataExt[] WD, GameObject track, ref Vector2 trackTextureOffset, WheelData[] upperWheels, ref float middleRPM){
		
		float delta = Time.fixedDeltaTime;
		
		RFRD rfrd = new RFRD();
		
		float trackRpm = 0.0f;
		
		trackRpm = CalculateSmoothRpm(WD);
		middleRPM = trackRpm;
		
		float RPMtoDeg = delta * trackRpm * 360.0f / 60.0f;
		
		if(wheelsAndBonesAxisSettings.inverseWheelsRotation)
			RPMtoDeg *=-1.0f;
		
								
		foreach (WheelDataExt w in WD){
			w.wheelTransform.localPosition = CalculateWheelOrBonePosition(w.wheelTransform,w.col,w.wheelStartPos,true);
			w.boneTransform.localPosition = CalculateWheelOrBonePosition(w.boneTransform,w.col,w.boneStartPos,false);
			
			w.wheelRotationAngles[wheelsAndBonesAxisSettings.WRAxisPointer] = 
							Mathf.Repeat(w.wheelRotationAngles[wheelsAndBonesAxisSettings.WRAxisPointer] + RPMtoDeg,360.0f);
			
			w.wheelTransform.localEulerAngles = w.wheelRotationAngles;	
			
			rfrd += CalculateMotorForce(w.col,accel,steer);			
			
			//Debug.Log(rfrd.rotationDamper);
			
		}
			
		
		if(trackTextireAnimationSettings.inverseTextureDirection)			
			trackRpm *=-1.0f;
		
		trackTextureOffset[trackTextireAnimationSettings.TTAxisPointer] = Mathf.Repeat(trackTextureOffset[trackTextireAnimationSettings.TTAxisPointer] + delta*trackRpm*trackTextureSpeed/60.0f,1.0f);
				
		
		if(track.GetComponent<Renderer>().material.GetTexture("_MainTex")){
			track.GetComponent<Renderer>().material.SetTextureOffset("_MainTex",trackTextureOffset);	
		}
		
		
		
		if(track.GetComponent<Renderer>().material.GetTexture("_BumpMap")){
			track.GetComponent<Renderer>().material.SetTextureOffset("_BumpMap",trackTextureOffset);			
		}
		
				
				
		foreach (WheelData w in upperWheels){
						
			w.wheelRotationAngles[wheelsAndBonesAxisSettings.WRAxisPointer] = 
							Mathf.Repeat(w.wheelRotationAngles[wheelsAndBonesAxisSettings.WRAxisPointer] + RPMtoDeg,360.0f);
			
			w.wheelTransform.localEulerAngles = w.wheelRotationAngles;			
			
		}
		
		return rfrd;	
		
	}
	
	
	
	void OnGUI(){
		if(showDebugInfo){
			float spd = CurrentSpeed;
			GUILayout.BeginVertical();
				GUILayout.Label("Rigidbody Speed = "+spd.ToString()+" km/ph",debugInfoStyle);
				GUILayout.Label("Left Track RPM = "+leftTrackMiddleRPM.ToString(),debugInfoStyle);
				GUILayout.Label("Left Track Speed = "+RPMtoKMPH(leftTrackWheelData[0].col.radius,leftTrackMiddleRPM)+" km/ph",debugInfoStyle);
				//GUILayout.Label("Left Track BTD = "+brakeTorqueDynamicsOnMoveForward.Evaluate(Mathf.Abs(leftTrackMiddleRPM)),debugInfoStyle);			
							
				GUILayout.Label("Right Track RPM = "+rightTrackMiddleRPM.ToString(),debugInfoStyle);
				GUILayout.Label("Right Track Speed = "+RPMtoKMPH(rightTrackWheelData[0].col.radius,rightTrackMiddleRPM)+" km/ph",debugInfoStyle);
				GUILayout.Label("Rotation vector = "+rotationVector,debugInfoStyle);
				
				//GUILayout.Label("Right Track BTD = "+brakeTorqueDynamicsOnMoveForward.Evaluate(Mathf.Abs(rightTrackMiddleRPM)),debugInfoStyle);				
				
			GUILayout.EndVertical();
		}		
	}
}

