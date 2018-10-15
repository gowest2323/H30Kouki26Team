
using UnityEngine;
using UnityEngine.AI;
//http://tsubakit1.hateblo.jp/entry/2018/01/06/003209#%E5%8B%95%E7%9A%84%E3%81%ABNavMesh%E3%82%92%E3%83%99%E3%82%A4%E3%82%AF%E3%81%99%E3%82%8B
//動的に NavMesh　を焼くためのコンポーネント

[DefaultExecutionOrder(-103)]
public class BuildNavmesh : MonoBehaviour 
{
	void Awake () 
	{
		GetComponent<NavMeshSurface> ().BuildNavMesh();
	}
}