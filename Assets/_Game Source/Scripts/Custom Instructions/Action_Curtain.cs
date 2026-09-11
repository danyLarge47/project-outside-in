using System;
using System.Threading.Tasks;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.VisualScripting;

[Serializable]
public class Action_Curtain : Instruction
{
    public bool showCurtain;
    protected override async Task Run(Args args)
    {
        if (showCurtain)
        await GameConfig.Instance.showCurtain.Run();
            
            else 
        await GameConfig.Instance.hideCurtain.Run();
    }
}
