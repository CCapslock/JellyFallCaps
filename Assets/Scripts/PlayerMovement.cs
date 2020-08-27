using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
	public float YDelay;

	[SerializeField] private Rigidbody _playerRigidbody;
	[SerializeField] private Vector3 _delay;
	[SerializeField] private Vector3 _startPosition;
	[SerializeField] private Vector3 _zeroVelocity;
	[SerializeField] private bool _delayCounted;
	[SerializeField] private bool _isCollisonWithPipe;
	[SerializeField] private bool _offMomentum;
	[SerializeField] private InputController _inputController;
	[SerializeField] private Transform _playerTransform;
	[SerializeField] private Vector2 _minScreenPosition, _maxScreenPosition;

	public GameObject CubeLittlePart;

	private void Start()
	{
		_playerRigidbody = GetComponent<Rigidbody>();
		_playerTransform = GetComponent<Transform>();
		_inputController = FindObjectOfType<InputController>();
		_delay = new Vector3(0, YDelay);
		_delay = new Vector3(0, 0, 0);
		_minScreenPosition = _inputController.CameraForInput.ViewportToWorldPoint(new Vector2(0, 0));
		_maxScreenPosition = _inputController.CameraForInput.ViewportToWorldPoint(new Vector2(1, 1));
	}
	private void FixedUpdate()
	{



		if (Input.GetKeyDown(KeyCode.F))
		{
			Instantiate(CubeLittlePart, new Vector3(2, -1, 0), Quaternion.identity);
		}

		if (_inputController.DragingStarted)
		{
			if (!_delayCounted || _isCollisonWithPipe)
			{
				_delay = _inputController.TouchPosition;
				_startPosition = _playerTransform.position;
				_delayCounted = true;
				//_playerRigidbody.isKinematic = true;
			}
			//_playerTransform.position = _startPosition + _inputController.TouchPosition - _delay;
			//transform.position = _playerTransform.position;
			_playerRigidbody.MovePosition(_startPosition + _inputController.TouchPosition - _delay);
			//_playerRigidbody.isKinematic = false;
			//Debug.Log(_playerTransform.position);
			_offMomentum = false;
		}
		else if (!_inputController.DragingStarted && !_offMomentum)
		{
			_playerRigidbody.isKinematic = true;
			_playerRigidbody.isKinematic = false;
			_offMomentum = true;
		}
		else
		{
			_delayCounted = false;
		}
		//ограничение экрана
		//transform.position = new Vector3(Mathf.Clamp(transform.position.x, _minScreenPosition.x, _maxScreenPosition.x), Mathf.Clamp(transform.position.y, _minScreenPosition.y, _maxScreenPosition.y), transform.position.z);
	}
	private void OnCollisionEnter(Collision other)
	{
		if (other.gameObject.CompareTag("Obstacle"))
		{
			_isCollisonWithPipe = true;
		}
	}
	private void OnCollisionStay(Collision other)
	{
		if (other.gameObject.CompareTag("Obstacle"))
		{
			_isCollisonWithPipe = true;
		}
	}
	private void OnCollisionExit(Collision other)
	{
		if (other.gameObject.CompareTag("Obstacle"))
		{
			_isCollisonWithPipe = false;
			//_playerRigidbody.isKinematic = true;
			//_playerRigidbody.isKinematic = false;
		}
	}

	private void FrezzeAllPositions(bool NeedToFrezze)
	{
		Debug.Log(1);
		if (NeedToFrezze)
			_playerRigidbody.constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionY;
		else
			_playerRigidbody.constraints = RigidbodyConstraints.FreezePosition;
	}
}
