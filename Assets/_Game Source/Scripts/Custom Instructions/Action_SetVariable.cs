using System;
using System.Threading.Tasks;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.VisualScripting;

[Serializable]
public class Action_SetVariable : Instruction
{

    public string varName;
    public string varValue;
    
    protected override Task Run(Args args)
    {
       GameConfig.Instance.gameProgression.SetVariable(varName, varValue);
        return DefaultResult;
    }
}
