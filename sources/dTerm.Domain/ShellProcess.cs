using System;

namespace dTerm.Domain
{
    public class ShellProcess
    {
        public const int NameMaxLength = 50;
        public const int IconMaxLength = 2000;
        public const int ProcessExecutablePathMaxLength = 2000;
        public const int ProcessStartupArgsMaxLength = 2000;

        public Guid Id { get; set; }
        public string Icon { get; set; }
        public string Name { get; set; }
        public int OrderIndex { get; set; }
        public string ProcessExecutablePath { get; set; }
        public string ProcessStartupArgs { get; set; }
        public DateTime UTCCreation { get; set; }
    }
}
