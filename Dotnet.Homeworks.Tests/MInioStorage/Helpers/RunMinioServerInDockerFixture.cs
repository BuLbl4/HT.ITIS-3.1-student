using System.Diagnostics;
using System.Net.Sockets;
using Dotnet.Homeworks.Tests.RunLogic.Utils.Docker;
using MinioInstance = Dotnet.Homeworks.Tests.MinioStorage.Helpers.TestEnvironmentMinioInstance;

namespace Dotnet.Homeworks.Tests.MinioStorage.Helpers;

[CollectionDefinition(nameof(RunMinioServerInDockerFixture))]
public class RunMinioServerInDockerFixture : IDisposable, ICollectionFixture<RunMinioServerInDockerFixture>
{
    private readonly string _minioContainerName;
    
    public RunMinioServerInDockerFixture()
    {
        _minioContainerName = Guid.NewGuid().ToString();
        if (!IsPortAvailable(MinioInstance.Config.Port))
            throw new PortAlreadyAllocatedException(MinioInstance.Config.Port);
        RunMinioContainer(_minioContainerName);
        Thread.Sleep(TimeSpan.FromSeconds(1)); // waiting for minio container to fully set up
    }

    private static bool IsPortAvailable(int port)
    {
        try
        {
            using var client = new TcpClient();
            client.Connect(MinioInstance.Config.Endpoint, port);
            return false;
        }
        catch (SocketException)
        {
            return true;
        }
    }

    private static void RunMinioContainer(string containerName) =>
        RunProcess("docker", $"run -d -p {MinioInstance.Config.Port}:9000 --name {containerName} " +
                             $"-e {Constants.MinioRootUserEnvVar}={MinioInstance.Config.Username} " +
                             $"-e {Constants.MinioRootPassEnvVar}={MinioInstance.Config.Password} " +
                             $"minio/minio server /data");
    
    private static void StopMinioContainer(string containerName) =>
        RunProcess("docker", $"stop {containerName}");

    private static void DeleteMinioContainer(string containerName) =>
        RunProcess("docker", $"rm {containerName}");

    private static void RunProcess(string command, string arguments)
    {
        using var process = new Process();

        process.StartInfo.FileName = command;
        process.StartInfo.Arguments = arguments;

        process.Start();
        process.WaitForExit();
        process.Close();
    }
    
    public void Dispose()
    {
        StopMinioContainer(_minioContainerName);
        DeleteMinioContainer(_minioContainerName);
        GC.SuppressFinalize(this);
    }
}