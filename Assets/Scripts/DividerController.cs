using Deform;
using UnityEngine;

public class DividerController : MonoBehaviour
{
	public GameObject CubePartPrefab;

	private Transform _playerTransform;
	public MagnetDeformer PlayerMagnetDeformer;
	private bool _needToMakeBigger = false;
	private bool _needToMakeSmaller = false;
	private Vector3 CutedCubeScale, AmountOfCutting, AmountOfApplingForSingleBlock;
	private Vector3 AppliedCubeScale, AmountOfAppling, AmountOfCuttingForSingleLaser;
	private Vector3 ChangedScale;
	private void Start()
	{
		_playerTransform = GetComponent<Transform>();
		AmountOfAppling = new Vector3(0.01f, 0.01f, 0.01f);
		AmountOfCutting = new Vector3(0.01f, 0.01f, 0.01f);

		AmountOfApplingForSingleBlock = new Vector3(0.05f, 0.05f, 0.05f);
		AmountOfCuttingForSingleLaser = new Vector3(0.2f, 0.2f, 0.2f);

		AppliedCubeScale = new Vector3();
		CutedCubeScale = new Vector3();
		ChangedScale = new Vector3(1, 1, 1);
	}
	private void Update()
	{
		
	}
	private void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Laser"))
		{
			ChangedScale = _playerTransform.localScale - AmountOfCuttingForSingleLaser;
			for (int i = 0; i < 4; i++)
			{
				Instantiate(CubePartPrefab, new Vector3(2, -1, 0), Quaternion.identity);
			}
		}
	}
	private void OnCollisionEnter(Collision other)
	{
		if (other.gameObject.CompareTag("CubePart"))
		{
			ChangedScale = _playerTransform.localScale + AmountOfApplingForSingleBlock;
			Collider CubePartCollider = other.gameObject.GetComponent<Collider>();
			CubePartCollider.enabled = false;
			Deformable CubePartDeformable = other.gameObject.GetComponentInChildren<Deformable>();
			CubePartDeformable.AddDeformer(PlayerMagnetDeformer);
			SpringJoint CubePartJoint = other.gameObject.GetComponent<SpringJoint>();
			CubePartJoint.connectedBody = GetComponent<Rigidbody>();
			CubePartJoint.connectedAnchor = new Vector3(0, 0, 0);
			CubePartDestroyController DestroyController = other.gameObject.GetComponent<CubePartDestroyController>();
			DestroyController.DestroythisPart(1f);
		}

	}
	private void FixedUpdate()
	{
		if (_playerTransform.localScale.x != ChangedScale.x)
		{
			if (_playerTransform.localScale.x > ChangedScale.x && _needToMakeBigger != true)
			{
				_needToMakeBigger = true;
			}
			else if (_playerTransform.localScale.x < ChangedScale.x && _needToMakeSmaller != true)
			{
				_needToMakeSmaller = true;
			}
		}

		if (_needToMakeBigger)
		{
			AppliedCubeScale = _playerTransform.localScale + AmountOfAppling;
			_playerTransform.localScale = AppliedCubeScale;
			if (_playerTransform.localScale.x >= ChangedScale.x)
			{
				_playerTransform.localScale = ChangedScale;
				_needToMakeBigger = false;
			}

		}

		if (_needToMakeSmaller)
		{
			CutedCubeScale = _playerTransform.localScale - AmountOfCutting;
			_playerTransform.localScale = CutedCubeScale;
			if (_playerTransform.localScale.x <= ChangedScale.x)
			{
				_playerTransform.localScale = ChangedScale;
				_needToMakeSmaller = false;
			}

		}
	}
}
