using Azure.Storage.Blobs;

namespace GameStore.Api.Shared.Cdn;

public class CdnUrlTransformer(
    IConfiguration configuration,
    BlobServiceClient blobServiceClient)
{
    public string TransformToCdnUrl(string storageUrl)
    {
        ArgumentException.ThrowIfNullOrEmpty(storageUrl);

        var frontDoorHost = configuration["AZURE_FRONTDOOR_HOSTNAME"];

        if (string.IsNullOrEmpty(frontDoorHost) ||
            !Uri.TryCreate(storageUrl, UriKind.Absolute, out var uri))
        {
            return storageUrl;
        }

        var storageHost = blobServiceClient.Uri.Host;

        if (!string.Equals(uri.Host, storageHost,
                StringComparison.OrdinalIgnoreCase))
        {
            return storageUrl;
        }

        var uriBuilder = new UriBuilder(uri)
        {
            Host = frontDoorHost
        };

        return uriBuilder.Uri.ToString();
    }
}