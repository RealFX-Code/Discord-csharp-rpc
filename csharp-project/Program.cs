using System;
using Discord;
using System.IO;
using System.Linq;
using System.Threading;
using System.Diagnostics;
using System.Windows.Input;
using System.Threading.Tasks;

/*
WARNING! This app will only work for x86-64 systems. In layman's terms, 64-bit only.
(you can change it to be x86-32, but you will need to understand the discord SDK then.)
When running the app, make sure discord_game_sdk.dll is in the same directory as the file.
Don't worry! unless you tampered with some wierd settings, the dll will be automatically
copied to the output direcrory. (assuming you have it in the same directory as the .cs file)
*/

namespace CsharpProject
{
    class FirstSetup
    {
        public static bool WriteFile(string WTW)
        {
            try
            {
                File.WriteAllText("Settings.dat", WTW);
                return true;
            }
            catch
            {
                return false;
            }
        }
        public static void AskFile()
        {
            string ClientIDToSave;
            UInt64 ClientIDlong;
            Console.Write(@"Please Enter Your ClientID. > ");
            ClientIDToSave = Console.ReadLine();
            if (ClientIDToSave != null)
            {
                try
                {
                    ClientIDlong = (ulong)Convert.ToInt64(ClientIDToSave);
                }
                catch
                {
                    Console.WriteLine("Not a valid ClientID.");
                    return;
                }
                if (FirstSetup.WriteFile(ClientIDlong.ToString()))
                {
                    Console.WriteLine("Succsessfully saved your clientid!");
                    Console.WriteLine("Closing in 5 seconds.");
                    Thread.Sleep(5000);
                }
                else
                {
                    Console.WriteLine("Could not write file!");
                    Console.WriteLine("Closing in 5 seconds.");
                    Thread.Sleep(5000);
                }
            }
            else
            {
                Console.WriteLine("Please Enter A Client ID!");
                Console.WriteLine("Closing in 5 seconds.");
                Thread.Sleep(5000);
                return;
            }
        }
    }
    class EntryPoint
    {
        public static bool IsRunning(string name) => Process.GetProcessesByName(name).Length > 0;
        public static void Main(string[] args)
        {
            Int64 CLIENTID = 0;
            Console.Title = "RealFX Discord SDK | Connecting To Discord...";
            Console.ForegroundColor = ConsoleColor.White;
            /*foreach (string arg in args)
            {
                Console.WriteLine(arg);
            }*/
            if (!File.Exists("Settings.dat"))
            {
                FirstSetup.AskFile();
                return;
            }
            else
            {
                Console.WriteLine("[!] Found Settings File.");
                CLIENTID = Convert.ToInt64(File.ReadAllText("Settings.dat"));
            }
            if(CLIENTID == 0)
            {
                Console.WriteLine("ClientID Was Not Valid.");
            }
            if (args.Length != 0)
            {
                if (args.Contains("--crash"))
                {
                    DiscordSDK.DiscordInitializeFail("User Initiated Crash", true);
                }
                else if (args.Contains("--v"))
                {
                    if (!IsRunning("Discord"))
                    {
                        DiscordSDK.DiscordInitializeFail(@"Fatal Error - Discord Not Running?", false);
                    }
                    else
                    {
                        if (DiscordSDK.ClassAccess)
                        {
                            DiscordSDK.InitializeDiscordRPC(true, Convert.ToInt64(CLIENTID));
                        }
                        else
                        {
                            DiscordSDK.DiscordInitializeFail(@"Fatal Error - Permissions Corrupt!", false);
                        }
                    }
                }
                else if (args.Contains("--setup"))
                {
                    FirstSetup.AskFile();
                }
                else {
                    Console.WriteLine(@"Invalid Arguments!");
                }
            }
            else
            {
                if (!IsRunning("Discord"))
                {
                    DiscordSDK.DiscordInitializeFail(@"Fatal Error - Discord Not Running?", false);
                }
                else
                {
                    if (DiscordSDK.ClassAccess)
                    {
                        DiscordSDK.InitializeDiscordRPC(false, Convert.ToInt64(CLIENTID));
                    }
                    else
                    {
                        DiscordSDK.DiscordInitializeFail(@"Fatal Error - Permissions Corrupt!", false);
                    }
                }
            }
            Thread.Sleep(1000);
        }
    }
}
