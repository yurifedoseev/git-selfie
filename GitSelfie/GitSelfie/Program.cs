using AForge.Video.DirectShow;

namespace GitSelfie
{
    class Program
    {
       static void Main(string[] args)
        {
            if (args.Length == 1 && args[0].ToLower() == "init")
            {
                PostCommitHook.Initialize();            
                return;
            }

            if (args.Length < 2)
            {
                return;
            }

            var commit = LoadCommitData(args);
            var command = new SelfieCommand(commit);
            command.Execute();
        }

        private static Commit LoadCommitData(string[] args)
        {
            var data = new Commit
            {
                Message = args[0],
                Sha1 = args[1]
            };

            if (data.Sha1.Length > 20)
            {
                data.Sha1 = data.Sha1.Substring(0, 20);
            }

            return data;
        }
    }
}
