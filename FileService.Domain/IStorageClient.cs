using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileService.Domain
{
    public interface IStorageClient
    {
        StorageType StorageType { get; }
        /// <summary>
        /// 保存文件
        /// </summary>
        /// <param name="key">文件的key（一般是文件路径的一部分）</param>
        /// <param name="content"></param>
        /// <param name="cancellationToken"></param>
        /// <returns>文件内容</returns>
        Task<Uri> SaveFileAsync(string key,Stream content,CancellationToken cancellationToken = default);
    }
}
