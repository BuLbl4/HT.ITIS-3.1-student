﻿using System.Reactive.Linq;
using Dotnet.Homeworks.Shared.Dto;
using Dotnet.Homeworks.Storage.API.Constants;
using Dotnet.Homeworks.Storage.API.Dto.Internal;
using Minio;

namespace Dotnet.Homeworks.Storage.API.Services;

public class ImageStorage : IStorage<Image>
{
    private readonly IMinioClient _client;
    private readonly string _bucketName;

    public ImageStorage(IMinioClient client, string bucketName)
    {
        _client = client;
        _bucketName = bucketName;
    }
    public async Task<Result> PutItemAsync(Image item, CancellationToken cancellationToken = default)
    {
        try
        {
            item.Metadata.Add(MetadataKeys.Destination, _bucketName);

            await _client.PutObjectAsync(new PutObjectArgs()
                .WithBucket(Buckets.Pending)
                .WithObject(item.FileName)
                .WithStreamData(item.Content)
                .WithObjectSize(item.Content.Length)
                .WithHeaders(item.Metadata)
                .WithContentType(item.ContentType), cancellationToken);

            return ResultFactory.CreateResult<Result>(true);
        }
        catch (Exception e)
        {
            return ResultFactory.CreateResult<Result>(false, error: e.Message);
        }
    }

    public async Task<Image?> GetItemAsync(string itemName, CancellationToken cancellationToken = default)
    {
        try
        {
            var content = new MemoryStream();
            var obj = await _client.GetObjectAsync(new GetObjectArgs()
                .WithBucket(_bucketName)
                .WithObject(itemName)
                .WithCallbackStream(async (stream, ct) =>
                {
                    await stream.CopyToAsync(content, ct);
                }), cancellationToken);

            return new Image(content, obj.ObjectName, obj.ContentType, obj.MetaData);
        }
        catch (Exception)
        {
            return null;
        }
    }

    public async Task<Result> RemoveItemAsync(string itemName, CancellationToken cancellationToken = default)
    {
        try
        {
            await _client.RemoveObjectAsync(new RemoveObjectArgs()
                .WithBucket(_bucketName)
                .WithObject(itemName),
                cancellationToken);

            return ResultFactory.CreateResult<Result>(true);
        }
        catch (Exception e)
        {
            return ResultFactory.CreateResult<Result>(false, error: e.Message);
        }
    }

    public async Task<IEnumerable<string>> EnumerateItemNamesAsync(CancellationToken cancellationToken = default)
    {
        var items = await _client.ListObjectsAsync(new ListObjectsArgs()
                    .WithBucket(_bucketName),
                cancellationToken)
            .Select(x => x.Key)
            .ToList();

        return items;
    }

    public async Task<Result> CopyItemToBucketAsync(string itemName, string destinationBucketName,
        CancellationToken cancellationToken = default)
    {
        try
        {
            await _client.CopyObjectAsync(new CopyObjectArgs()
                .WithCopyObjectSource(new CopySourceObjectArgs()
                    .WithBucket(_bucketName)
                    .WithObject(itemName))
                .WithBucket(destinationBucketName), cancellationToken);

            return ResultFactory.CreateResult<Result>(true);
        }
        catch (Exception ex)
        {
            return ResultFactory.CreateResult<Result>(false, error: ex.Message);
        }
    }
}