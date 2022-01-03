namespace Tada
{
    interface ITool
    {
        public string Title { get; set; }
        public string Version { get; set; }
        public bool Help { get; set; }
        public string Project { get; set; }
        public string Command { get; set; }
        public string Name { get; set; }
        public string[] Arguments { get; set; }

        public void Run();
        public void ShowHelp();
        public string ToCode();
    }
}
