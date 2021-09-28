using System;

/*
WARNING! This app will only work for x86-64 systems. In layman's terms, 64-bit only.
(you can change it to be x86-32, but you will need to understand the discord SDK then.)
When running the app, make sure discord_game_sdk.dll is in the same directory as the file.
Don't worry! unless you tampered with some wierd settings, the dll will be automatically
copied to the output direcrory. (assuming you have it in the same directory as the .cs file) */

namespace CsharpProject
{
    public class DiscordSDK
    {
        public static bool ClassAccess = true;
        public static void InitializeDiscordRPC(bool v, long ClientIdentifier)
        {

            string StatusState = @"Do My Math Homework Please...";
            string StatusDetails = @"x + x = y | y = z - 1 | x < y | z = x + 1";
            bool StatusInstance = true;

            var clientID = Environment.GetEnvironmentVariable("DISCORD_CLIENT_ID");
            if (clientID == null)
            {
                clientID = ClientIdentifier.ToString();
            }
            bool ActivityError = false;
            var discord = new Discord.Discord(Int64.Parse(clientID), (UInt64)Discord.CreateFlags.NoRequireDiscord);
            var activityManager = discord.GetActivityManager();
            var relationshipManager = discord.GetRelationshipManager();
            var imageManager = discord.GetImageManager();
            var userManager = discord.GetUserManager();
            if (v)
            {
                int ModulesLoaded = 0;
                if (discord != null)
                {
                    Console.WriteLine(@"[!] Discord Initialized!");
                    ModulesLoaded++;
                }
                if (activityManager != null)
                {
                    Console.WriteLine(@"[!] ActivityManager Initialized!");
                    ModulesLoaded++;
                }
                if (relationshipManager != null)
                {
                    Console.WriteLine(@"[!] RelationshipManaget Initialized!");
                    ModulesLoaded++;
                }
                if (imageManager != null)
                {
                    Console.WriteLine(@"[!] imageManager Initialized!");
                    ModulesLoaded++;
                }
                if (userManager != null)
                {
                    Console.WriteLine(@"[!] userManager Initialized!");
                    ModulesLoaded++;
                }
                if (ModulesLoaded < 5)
                {
                    Console.WriteLine(@"[! ! !] One or more critical components could not load!!");
                }
            }
            Console.WriteLine(@"" + @"!! Press Up-Arrow To Close Presence." + "\n\n");
            Console.WriteLine("Initializing Discord Game SDK RPC...\n");
            userManager.GetUser(Int64.Parse(clientID), (Discord.Result result, ref Discord.User otherUser) =>
            {
                if (result == Discord.Result.Ok)
                {
                    if(v)
                    {
                        Console.WriteLine("OtherUser: " + otherUser.Username + @"#" + otherUser.Discriminator);
                        Console.WriteLine("OtherUser ID: " + otherUser.Id + "\n");
                    }
                }
            });
            userManager.OnCurrentUserUpdate += () =>
            {
                var currentUser = userManager.GetCurrentUser();
                Console.WriteLine(@"// User Details Are Below!! //" + "\n");
                Console.WriteLine("Discord User: " + currentUser.Username + "#" + currentUser.Discriminator);
                Console.WriteLine("Discord User ID: " + currentUser.Id + "\n");
                Console.WriteLine(@"// User Details End Here!! //" + "\n");
                if (currentUser.Username != null)
                {
                    Console.Title = "RealFX Discord RPC | " + currentUser.Username + @"#" + currentUser.Discriminator;
                }
                if (v)
                {
                    Console.Title = "RealFX Discord RPC | " + currentUser.Username + @"#" + currentUser.Discriminator + @" | Verbose Enabled.";
                }
            };
            var activity = new Discord.Activity
            {
                State = StatusState,
                Details = StatusDetails,
                Instance = StatusInstance,
            };
            activityManager.UpdateActivity(activity, (result) =>
            {
                if (result == Discord.Result.Ok)
                {
                    Console.WriteLine("Success Setting Activity!\n");
                }
                else
                {
                    Console.WriteLine("Failed To Set Activity.");
                    ActivityError = true;
                }
            });
            if (v)
            {
                Console.WriteLine("State: " + StatusState);
                Console.WriteLine("Details: " + StatusDetails);
                Console.WriteLine("Instance: " + StatusInstance.ToString() + "\n");
            }
            if (ActivityError != true)
            {
                while (true)
                {
                    discord.RunCallbacks();
                    System.Threading.Thread.Sleep(100);
                    if (Console.KeyAvailable)
                        if (Console.ReadKey(true).Key == ConsoleKey.UpArrow)
                        {
                            break;
                        }
                }
                activityManager.ClearActivity((result) =>
                {
                    if (result == Discord.Result.Ok)
                    {
                        Console.WriteLine("\n" + "Successfully Cleared Activity!");
                    }
                    else
                    {
                        Console.WriteLine("\n" + "Failed Clearing Activity.");
                    }
                });
                discord.Dispose();
            }
            else
            {
                Console.WriteLine("Discord SDK could not set your activity.");
                if (Console.KeyAvailable)
                    if (Console.ReadKey(true).Key == ConsoleKey.UpArrow)
                    {
                        discord.Dispose();
                        return;
                    }
            }
        }
        public static void DiscordInitializeFail(string ex, bool userinitiated)
        {
            if (userinitiated)
            {
                Console.Title = "RealFX Discord RPC | User Initiated Crash.";
            }
            else {
                Console.Title = "RealFX Discord RPC | Fatal Error!"; }
            Console.WriteLine(@"! Discord RPC Could Not Be Initialized !");
            Console.WriteLine("");
            if (ex != null)
            {
                Console.WriteLine(ex);
            }
            else
            {
                Console.WriteLine(@"Could Not Find 'ex' !!");
            }
            Console.WriteLine("");
            Console.WriteLine(@"!! Press Up-Arrow To Exit.");
            while (true)
            {
                System.Threading.Thread.Sleep(100);
                if (Console.KeyAvailable)
                {
                    if (Console.ReadKey(true).Key == ConsoleKey.UpArrow)
                    {
                        return;
                    }
                }
            }
        }
    }
}
