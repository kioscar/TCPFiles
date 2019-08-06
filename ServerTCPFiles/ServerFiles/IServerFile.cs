using System;
using System.Collections.Generic;
using System.Text;

namespace ServerFiles
{
   public interface IServerFile
    {
        void DoAction(string accion);
        void MuestraError(string error);
    }
}
