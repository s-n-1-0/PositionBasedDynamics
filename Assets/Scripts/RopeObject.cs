using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using static UnityEngine.ParticleSystem;

public class RopeObject : MonoBehaviour
{
	List<ParticleObject> _particles = new();

    /// <summary>
    /// 指定した場合、ロープの終点が指定したオブジェクトに固定される。
    /// </summary>
    public Transform endHandle = null;

	public IReadOnlyList<ParticleObject> particles => _particles;
	public Color gizmosColor { get; set; } = Color.green;
	public string gizmosLabel { get; set; }

	public int initialParticlesCount = 8;

    private void Awake()
    {
		for (int i = 0; i < initialParticlesCount; i++)
		{
			var obj = new GameObject($"Particle {i + 1}");
			obj.transform.parent = transform;
            obj.transform.localPosition = new Vector3(0, 0.5f * -i, 0);
            var particle = obj.AddComponent<ParticleObject>();

			if (i == 0) particle.SetPositionConstraint(transform);
			if (endHandle && i == initialParticlesCount - 1) particle.SetPositionConstraint(endHandle);

            _particles.Add(particle);

        }

		GetComponent<RopeSolver>().Generate();
    }



#if UNITY_EDITOR
    void OnDrawGizmos()
	{
		if (_particles.Count == 0) { return; }

		const float radius = 0.1f;
		UnityEditor.Handles.Label(
			_particles[0].transform.position + 3.5f * radius * Vector3.up,
			gizmosLabel
		);

		for (int i = 0; i < _particles.Count; i++)
		{
			Gizmos.color = gizmosColor;
			Gizmos.DrawSphere(_particles[i].transform.position, radius);
			if (i < _particles.Count - 1)
			{
				Gizmos.DrawLine(_particles[i].transform.position, _particles[i + 1].transform.position);
			}
		}
	}
#endif
}