using System;
using System.Collections.Generic;

class BTreeNode
{
    public string Key { get; set; } // Name of the file/directory
    public List<BTreeNode> Children { get; set; } // List of child nodes
    public bool IsLeaf { get; set; } // Indicates if this is a leaf node

    public BTreeNode(string key, bool isLeaf)
    {
        Key = key;
        Children = new List<BTreeNode>();
        IsLeaf = isLeaf;
    }
}

class FileSystem
{
    public BTreeNode Root { get; private set; }
    public int T { get; private set; } // Minimum degree

    public FileSystem(int t)
    {
        Root = new BTreeNode("C:", true);
        T = t;
    }

    public void Add(string path)
    {
        // Normalizes the path to use '/' as separator
        var normalizedPath = path.Replace('\\', '/');
        var parts = normalizedPath.Split('/');
        AddRecursive(Root, parts, 0);
    }

    private void AddRecursive(BTreeNode node, string[] parts, int index)
    {
        if (index >= parts.Length)
        {
            return;
        }

        var key = parts[index];
        var child = node.Children.Find(c => c.Key == key);
        if (child == null)
        {
            child = new BTreeNode(key, index == parts.Length - 1);
            node.Children.Add(child);
            node.Children.Sort((a, b) => string.Compare(a.Key, b.Key, StringComparison.Ordinal));
        }

        AddRecursive(child, parts, index + 1);
    }

    public void DepthFirstSearch()
    {
        if (Root == null)
        {
            return;
        }

        DFSRecursive(Root, 0);
    }

    private void DFSRecursive(BTreeNode node, int depth)
    {
        Console.WriteLine(new string(' ', depth * 2) + node.Key);
        foreach (var child in node.Children)
        {
            DFSRecursive(child, depth + 1);
        }
    }

    public bool SearchFile(string fileName)
    {
        return SearchFileRecursive(Root, fileName, new List<string>());
    }

    private bool SearchFileRecursive(BTreeNode node, string fileName, List<string> path)
    {
        path.Add(node.Key);
        if (node.Key == fileName)
        {
            Console.WriteLine("File found at path: " + string.Join("/", path));
            return true;
        }

        foreach (var child in node.Children)
        {
            if (SearchFileRecursive(child, fileName, new List<string>(path)))
            {
                return true;
            }
        }

        return false;
    }
}

class Program
{
    static void Main(string[] args)
    {
        // Creates a file system with minimum degree 2
        FileSystem fileSystem = new FileSystem(2);

        // Adds directories and files
        fileSystem.Add("Users/fhulufhelo/Documents");
        fileSystem.Add("Users/fhulufhelo/Downloads");
        fileSystem.Add("Users/dpduser/Music");
        fileSystem.Add("Users/dpduser/Pictures");
        fileSystem.Add("Windows/assembly/GAC_32");
        fileSystem.Add("Windows/assembly/GAC_64");
        fileSystem.Add("Windows/Boot/DVD");
        fileSystem.Add("Windows/Boot/EFI");
        fileSystem.Add("Program Files/Microsoft Office/Office16");
        fileSystem.Add("Program Files/Microsoft Office/Stationery");
        fileSystem.Add("Program Files/Git/dev");
        fileSystem.Add("Program Files/Git/tmp");

        // Performs DFS on the file system
        fileSystem.DepthFirstSearch();

        // Searches for a specific file (pictures folder)
        string fileName = "Pictures";
        if (!fileSystem.SearchFile(fileName))
        {
            Console.WriteLine("File not found: " + fileName);
        }
    }
}
 