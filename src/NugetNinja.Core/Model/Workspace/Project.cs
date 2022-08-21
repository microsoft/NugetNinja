// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using HtmlAgilityPack;

namespace Microsoft.NugetNinja.Core;

public class Project
{
    public Project(string pathOnDisk)
    {
        PathOnDisk = pathOnDisk;
    }

    public string PathOnDisk { get; set; }

    #region Framework
    public string? Sdk { get; set; }
    public string? OutputType { get; set; }
    public string? TargetFramework { get; set; }
    #endregion

    #region Features
    public string? Nullable { get; set; }
    public string? ImplicitUsings { get; set; }
    #endregion

    #region Nuget Packaging
    public string? Description { get; set; }
    public string? Version { get; set; }
    public string? Company { get; set; }
    public string? Product { get; set; }
    public string? Authors { get; set; }
    public string? PackageTags { get; set; }
    public string? PackageProjectUrl { get; set; }
    public string? RepositoryUrl { get; set; }
    public string? RepositoryType { get; set; }
    #endregion

    public List<Project> ProjectReferences { get; init; } = new();

    public List<Package> PackageReferences { get; init; } = new();

    public override string ToString()
    {
        return Path.GetFileNameWithoutExtension(PathOnDisk);
    }

    public async Task SetPackageReferenceVersionAsync(string refName, NugetVersion newVersion)
    {
        var csprojContent = await File.ReadAllTextAsync(PathOnDisk);
        var doc = new HtmlDocument();
        doc.OptionOutputOriginalCase = true;
        doc.OptionAutoCloseOnEnd = true;
        doc.OptionWriteEmptyNodes = true;
        doc.LoadHtml(csprojContent);
        var node = doc.DocumentNode
            .Descendants("PackageReference")
            .FirstOrDefault(d => d.Attributes["Include"].Value == refName);

        if (node == null)
        {
            throw new InvalidOperationException($"Could remove PackageReference {refName} in project {this} because it was not found!");
        }

        node.Attributes["Version"].Value = newVersion.ToString();

        await SaveDocToDisk(doc);
    }

    public async Task RemovePackageReferenceAsync(string refName)
    {
        var csprojContent = await File.ReadAllTextAsync(PathOnDisk);
        var doc = new HtmlDocument();
        doc.OptionOutputOriginalCase = true;
        doc.OptionAutoCloseOnEnd = true;
        doc.OptionWriteEmptyNodes = true;
        doc.LoadHtml(csprojContent);
        var node = doc.DocumentNode
            .Descendants("PackageReference")
            .FirstOrDefault(d => d.Attributes["Include"].Value == refName);

        if (node == null)
        {
            throw new InvalidOperationException($"Could remove PackageReference {refName} in project {this} because it was not found!");
        }

        await RemoveNode(node, doc);
    }

    public async Task RemoveProjectReference(string absPath)
    {
        var csprojContent = await File.ReadAllTextAsync(PathOnDisk);
        var contextPath = Path.GetDirectoryName(PathOnDisk) ?? throw new IOException($"Couldn't find the project path based on: '{PathOnDisk}'.");
        var doc = new HtmlDocument();
        doc.OptionOutputOriginalCase = true;
        doc.LoadHtml(csprojContent);
        var node = doc.DocumentNode
            .Descendants("ProjectReference")
            .FirstOrDefault(p => Equals(absPath, StringExtensions.GetAbsolutePath(contextPath, p.Attributes["Include"].Value)));

        if (node == null)
        {
            throw new InvalidOperationException($"Could remove PackageReference {absPath} in project {this} because it was not found!");
        }

        await RemoveNode(node, doc);
    }

    private async Task RemoveNode(HtmlNode node, HtmlDocument doc)
    {
        var parent = node.ParentNode;
        if (!parent.Descendants(0).Where(n => n.NodeType == HtmlNodeType.Element).Except(new[] { node }).Any())
        {
            parent.Remove();
        }
        else
        {
            node.Remove();
        }

        await SaveDocToDisk(doc);
    }

    private async Task SaveDocToDisk(HtmlDocument doc)
    {
        var memoryStream = new MemoryStream();
        doc.Save(memoryStream);
        memoryStream.Seek(0, SeekOrigin.Begin);
        var csprojText = await new StreamReader(memoryStream).ReadToEndAsync();
        csprojText = csprojText
            .Replace(@"></PackageReference>", " />")
            .Replace(@"></ProjectReference>", " />")
            .Replace(@"></FrameworkReference>", " />")
            .Replace(@"></Using>", " />");
        await File.WriteAllTextAsync(PathOnDisk, csprojText);
    }
}
