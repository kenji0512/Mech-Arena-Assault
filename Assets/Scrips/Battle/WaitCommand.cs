using UnityEngine;

public class WaitCommand : ICommand
{
    public void Execute(UnitBase executor)
    {
        Debug.Log($"{executor.name} は待機した");
        // 何もしない
    }
}
