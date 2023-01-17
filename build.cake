#addin nuget:?package=Cake.Docker&version=1.2.0
#tool "dotnet:?package=GitVersion.Tool&version=5.11.1"

///////////////////////////////////////////////////////////////////////////////
// ARGUMENTS
///////////////////////////////////////////////////////////////////////////////

var target = Argument("target", "Default");
var configuration = Argument("configuration", "Release");

///////////////////////////////////////////////////////////////////////////////
// SETUP / TEARDOWN
///////////////////////////////////////////////////////////////////////////////

Setup(ctx =>
{
   // Executed BEFORE the first task.
   Information("Running tasks...");
});

Teardown(ctx =>
{
   // Executed AFTER the last task.
   Information("Finished running tasks.");
});

///////////////////////////////////////////////////////////////////////////////
// TASKS
///////////////////////////////////////////////////////////////////////////////
var projects = GetFiles("./**/*.csproj");
var projectPaths = projects.Select(project => project.GetDirectory().ToString());
var dockerFiles = GetFiles("./**/Dockerfile");
GitVersion gitVersion;
var artifactsDir = "./Artifacts";
var coverageThreshold = 60;

Task("Clean")
.Does(() => {
var settings = new DeleteDirectorySettings {
        Recursive = true,
        Force = true
    };
    // Clean solution directories.
    foreach(var path in projectPaths)
    {
        Information($"Cleaning path {path} ...");
        var directoriesToDelete = new DirectoryPath[]{
            Directory($"{path}/obj"),
            Directory($"{path}/bin")
        };
        foreach(var dir in directoriesToDelete)
        {
            if (DirectoryExists(dir))
            {
                DeleteDirectory(dir, settings);
            }
        }
    }
    // Delete artifact output too
    if (DirectoryExists(artifactsDir))
    {
        Information($"Cleaning path {artifactsDir} ...");
        DeleteDirectory(artifactsDir, settings);
    }
});

Task("Restore")
    .Description("Restores all the NuGet packages that are used by the specified solution.")
    .Does(() =>
{
    // Restore all NuGet packages.
    foreach(var path in projectPaths)
    {
        Information($"Restoring {path}...");
        DotNetRestore(path);
    }
});

Task("UpdateAssemblyInfo")
    .Does(() =>
{
    gitVersion = GitVersion(new GitVersionSettings {
        UpdateAssemblyInfo = true
    });
});

Task("Compile")
    .Description("Builds all the different parts of the project.")
    .IsDependentOn("Clean")
    .IsDependentOn("Restore")
    .IsDependentOn("UpdateAssemblyInfo")
.Does(() => {
  var buildSettings = new DotNetBuildSettings(){
     Configuration = "Release"
  };
  DotNetBuild("./EmojiVoto.sln", buildSettings);
});

Task("Test")
.Does(() => {
   var testSettings = new DotNetTestSettings(){
      Configuration = "Release",
      ArgumentCustomization = args => args.Append("/p:CollectCoverage=true")
                                             .Append("/p:CoverletOutputFormat=opencover")
                                             //.Append("/p:ThresholdType=line")
                                             //.Append($"/p:Threshold={coverageThreshold}")
   };
   DotNetTest("./EmojiVoto.sln", testSettings);
});

Task("Docker-Build")
 .IsDependentOn("UpdateAssemblyInfo")
.Does(() => {
  foreach(var file in dockerFiles)
  {
    var imageLookup = new Dictionary<string, string>(){
        {"EmojiUI","bravecobra/emoji-ui-svc" },
        {"EmojiSvc","bravecobra/emoji-svc" },
        {"EmojiVoting","bravecobra/emoji-voting-svc" },
        {"EmojiVoteBot","bravecobra/emoji-votebot" },
    };
    var directory = file.GetDirectory().Segments[file.GetDirectory().Segments.Length-1];
    var imageName = imageLookup[directory];
    var settings = new DockerImageBuildSettings {
      File = file.FullPath,
      BuildArg = new []{ $"SemVer={gitVersion.SemVer}" },
      Tag = new []{ $"{imageName}:{gitVersion.SemVer}" , $"{imageName}:latest"}
    };
    DockerBuild(settings, ".");
  }
});


Task("Default")
.IsDependentOn("UpdateAssemblyInfo")
.IsDependentOn("Compile")
.IsDependentOn("Test")
.IsDependentOn("Docker-Build")
;

RunTarget(target);