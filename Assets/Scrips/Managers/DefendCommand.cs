using UnityEngine;

public class DefendCommand : ICommand
{
    public void Execute(UnitBase executor)
    {
        Debug.Log($"{executor.name} は防御の体勢を取った！");

        // 仮の防御処理：次のターンまでダメージ半減など
        if (executor is PlayerUnit player)
        {
            player.IsDefending = true;
        }
    }
}
