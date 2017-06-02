using System;

namespace Wss3ContentRecovery
{
    public static class HelpText
    {
        public static void Display()
        {
            Console.WriteLine();
            Console.WriteLine("Arguments:");
            Console.WriteLine();
            Console.WriteLine("-server (Required)");
            Console.WriteLine("-database (Required)");
            Console.WriteLine("-connectiontimeout (Optional, default = 30)");
            Console.WriteLine("-commandtimeout (Optional, default = 30)");
            Console.WriteLine("-buffersize (Optional, default = 1000000)");
            Console.WriteLine("-whatif (Optional, default = false)");
            Console.WriteLine();
            Console.WriteLine("Examples:");
            Console.WriteLine();
            Console.WriteLine("recover.exe -server your_server_name -database your_database_name");
            Console.WriteLine("recover.exe -server your_server_name -database your_database_name -whatif");
            Console.WriteLine("recover.exe -server your_server_name -database your_database_name -connectiontimeout 60 -commandtimeout 45");
            Console.WriteLine();
            Console.WriteLine("Specifying -whatif will only test the database connection and check file name length. Directory creation and file writing will be skipped.");
            Console.WriteLine();
        }
    }
}
