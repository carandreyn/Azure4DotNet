﻿using Azure.Storage.Blobs.Models;
using Azure.Storage.Blobs;

namespace AzureBlobProject.Services
{
	public class ContainerService : IContainerService
	{
		private readonly BlobServiceClient _blobClient;

		public ContainerService(BlobServiceClient blobClient)
		{
			_blobClient = blobClient;
		}

		public async Task CreateContainer(string containerName)
		{
			BlobContainerClient blobContainerClient = _blobClient.GetBlobContainerClient(containerName);
			await blobContainerClient.CreateIfNotExistsAsync(PublicAccessType.BlobContainer);

		}

		public async Task DeleteContainer(string containerName)
		{
			BlobContainerClient blobContainerClient = _blobClient.GetBlobContainerClient(containerName);
			await blobContainerClient.DeleteIfExistsAsync();
		}

		public async Task<List<string>> GetAllContainers()
		{
			List<string> containerName = new();

			await foreach (BlobContainerItem blobkContainerItem in _blobClient.GetBlobContainersAsync())
			{
				containerName.Add(blobkContainerItem.Name);
			}

			return containerName;
		}

		public async Task<List<string>> GetAllContainersAndBlobs()
		{
			List<string> containerAndBlobNames = new();
			containerAndBlobNames.Add("Account Name : " + _blobClient.AccountName);
			containerAndBlobNames.Add("------------------------------------------------------------------------------------------------------------");
			await foreach (BlobContainerItem blobContainerItem in _blobClient.GetBlobContainersAsync())
			{
				containerAndBlobNames.Add("--" + blobContainerItem.Name);
				BlobContainerClient _blobContainer =
					  _blobClient.GetBlobContainerClient(blobContainerItem.Name);
				await foreach (BlobItem blobItem in _blobContainer.GetBlobsAsync())
				{
					containerAndBlobNames.Add("------" + blobItem.Name);
				}
				containerAndBlobNames.Add("------------------------------------------------------------------------------------------------------------");

			}
			return containerAndBlobNames;
		}
	}
}