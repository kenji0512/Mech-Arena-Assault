using UnityEngine;

[CreateAssetMenu(menuName = "Stage/StageData")]
public class StageData : ScriptableObject
{
    public string stageName;
    public string sceneName; // ex: "BattleScene1"
    public Sprite previewImage;
    public string description;
}
