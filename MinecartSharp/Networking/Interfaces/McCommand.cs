using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MinecartSharp.Objects;

namespace MinecartSharp.Networking.Interfaces
{
    interface McCommand
    {
        string CommandName { get; }

        List<string> Aliases { get; }

        void OnExecute(Player player, CommandContext context);
    }
}
