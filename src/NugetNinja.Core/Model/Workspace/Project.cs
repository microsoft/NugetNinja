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

    public List<Project> ProjectReferences { get; init; } = new List<Project>();

    public List<Package> PackageReferences { get; init; } = new List<Package>();

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
            .Where(d => d.Attributes["Include"].Value == refName)
            .FirstOrDefault();

        if (node == null)
        {
            throw new InvalidOperationException($"Could remove PackageReference {refName} in project {this} because it was not found!");
        }

        node.Attributes["Version"].Value = newVersion.ToString();

        await this.SaveDocToDisk(doc);
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
            .Where(d => d.Attributes["Include"].Value == refName)
            .FirstOrDefault();

        if (node == null)
        {
            throw new InvalidOperationException($"Could remove PackageReference {refName} in project {this} because it was not found!");
        }

        await this.RemoveNode(node, doc);
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
            .Where(p => Equals(absPath, StringExtensions.GetAbsolutePath(contextPath, p.Attributes["Include"].Value)))
            .FirstOrDefault();

        if (node == null)
        {
            throw new InvalidOperationException($"Could remove PackageReference {absPath} in project {this} because it was not found!");
        }

        await this.RemoveNode(node, doc);
    }

    private async Task RemoveNode(HtmlNode node, HtmlDocument doc)
    {
        var parent = node.ParentNode;
        if (!parent.Descendants(0).Where(n => n.NodeType == HtmlNodeType.Element).Except(new HtmlNode[] { node }).Any())
        {
            parent.Remove();
        }
        else
        {
            node.Remove();
        }

        await this.SaveDocToDisk(doc);
    }

    private async Task SaveDocToDisk(HtmlDocument doc)
    {
        var memoryStream = new MemoryStream();
        doc.Save(memoryStream);
        memoryStream.Seek(0, SeekOrigin.Begin);
        var csprojText = await new StreamReader(memoryStream).ReadToEndAsync();
        csprojText = csprojText
            .Replace(@"></PackageReference>", "/>")
            .Replace(@"></ProjectReference>", "/>");
        await File.WriteAllTextAsync(this.PathOnDisk, csprojText);
    }
}
