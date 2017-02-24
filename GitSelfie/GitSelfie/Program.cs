using GitSelfie.Commands;

namespace GitSelfie
{
    internal static class Program
    {
        private static void Main(string[] args)
        {
            var command = CommandFactory.Create(args);
            command?.Execute();
        }
    }
}
