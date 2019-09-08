#tool nuget:?package=GitVersion.CommandLine&version=5.0.1
#tool nuget:?package=NUnit.ConsoleRunner&version=3.10.0

///////////////////////////////////////////////////////////////////////////////
// ARGUMENTS
///////////////////////////////////////////////////////////////////////////////

var target = Argument("target", "Default");
var configuration = Argument("configuration", "Release");

///////////////////////////////////////////////////////////////////////////////
// SETUP / TEARDOWN
///////////////////////////////////////////////////////////////////////////////

var sourceDir = Directory("./Source");
var stageDir = Directory("./stage");
var buildDir = stageDir + Directory("build");
var packageDir = stageDir + Directory("package");
var publishDir = stageDir + Directory("publish");

var solution = sourceDir + File("CoroutinesForWpf.sln");

DirectoryPath msbuildInstallationPath;
FilePath msbuildExePath;
GitVersion version;

Setup(ctx =>
{
    // Executed BEFORE the first task.
    Information("Running tasks...");

    // workaround to not pick msbuild from VS2019 Preview
    msbuildInstallationPath = VSWhereLatest(new VSWhereLatestSettings { Requires = "Microsoft.Component.MSBuild" });
    msbuildExePath = msbuildInstallationPath.CombineWithFilePath("MSBuild/current/Bin/MSBuild.exe");

    version = GitVersion();
});

Teardown(ctx =>
{
    // Executed AFTER the last task.
    Information("Finished running tasks.");
});

///////////////////////////////////////////////////////////////////////////////
// TASKS
///////////////////////////////////////////////////////////////////////////////

Task("Restore").Does(()=>
{
    CleanDirectories("**/bin/" + configuration);
    CleanDirectories("**/obj" + configuration);

    EnsureDirectoryExists(stageDir);
    CleanDirectory(stageDir);
    EnsureDirectoryExists(buildDir);
    EnsureDirectoryExists(packageDir);
    EnsureDirectoryExists(publishDir);

    var settings = new NuGetRestoreSettings
    {
        PackagesDirectory = sourceDir + Directory("packages"),
        MSBuildPath = msbuildExePath.GetDirectory()
    };

    NuGetRestore(solution, settings);
});

Task("PreBuild").Does(() =>
{
    CreateAssemblyInfo(stageDir + File("AssemblyVersion.generated.cs"), new AssemblyInfoSettings {
        Version = version.MajorMinorPatch,
        FileVersion = version.MajorMinorPatch,
        InformationalVersion = version.InformationalVersion,
    });
});

Task("Build").Does(() =>
{
    var settings = new MSBuildSettings
    {
        ToolPath = msbuildExePath,
        Configuration = configuration,
    }
        .WithTarget("Rebuild");

    MSBuild(solution, settings);
});

Task("Test").Does(() =>
{
    NUnit3($"./source/**/bin/{configuration}/*.Tests.dll", new NUnit3Settings
    {
        //X86 = true,
        Results = new[]
        {
            new NUnit3Result
            {
                FileName = stageDir + File("TestResult.xml")
            }
        },
    });
});

Task("Package").Does(() =>
{
    var libDir = packageDir + Directory("lib/net40");

    EnsureDirectoryExists(libDir);
    CopyFileToDirectory(File("./nuspec/core.nuspec"), packageDir);

    CopyDirectory(sourceDir + Directory("CoroutinesForWpf/bin/" + configuration), libDir);

    var coreSettings = new NuGetPackSettings
    {
        Version = version.NuGetVersion,
        ProjectUrl = new Uri("https://github.com/dansav/coroutines-for-wpf"),
        License = new NuSpecLicense() { Type = "expression", Value = "MIT" },
        BasePath = packageDir,
        OutputDirectory = publishDir
    };
    NuGetPack(packageDir + File("core.nuspec"), coreSettings);
});

Task("Publish")
    .WithCriteria(() => (version.BranchName == "master" || version.BranchName.StartsWith("release/")) && !BuildSystem.IsLocalBuild && !BuildSystem.IsPullRequest)
    .Does(() =>
{
    if (string.IsNullOrWhiteSpace(EnvironmentVariable("NUGET_API_KEY")))
    {
        Warning($"Not Pushing the nuget package. Api key is missing!");
        return;
    }

    var feedUrl = "https://api.nuget.org/v3/index.json";
    var package = GetFiles($"{publishDir}/*{version.NuGetVersion}.nupkg").First();

    Information($"Pushing {package} to {feedUrl}");

    var settings = new NuGetPushSettings
    {
        Source = feedUrl,
        ApiKey = EnvironmentVariable("NUGET_API_KEY")
    };

    NuGetPush(package, settings);
});

Task("Default")
    .IsDependentOn("Restore")
    .IsDependentOn("PreBuild")
    .IsDependentOn("Build")
    .IsDependentOn("Test")
    .IsDependentOn("Package")
    .IsDependentOn("Publish")
    ;

RunTarget(target);