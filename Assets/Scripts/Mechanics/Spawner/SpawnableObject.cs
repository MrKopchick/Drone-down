using UnityEngine;

[CreateAssetMenu(fileName = "SpawnableObject", menuName = "Spawn System/Spawnable Object")]
public class SpawnableObject : ScriptableObject
{
    public string objectName;
    public GameObject prefab;
    public GameObject previewPrefab;
}