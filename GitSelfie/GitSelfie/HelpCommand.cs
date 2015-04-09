using System;

namespace GitSelfie
{
    public class HelpCommand : ICommand
    {
        public void Execute()
        {
            Console.Out.WriteLine("Git Selfe - memorize your commit");
            Console.Out.WriteLine("-----");
            Console.Out.WriteLine("Commands:");
            Console.Out.WriteLine("gitselfie init                                 Initialized git-selfie post-commit hook for each git-repository in subdirectories");
            Console.Out.WriteLine("gitselfie rm                                   Remove git-selfie post-commit hook for every each-repository in subdirectories");
            Console.Out.WriteLine("gitselfie \"commit message\" \"commit SHA1\"       Take a snapshot and save it in " + Environment.SpecialFolder.MyPictures + " directory");
        }
    }
}