using UnityEngine;

[CreateAssetMenu(menuName ="Cats", fileName ="New Cat")]
public class ScriptableCat : ScriptableObject
{
    [Tooltip("Prefab of choosen type of a cat")]
    public GameObject catPrefab;
    [Tooltip("Maximum time for irritation to build")]
    public float irritationTime;
    [Tooltip("Name of color of a cat")]
    public string catColor;
}
