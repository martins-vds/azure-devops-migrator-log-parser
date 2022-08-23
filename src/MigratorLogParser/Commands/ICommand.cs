using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MigratorLogParser.Commands
{
    public interface ICommand
    {
        void Execute();
    }
}
