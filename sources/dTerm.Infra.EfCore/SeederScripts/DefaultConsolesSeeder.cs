using dTerm.Core;
using System;
using System.Collections.Generic;

namespace dTerm.Infra.EfCore.SeederScripts
{
    public static class DefaultConsolesSeeder
    {
        public static List<ShellProcess> GetEntities() => new List<ShellProcess>
        {
            new ShellProcess {
                Id = new Guid("036c0c75-2509-4114-976e-0c123b80ad55"),
                Icon = "Console",
                Name = "Command Promp",
                OrderIndex = 1,
                ProcessExecutablePath = "cmd.exe",
                ProcessStartupArgs = string.Empty,
                UTCCreation = DateTime.UtcNow
            },

            new ShellProcess {
                Id = new Guid("5de39ac0-296f-423c-97f1-6ee71b7a4b2d"),
                Icon = "Git",
                Name = "Git Bash",
                OrderIndex = 2,
                ProcessExecutablePath = "git-bash.exe",
                ProcessStartupArgs = "--login -i",
                UTCCreation = DateTime.UtcNow
            },

            new ShellProcess {
                Id = new Guid("4e6d5d59-d537-4b66-aa97-1bd066dde1fe"),
                Icon = "Powershell",
                Name = "Power Shell",
                OrderIndex = 3,
                ProcessExecutablePath = "powershell.exe",
                ProcessStartupArgs = string.Empty,
                UTCCreation = DateTime.UtcNow
            },

            new ShellProcess {
                Id = new Guid("a460e3b0-278e-4328-bc5b-16f5a7eb1f27"),
                Icon = "Linux",
                Name = "WSL Default Shel",
                OrderIndex = 4,
                ProcessExecutablePath = "wsl.exe",
                ProcessStartupArgs = string.Empty,
                UTCCreation = DateTime.UtcNow
            },
        };
    }
}
