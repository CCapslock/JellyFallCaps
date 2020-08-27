using UnityEngine;

public class LVLGenerator : MonoBehaviour
{
	public GameObject[] ObstaclePrefabs;
	public GameObject[] ConnectorPrefabs;
	public float ObstacleSpeed = 0.5f;
	public int AmountofMovingObstacles = 3;

	private GameObject[] _obstacles;
	private GameObject[] _connectors;
	private GameObject[] _obstaclesForMovement;
	private GameObject[] _connectorsForMovement;
	private GameObject _lastPlacedObstacle;
	//private ObstacleController[] _obstacleControllers;
	private float _obstacleLength = 40f;
	private float _connectorLength = 4f;
	private Vector3 _transformVector;
	private Vector3 _addingObstaclesVector;
	private Vector3 _movingVector;
	private Vector3 _endingVector;
	private Vector3 _poolVector;

	private void Start()
	{
		_poolVector = new Vector3(0, -20, 0);
		_endingVector = new Vector3(0, 0, -40);
		_obstacles = new GameObject[ObstaclePrefabs.Length];
		_connectors = new GameObject[ConnectorPrefabs.Length];
		//_obstacleControllers = new ObstacleController[ObstaclePrefabs.Length];
		Vector3 PoolTransformVector = new Vector3(0, 10, 0);
		for (int i = 0; i < ObstaclePrefabs.Length; i++)
		{
			_obstacles[i] = Instantiate(ObstaclePrefabs[i], PoolTransformVector, Quaternion.identity);
			//_obstacleControllers[i] = _obstacles[i].GetComponent<ObstacleController>();
			//_obstacleControllers[i].PoolPositionVector = PoolTransformVector;
			PoolTransformVector.x += 10;
		}
		for (int i = 0; i < ConnectorPrefabs.Length; i++)
		{
			_connectors[i] = Instantiate(ConnectorPrefabs[i], PoolTransformVector, Quaternion.identity);
			//_obstacleControllers[i] = _obstacles[i].GetComponent<ObstacleController>();
			//_obstacleControllers[i].PoolPositionVector = PoolTransformVector;
			PoolTransformVector.x += 10;
		}
		_obstaclesForMovement = new GameObject[AmountofMovingObstacles];
		_connectorsForMovement = new GameObject[AmountofMovingObstacles];
		AddFirstLayerOfObstacles(AmountofMovingObstacles);
		_addingObstaclesVector = new Vector3(0, 0, _obstacleLength / 2 + _connectorLength / 2);
	}

	private void FixedUpdate()
	{
		MoveObstacles();
		CheckForObstacleDestroy();
	}

	private void CheckForObstacleDestroy()
	{
		for (int i = 0; i < _obstaclesForMovement.Length; i++)
		{
			if (_obstaclesForMovement[i].transform.position.z <= _endingVector.z)
			{
				AddNextObstacle(false, i);
			}
			if (_connectorsForMovement[i].transform.position.z <= _endingVector.z)
			{
				AddNextObstacle(true, i);
			}
		}
	}
	private void MoveObstacles()
	{
		for (int i = 0; i < _obstaclesForMovement.Length; i++)
		{
			_movingVector = _obstaclesForMovement[i].transform.position;
			_movingVector.z -= ObstacleSpeed;
			_obstaclesForMovement[i].transform.position = _movingVector;
		}
		for (int i = 0; i < _connectorsForMovement.Length; i++)
		{
			_movingVector = _connectorsForMovement[i].transform.position;
			_movingVector.z -= ObstacleSpeed;
			_connectorsForMovement[i].transform.position = _movingVector;
		}
	}

	private void AddFirstLayerOfObstacles(int AmountOfObstacles)
	{
		int ObstacleNum = 0;
		int ConnectorNum = 0;
		bool NumCounted = false;
		bool NumCountedForConnector = false;
		for (int i = 0; i < AmountOfObstacles; i++)
		{
			do
			{
				ObstacleNum = Random.Range(0, _obstacles.Length);
				//Debug.Log(ObstacleNum);
				if (_obstacles[ObstacleNum] != null)
				{
					NumCounted = true;
				}
			}
			while (!NumCounted);

			//трубы
			_obstacles[ObstacleNum].transform.position = _transformVector;
			_transformVector.z += _connectorLength / 2 + _obstacleLength / 2;

			if (i == AmountOfObstacles - 1)
				_lastPlacedObstacle = _obstacles[ObstacleNum];

			_obstaclesForMovement[i] = _obstacles[ObstacleNum];
			_obstacles[ObstacleNum] = null;


			//соеденители

			do
			{
				ConnectorNum = Random.Range(0, _connectors.Length);
				//Debug.Log(ObstacleNum);
				if (_connectors[ConnectorNum] != null)
				{
					NumCountedForConnector = true;
				}
			}
			while (!NumCountedForConnector);

			_connectors[ConnectorNum].transform.position = _transformVector;
			_transformVector.z += _connectorLength / 2 + _obstacleLength / 2;

			if (i == AmountOfObstacles - 1)
				_lastPlacedObstacle = _connectors[ConnectorNum];

			_connectorsForMovement[i] = _connectors[ConnectorNum];
			_connectors[ConnectorNum] = null;

			NumCounted = false;
			NumCountedForConnector = false;
		}
	}

	public void AddNextObstacle(bool IsConnector, int DestroingObstacleNum)
	{
		if (IsConnector)
		{
			int ConnectorNum = 0;
			bool NumCounted = false;
			do
			{
				ConnectorNum = Random.Range(0, _connectors.Length);
				//Debug.Log(ObstacleNum);
				if (_connectors[ConnectorNum] != null)
				{
					NumCounted = true;
				}
			}
			while (!NumCounted);
			for (int i = 0; i < _connectors.Length; i++)
			{
				if (_connectors[i] == null)
				{
					_connectors[i] = _connectorsForMovement[DestroingObstacleNum];
					_connectors[i].transform.position = _poolVector;
					_connectorsForMovement[DestroingObstacleNum] = _connectors[ConnectorNum];
					_connectors[ConnectorNum] = null;
					break;
				}
			}
			_connectorsForMovement[DestroingObstacleNum].transform.position = _lastPlacedObstacle.transform.position + _addingObstaclesVector;
			_lastPlacedObstacle = _connectorsForMovement[DestroingObstacleNum];
		}
		else
		{
			int ObstacleNum = 0;
			bool NumCounted = false;
			do
			{
				ObstacleNum = Random.Range(0, _obstacles.Length);
				//Debug.Log(ObstacleNum);
				if (_obstacles[ObstacleNum] != null)
				{
					NumCounted = true;
				}
			}
			while (!NumCounted);
			for (int i = 0; i < _obstacles.Length; i++)
			{
				if (_obstacles[i] == null)
				{
					_obstacles[i] = _obstaclesForMovement[DestroingObstacleNum];
					_obstacles[i].transform.position = _poolVector;
					_obstaclesForMovement[DestroingObstacleNum] = _obstacles[ObstacleNum];
					_obstacles[ObstacleNum] = null;
					break;
				}
			}
			_obstaclesForMovement[DestroingObstacleNum].transform.position = _lastPlacedObstacle.transform.position + _addingObstaclesVector;
			_lastPlacedObstacle = _obstaclesForMovement[DestroingObstacleNum];
		}
	}
}
