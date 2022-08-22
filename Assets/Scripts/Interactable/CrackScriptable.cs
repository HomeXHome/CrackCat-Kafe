using UnityEngine;

[CreateAssetMenu(menuName ="Crack Type / New Crack", fileName = "Crack")]
public class CrackScriptable : ScriptableObject
{
    public enum CrackType
    {
        TypeOne,
        TypeTwo,
        TypeThree
    }
    public CrackType type;


    public CrackType ReturnCrackType()
    {
        return type;
    }
}
