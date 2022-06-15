using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
	public float FollowSpeed = 2f;
	public Transform Target;

	// Transform of the camera to shake. Grabs the gameObject's transform
	// if null.
	private Transform camTransform;

	// How long the object should shake for.
	public float shakeDuration = 0f;

	// Amplitude of the shake. A larger value shakes the camera harder.
	public float shakeAmount = 0.1f;
	public float decreaseFactor = 1.0f;

	//
	[SerializeField]
	STRCamShakeProperties camShakeProp;
	STRCamShakeProperties defaultCamShakeProp;

	Vector3 originalPos;

	void Awake()
	{
		Cursor.visible = false;
		if (camTransform == null)
		{
			camTransform = GetComponent(typeof(Transform)) as Transform;
		}

		camShakeProp.decreaseFactor = 1f;
		camShakeProp.shakeAmount = .1f;
		camShakeProp.duration = 0f;

		defaultCamShakeProp = camShakeProp;
	}

	void OnEnable()
	{
		originalPos = camTransform.localPosition;
	}

	private void Update()
	{
		Vector3 newPosition = Target.position;
		newPosition.z = -10;
		transform.position = Vector3.Slerp(transform.position, newPosition, FollowSpeed * Time.deltaTime);

		if (camShakeProp.duration > 0)
		{
			//camTransform.localPosition = originalPos + Random.insideUnitSphere * shakeAmount;
			camTransform.localPosition = originalPos + Random.insideUnitSphere * camShakeProp.shakeAmount;

			//shakeDuration -= Time.deltaTime * decreaseFactor;
			camShakeProp.duration -= Time.deltaTime * decreaseFactor;
		}
		else if(camShakeProp.duration < .1f && camShakeProp.duration > 0f) {
			camShakeProp = defaultCamShakeProp;
		}
	}

	public void ShakeCamera()
	{
		originalPos = camTransform.localPosition;
		//shakeDuration = 0.2f;
		camShakeProp.duration = 0.2f;
	}
	
	/// <summary>
	/// Shake camera as long as duration
	/// </summary>
	/// <param name="duration"></param>
	public void ShakeCamera(float duration)
	{
		originalPos = camTransform.localPosition;
		//shakeDuration = duration;
		camShakeProp.duration = duration;
	}

	/// <summary>
	/// Shake camera by a config value
	/// </summary>
	/// <param name="duration">.2 is default(strong)</param>
	/// <param name="shakeAmout">.1 is default(strong)</param>
	/// <param name="decreaseFactor">1 is default normal time</param>
	public void ShakeCamera(float duration, float shakeAmount, float decreaseFactor)
	{
		originalPos = camTransform.localPosition;
		//shakeDuration = 0.2f;
		camShakeProp.duration = duration;
		camShakeProp.shakeAmount = shakeAmount;
		camShakeProp.decreaseFactor = decreaseFactor;
	}
}
