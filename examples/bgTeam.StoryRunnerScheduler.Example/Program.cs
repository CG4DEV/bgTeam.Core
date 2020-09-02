namespace bgTeam.StoryRunnerScheduler.Example
{
    class Program
    {
        readonly static Program<Startup> program = new Program<Startup>();

        static void Main(string[] args)
        {
            program.Init(args);
            program.Run();
        }
    }
}
