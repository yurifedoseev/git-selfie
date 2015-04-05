using System;
using System.IO;
using System.Text;

namespace GitSelfie
{
    public class InitCommand : ICommand
    {
        public void Execute()
        {
            Console.Out.WriteLine("Initialize git-selfie for current repository");

            if (Directory.Exists(".git"))
            {
                WritePostCommitHook(@".git\hooks\post-commit");
                Console.Out.WriteLine("git-selfie initialized");
            }
            else
            {
                Console.Out.WriteLine("Could not initialize git-selfie because .git folder was not found");
            }
        }

        private void WritePostCommitHook(string path)
        {
            var sb = new StringBuilder();
            sb.AppendLine(@"#!/bin/sh");
            sb.AppendLine("MESSAGE=$(git log -1 HEAD --pretty=format:%s)");
            sb.AppendLine("SHA=$(git rev-list -1 HEAD)");
            sb.AppendLine("exec \"gitselfie\" \"$MESSAGE\" \"$SHA\"");

            File.WriteAllText(path, sb.ToString());
        }
    }
}