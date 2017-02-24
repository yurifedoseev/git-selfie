using System;
using System.IO;
using GitSelfie.Domain.Interfaces;
using GitSelfie.Helpers;

namespace GitSelfie.Commands
{
    public class RemoveCommand : ICommand
    {
        public void Execute()
        {
            string[] directories = DirectoryFinder.FindGitDerictories();
            
            foreach (string directory in directories)
            {
                const string hookPath = @"hooks\post-commit";
                var path = Path.Combine(directory, hookPath);
                File.Delete(path);
                
                Console.Out.WriteLine("git-selfie was removed from " + directory);
            }
        }
    }
}