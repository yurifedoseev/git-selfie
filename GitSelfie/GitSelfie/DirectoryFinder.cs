using System;
using System.IO;

namespace GitSelfie
{
    public class DirectoryFinder
    {
        public static string[] FindGitDerictories()
        {
            return Directory.GetDirectories(Environment.CurrentDirectory, ".git", SearchOption.AllDirectories);
        }
    }
}