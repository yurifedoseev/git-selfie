using System;
using System.IO;
using System.Text;
using GitSelfie.Domain.Interfaces;
using GitSelfie.Helpers;

namespace GitSelfie.Commands
{
    public class InitCommand : ICommand
    {
        public void Execute()
        {
            string[] directories = DirectoryFinder.FindGitDerictories();
            
            foreach (string directory in directories)
            {
                WritePostCommitHook(directory);
                Console.Out.WriteLine("git-selfie initialized in " + directory);
            }
        }

        private void WritePostCommitHook(string directoryPath)
        {
            const string hookPath = @"hooks\post-commit";
            var path = Path.Combine(directoryPath, hookPath);
            
            var sb = new StringBuilder();
            sb.AppendLine(@"#!/bin/sh");
            sb.AppendLine("MESSAGE=$(git log -1 HEAD --pretty=format:%s)");
            sb.AppendLine("SHA=$(git rev-list -1 HEAD)");
            sb.AppendLine("exec \"gitselfie\" \"$MESSAGE\" \"$SHA\"");

            File.WriteAllText(path, sb.ToString());
        }
    }
}