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

    public async Task RemovePackageReferenceAsync(string refName)
    {
        var csprojContent = await File.ReadAllTextAsync(PathOnDisk);
        var doc = new HtmlDocument();
        doc.OptionOutputOriginalCase = true;
        doc.LoadHtml(csprojContent);
        var node = doc.DocumentNode
            .Descendants("PackageReference")
            .Where(d => d.Attributes["Include"].Value == refName)
            .FirstOrDefault();

        if (node == null)
        {
            throw new InvalidOperationException($"Could remove PackageReference {refName} in project {this} because it was not found!");
        }

        await this.RemoveNode(node, doc, PathOnDisk);
        // to do: Remove in itself.
    }

    public async Task RemoveProjectReference(string refName)
    {
        var csprojContent = await File.ReadAllTextAsync(PathOnDisk);
        var contextPath = Path.GetDirectoryName(PathOnDisk) ?? throw new IOException($"Couldn't find the project path based on: '{PathOnDisk}'.");
        var doc = new HtmlDocument();
        doc.OptionOutputOriginalCase = true;
        doc.LoadHtml(csprojContent);
        var node = doc.DocumentNode
            .Descendants("ProjectReference")
            .Where(p => Path.Equals(refName, StringExtensions.GetAbsolutePath(contextPath, p.Attributes["Include"].Value)))
            .FirstOrDefault();

        if (node == null)
        {
            throw new InvalidOperationException($"Could remove PackageReference {refName} in project {this} because it was not found!");
        }

        await this.RemoveNode(node, doc, PathOnDisk);
        // to do: Remove in itself.
    }

    private async Task RemoveNode(HtmlNode node, HtmlDocument doc, string path)
    {
        node.Remove();
        var memoryStream = new MemoryStream();
        doc.Save(memoryStream);
        memoryStream.Seek(0, SeekOrigin.Begin);
        var csprojText = await new StreamReader(memoryStream).ReadToEndAsync();
        await File.WriteAllTextAsync(path, csprojText);
    }
}
