using System;
using System.IO;

namespace GitSelfie.Helpers
{
    public static class DirectoryFinder
    {
        public static string[] FindGitDerictories()
        {
            return Directory.GetDirectories(Environment.CurrentDirectory, ".git", SearchOption.AllDirectories);
        }
    }
}