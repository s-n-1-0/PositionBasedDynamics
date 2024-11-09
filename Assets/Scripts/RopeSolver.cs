using System;
using UnityEngine;

/// <summary>
/// アルゴリズムの選択と実行サイクルを管理する
/// </summary>
[RequireComponent(typeof(RopeObject))]
public class RopeSolver : MonoBehaviour
{
	enum Algorithm
	{
		PBD,
		XPBD,
	}

	[SerializeField, HideInInspector] Algorithm _algorithm = Algorithm.PBD;
	[SerializeReference] ISimulationParameter _parameter;

	RopeObject _ropeObject;
	ISimulator _simulator;


	public void Generate()
	{
        _ropeObject = GetComponent<RopeObject>();
        _simulator = GenerateSimulator(_algorithm, _parameter, _ropeObject);
    }

	void FixedUpdate()
	{
		if(_ropeObject) UpdateManually(Time.deltaTime); 
	}

	public void UpdateManually(float dt)
	{
		_simulator?.Step(dt);
	}
	

	ISimulator GenerateSimulator(Algorithm algorithm, ISimulationParameter parameter, RopeObject ropeObject)
	{
		return algorithm switch
		{
			Algorithm.PBD => new PbdSimulator((PbdSimulator.Parameter)parameter, ropeObject),
			Algorithm.XPBD => new XpbdSimulator((XpbdSimulator.Parameter)parameter, ropeObject),
			_ => throw new ArgumentOutOfRangeException()
		};
	}


#if UNITY_EDITOR
	[UnityEditor.CustomEditor(typeof(RopeSolver))]
	class RopeSolverEditor : UnityEditor.Editor
	{
		public override void OnInspectorGUI()
		{
			base.OnInspectorGUI();

			RopeSolver ropeSolver = (RopeSolver)target;

			UnityEditor.EditorGUILayout.Space(30);

			UnityEditor.EditorGUI.BeginChangeCheck();
			var algorithm = (Algorithm)UnityEditor.EditorGUILayout.EnumPopup("Algorithm", ropeSolver._algorithm);
			if (UnityEditor.EditorGUI.EndChangeCheck())
			{
				ropeSolver._algorithm = algorithm;
				ropeSolver._parameter = algorithm switch
				{
					Algorithm.PBD => new PbdSimulator.Parameter(),
					Algorithm.XPBD => new XpbdSimulator.Parameter(),
					_ => ropeSolver._parameter
				};
				UnityEditor.EditorUtility.SetDirty(ropeSolver);
			}
		}
	}
#endif
}
