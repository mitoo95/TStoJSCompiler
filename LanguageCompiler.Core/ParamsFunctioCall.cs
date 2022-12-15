using System;
using System.Collections.Generic;

namespace TSCompiler.Core
{
    public class ParamsFunctioCall : Expresion
    {
        public List<ExpresionType> Params { get; set; }

        public ParamsFunctioCall(List<ExpresionType> @params)
        {
            Params = @params;
        }

        public override ExpresionType GetType()
        {
            if(Params != null )
            {
                for (int i = 0; i < Params.Count; i++)
                {
                    return Params[i];
                }
            }
            return null;
            /*throw new NotImplementedException();*/
        }

        public override string GenerateCode()
        {
            return "ParamsFunctionCall";
        }
    }
}
