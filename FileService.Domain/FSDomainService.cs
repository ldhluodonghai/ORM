using FileService.Domain.Eneities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileService.Domain
{
    public class FSDomainService
    {
        private readonly IFSRepository repository;
        private readonly IStorageClient backupStorage;
        private readonly IStorageClient remoteStorage;

        public FSDomainService(IFSRepository repository, IEnumerable<IStorageClient> storageClients)
        {
            this.repository = repository;
            this.backupStorage = storageClients.First(c=>c.StorageType==StorageType.Backup);
            this.remoteStorage = storageClients.First(c => c.StorageType == StorageType.Public);
        }

        public async Task<UploadedItem> UpLoadAsync(Stream stream,string filename,CancellationToken cancellationToken)
        {
            string hash = 
        }
    }
}
