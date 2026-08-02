## 2025-02-28 - Dependency and Package Modernization

**Observation:**
1. `Tedd.HashSetExtensions.Tests.csproj` dependencies are outdated (`Microsoft.NET.Test.Sdk` at 16.2.0, `xunit` at 2.4.0, etc.).
2. CI pipeline `azure-pipelines.yml` does not explicitly manage SDK versions for multi-targeting, needing `.NET 10.0.x` and `8.0.x` to reliably test targets.
3. `Tedd.HashSetExtensions.csproj` is missing a `PackageReadmeFile` and its `Version` is still `1.0.1`.

**Strategic Action:**
1. Updated `Tedd.HashSetExtensions.Tests.csproj` dependencies using latest available correct versions.
2. Updated `azure-pipelines.yml` to use `UseDotNet@2` for 10.0.x (with includePreviewVersions: true) and 8.0.x.
3. Updated `Tedd.HashSetExtensions.csproj` to include `PackageReadmeFile`, added `<None Include="../../README.md" Pack="True" PackagePath="" />`. Bumped `Version` to `1.0.2`.
