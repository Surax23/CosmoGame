using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game
{
    class GameObjectException : Exception
    {
        public GameObjectException():base()
        {

        }

        public override string Message => "Неверный параметр!";
    }
}