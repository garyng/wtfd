#load nuget:https://www.myget.org/F/cake-contrib/api/v2?package=Cake.Recipe&prerelease

Environment.SetVariableNames();

BuildParameters.SetParameters(context: Context,
                            buildSystem: BuildSystem,
                            sourceDirectoryPath: "./src",
                            title: "wtfd",
                            repositoryOwner: "garyng",
                            repositoryName: "wtfd",
                            shouldRunGitVersion: true);

BuildParameters.PrintParameters(Context);

Information($"Branch name: {BuildParameters.BuildProvider.Repository.Branch}");

ToolSettings.SetToolSettings(context: Context);

Build.RunDotNetCore();

// todo: remove Cake.Issues.Recipe
// todo: remove dupfinder, inspectcode

// todo: update test report