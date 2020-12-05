namespace Lelebot
{
    public struct Call
    {
        public string BaseCommand { get; }
        public string[] Args { get; }

        public Call(string baseCommand, params string[] args)
        {
            BaseCommand = baseCommand;
            Args = args ?? new string[] { };
        }
    }
}