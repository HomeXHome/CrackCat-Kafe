using UnityEngine;

[CreateAssetMenu(menuName = "Crack Pipe / New Crack Pipe", fileName = "Crack Pipe")]
public class CrackPipeScriptable : ScriptableObject
{
    public enum CrackPipeType
    {
        IntCrack,
        DexCrack,
        StrCrack
    }
    public CrackPipeType type;

    public GameObject prefab;

    public CrackPipeType ReturnCrackType()
    {
        return type;
    }
}
