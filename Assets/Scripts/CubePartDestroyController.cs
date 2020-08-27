using UnityEngine;

public class CubePartDestroyController : MonoBehaviour
{
    private Transform _cubePartTransform;
    private bool _needToMakeSmaller = false;
    private Vector3 CutedCubeScale, AmountOfCutting;
    private float MinScale = 0;

    private void Start()
    {
        _cubePartTransform = GetComponent<Transform>();
        AmountOfCutting = new Vector3(0.01f, 0.01f, 0.01f);
        CutedCubeScale = new Vector3();
    }
    public void DestroythisPart(float TimeBeforeDestroy)
    {
        _needToMakeSmaller = true;
    }
    private void FixedUpdate()
    {
        if (_needToMakeSmaller)
        {
            CutedCubeScale = _cubePartTransform.localScale - AmountOfCutting;
            _cubePartTransform.localScale = CutedCubeScale;
            if(_cubePartTransform.localScale.x <= MinScale)
                Destroy(this.gameObject);

        }
    }
}
