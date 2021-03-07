using dTerm.Infra.ConPTY;

namespace dTerm.UI.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            var terminalConsole = new TerminalConsole();

            terminalConsole.Run("cmd.exe");
        }
    }
}
