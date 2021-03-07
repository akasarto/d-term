using dTerm.Infra.ConPTY;
using System;

namespace dTerm.UI.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            var terminal = new Terminal();

            terminal.Run("C:\\Users\\akasarto\\scoop\\apps\\git-with-openssh\\current\\bin\\sh.exe");
        }
    }
}
