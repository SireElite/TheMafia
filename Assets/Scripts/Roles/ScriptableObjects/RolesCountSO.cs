using UnityEngine;

[CreateAssetMenu(menuName = "Room Scriptable Object")]
public class RolesCountSO : ScriptableObject 
{
    [SerializeField] private int _requiredPlayersCount;

    [field: Header("Roles")]
    [field: SerializeField] public int MafiaCount { get; private set; }
    [field: SerializeField] public int CitizenCount { get; private set; }
    [field: SerializeField] public int ProstituteCount { get; private set; }
    [field: SerializeField] public int SheriffCount { get; private set; }
    [field: SerializeField] public int DoctorCount { get; private set; }

    public int RolesCount => MafiaCount + CitizenCount + ProstituteCount + SheriffCount + DoctorCount;

    private void OnValidate()
    {
        if(RolesCount != _requiredPlayersCount)
        {
            throw new System.ArgumentOutOfRangeException();
        }
    }
}
