namespace bgTeam.StoryRunner.Example
{
    class Program
    {
        readonly static Program<Startup> _program = new Program<Startup>();

        static void Main(string[] args)
        {
            _program.Init(args);
            _program.Run();
        }
    }
}
