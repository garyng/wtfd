// #load nuget:https://www.myget.org/F/cake-contrib/api/v2?package=Cake.Recipe&prerelease
#load "./cake/Cake.Recipe/Content/*.cake"

Environment.SetVariableNames();

BuildParameters.SetParameters(context: Context,
                            buildSystem: BuildSystem,
                            sourceDirectoryPath: "./src",
                            title: "wtfd",
                            repositoryOwner: "garyng",
                            repositoryName: "wtfd",
                            shouldRunDupFinder: false,
                            shouldRunInspectCode: false,
                            shouldRunGitVersion: true, 
                            packageSourceDatas: new List<PackageSourceData> {
                                new PackageSourceData(Context, "CHOCOLATEY", "https://push.chocolatey.org/", FeedType.Chocolatey, isRelease: false),
                                new PackageSourceData(Context, "CHOCOLATEY", "https://push.chocolatey.org/", FeedType.Chocolatey, isRelease: true)
                            });

BuildParameters.PrintParameters(Context);

ToolSettings.SetToolSettings(context: Context);

Build.RunDotNetCore();