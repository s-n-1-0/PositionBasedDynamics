using UnityEngine;
using UnityEngine.Animations;

public class ParticleObject : MonoBehaviour
{
	public float mass { get; private set; } = 1;
	Transform _transform;


	public Transform GetTransform()
	{
		if (_transform == null) { _transform = transform; }
		return _transform;
	}

	public void SetPosition(Vector3 position)
	{
		GetTransform().position = position;
	}

	public Vector3 GetPosition()
	{
		return GetTransform().position;
	}

    public void SetPositionConstraint(Transform sourceTransform)
    {
        mass = 1e+15f;
        var positionConstraint = gameObject.AddComponent<PositionConstraint>();
        positionConstraint.constraintActive = true;
        var constraintSource = new ConstraintSource();
        constraintSource.sourceTransform = sourceTransform;
		constraintSource.weight = 1;
        positionConstraint.AddSource(constraintSource);
    }
}
