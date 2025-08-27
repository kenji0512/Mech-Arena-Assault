using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public ActionExecuter _actionExcuter;

    private StageData _selectedStage;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(this.gameObject);
    }

    private void Start()
    {
        if (_actionExcuter == null)
        {
            _actionExcuter = new ActionExecuter();
        }
    }
}
