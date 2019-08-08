using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lelebot.Commands
{
    public class DownloadYoutube : Command
    {
        public override bool Match(Context context)
        {
            if (context.Command == "get")
            {
                if (context.Args.Length == 1)
                {
                    if (context.Args[0].Contains("youtu"))
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        public override async void Run()
        {

        }
    }
}
