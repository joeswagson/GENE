global using static GENE.CLI.Program;
using GENE.Clusters;

public static class Global
{
    public static Dictionary<string, NodeCluster> Clusters { get; } = [];
}