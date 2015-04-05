namespace GitSelfie
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var command = CommandFactory.Create(args);
            if (command != null)
                command.Execute();
        }
    }
}
