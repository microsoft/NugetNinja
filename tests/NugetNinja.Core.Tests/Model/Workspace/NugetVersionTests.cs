// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.
#pragma warning disable CS1718 // Comparison made to same variable
#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
#pragma warning disable CS0253 // Possible unintended reference comparison; right hand side needs cast
#pragma warning disable CS8602 // Dereference of a possibly null reference.

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.NugetNinja.Core;

[TestClass]
public class NugetVersionTests
{
    [TestMethod]
    public void TestPointerEquals()
    {
        var version = new NugetVersion("10.1.0.0-Preview");
        Assert.IsTrue(version == version);
        Assert.IsTrue(version.Equals(version));
    }

    [TestMethod]
    public void TestNotNullEquals()
    {
        var version = new NugetVersion("10.1.0.0-Preview");
        Assert.IsTrue(version != null);
        Assert.IsTrue(!version.Equals(null));
        Assert.IsTrue(null != version);
    }

    [TestMethod]
    public void TestNullEquals()
    {
        NugetVersion version = null;
        Assert.IsTrue(version == null);
        Assert.IsTrue(null == version);
    }

    [TestMethod]
    public void TestNullLarger()
    {
        NugetVersion version = null;
        Assert.IsFalse(version > null);
        Assert.IsFalse(version < null);
        Assert.IsFalse(null < version);
        Assert.IsFalse(null > version);
    }

    [TestMethod]
    public void TestValueEquals()
    {
        var version1 = new NugetVersion("10.1.0.0-Preview");
        var version2 = new NugetVersion("10.1.0.0-preview");
        Assert.IsTrue(version1 == version2);
        Assert.IsTrue(version1.Equals(version2));
        Assert.IsTrue(version1.AdditionalText.Equals(version2.AdditionalText));
    }

    [TestMethod]
    public void TestOtherTypeEquals()
    {
        NugetVersion version1 = new NugetVersion("10.1.0.0-Preview");
        object version2 = new NugetVersion("10.1.0.0-preview");
        Assert.IsTrue(version1.Equals(version1 as object));
        Assert.IsFalse(version1 == version2);
        Assert.IsTrue(version1 == (NugetVersion)version2);
        Assert.IsTrue(version1.Equals(version2));
        Assert.IsFalse(version1.Equals(new int()));
    }

    [TestMethod]
    public void TestOtherTypeNullEquals()
    {
        NugetVersion version1 = new NugetVersion("10.1.0.0-Preview");
        object version2 = null;
        Assert.IsFalse(version1.Equals(version2));
    }

    [TestMethod]
    public void TestMinorValueLarger()
    {
        var version1 = new NugetVersion("10.1.999.0-Preview");
        var version2 = new NugetVersion("10.1.1000.0-preview");
        AssertLeftLarger(version2, version1);
    }

    [TestMethod]
    public void TestMajorValueLarger()
    {
        var version1 = new NugetVersion("10.1.999.0-Preview");
        var version2 = new NugetVersion("10.2.001.0-preview");
        AssertLeftLarger(version2, version1);
    }

    [TestMethod]
    public void TestProdMinorValueLarger()
    {
        var version1 = new NugetVersion("10.1.999.0");
        var version2 = new NugetVersion("10.1.1000.0");
        AssertLeftLarger(version2, version1);
    }

    [TestMethod]
    public void TestProdMajorValueLarger()
    {
        var version1 = new NugetVersion("10.1.999.0");
        var version2 = new NugetVersion("10.2.1.0");
        AssertLeftLarger(version2, version1);
    }

    [TestMethod]
    public void TestProdLargerPreview()
    {
        var version1 = new NugetVersion("2.2.10");
        var version2 = new NugetVersion("2.2.10-preview");
        AssertLeftLarger(version1, version2);
    }

    [TestMethod]
    public void TestClone()
    {
        var version1 = new NugetVersion("10.1.999.0");
        var version2 = version1.Clone() as NugetVersion;
        AssertEquals(version1, version2);
    }

    [TestMethod]
    public void TestIsPreviewVersion()
    {
        var version1 = new NugetVersion("10.1.999.0");
        var version2 = new NugetVersion("10.1.999.0-beta");
        Assert.IsFalse(version1.IsPreviewVersion());
        Assert.IsTrue(version2.IsPreviewVersion());
    }

    [TestMethod]
    public void TestToString()
    {
        var version = new NugetVersion("10000.200000.30000000.49999999-PrEView   ");
        Assert.AreEqual(version.ToString(), "10000.200000.30000000.49999999-preview");
    }

    [TestMethod]
    public void TestGetHashCodeEquals()
    {
        var version1 = new NugetVersion("10.1.999.0-beta");
        var version2 = new NugetVersion("10.1.999.0-beta");
        Assert.AreEqual(version1.GetHashCode(), version2.GetHashCode());
    }

    private void AssertEquals(NugetVersion? version1, NugetVersion? version2)
    {
        Assert.IsFalse(version1 < version2 || version2 < version1);
        Assert.IsFalse(version2 > version1 || version1 > version2);
        Assert.IsTrue(version1.Equals(version2));
        Assert.IsTrue(version2.Equals(version1));
        Assert.IsTrue(version1 == version1);
        Assert.IsTrue(version1 == version2);
        Assert.IsTrue(version2 == version1);
        Assert.IsTrue(version2 == version2);
        Assert.AreEqual(version1, version2);
        Assert.AreEqual(version2, version1);
        Assert.AreEqual(version1.GetHashCode(), version1.GetHashCode());
    }

    private void AssertLeftLarger(NugetVersion? big, NugetVersion? sml)
    {
        Assert.IsTrue(sml < big);
        Assert.IsTrue(big > sml);
        Assert.IsFalse(big < sml);
        Assert.IsFalse(sml > big);
        Assert.IsFalse(sml == big);
        Assert.IsFalse(big == sml);
        Assert.IsFalse(sml.Equals(big));
        Assert.IsFalse(big.Equals(sml));
        Assert.IsFalse(sml == big);
        Assert.IsFalse(big == sml);
        Assert.AreNotEqual(sml, big);
        Assert.AreNotEqual(big, sml);
        Assert.AreNotEqual(big.GetHashCode(), sml.GetHashCode());
    }
}
#pragma warning restore CS8602 // Dereference of a possibly null reference.
#pragma warning restore CS1718 // Comparison made to same variable
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.
#pragma warning restore CS0253 // Possible unintended reference comparison; right hand side needs cast
