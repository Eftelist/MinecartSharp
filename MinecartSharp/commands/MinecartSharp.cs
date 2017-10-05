
using System.Collections.Generic;
using System.Reflection;
using MinecartSharp.Networking.Interfaces;
using MinecartSharp.Objects;

namespace MinecartSharp.commands
{
    class MinecartSharp : McCommand
    {
        public string CommandName { get; } = "minecartsharp";

        public List<string> Aliases { get; }

        public void OnExecute(Player player, CommandContext context)
        {
            context.SendMessage("This server is running MinecartSharp version " + Assembly.GetExecutingAssembly().GetName().Version);
        }
    }
}
