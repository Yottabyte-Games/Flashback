using UnityEngine;
using UnityEngine.Serialization;
namespace Imp_Assets.PROMETEO___Car_Controller.Scripts
{
	public class CameraFollow : MonoBehaviour {

		[FormerlySerializedAs("carTransform")] public Transform CarTransform;
		[FormerlySerializedAs("followSpeed")] [Range(1, 10)]
		public float FollowSpeed = 2;
		[FormerlySerializedAs("lookSpeed")] [Range(1, 10)]
		public float LookSpeed = 5;
		Vector3 _initialCameraPosition;
		Vector3 _initialCarPosition;
		Vector3 _absoluteInitCameraPosition;

		void Start(){
			_initialCameraPosition = gameObject.transform.position;
			_initialCarPosition = CarTransform.position;
			_absoluteInitCameraPosition = _initialCameraPosition - _initialCarPosition;
		}

		void FixedUpdate()
		{
			//Look at car
			Vector3 lookDirection = (new Vector3(CarTransform.position.x, CarTransform.position.y, CarTransform.position.z)) - transform.position;
			Quaternion rot = Quaternion.LookRotation(lookDirection, Vector3.up);
			transform.rotation = Quaternion.Lerp(transform.rotation, rot, LookSpeed * Time.deltaTime);

			//Move to car
			Vector3 targetPos = _absoluteInitCameraPosition + CarTransform.transform.position;
			transform.position = Vector3.Lerp(transform.position, targetPos, FollowSpeed * Time.deltaTime);

		}

	}
}
