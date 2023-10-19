using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileService.Domain.Eneities
{
    public record class UploadedItem 
    {
        public DateTime CreateTime {  get;private set; }
        public long FileName { get; private set; }
        /// <summary>
        /// 文件的散列值，判断文件内容是否相同
        /// </summary>
        /// <summary>
        /// 文件大小
        /// </summary>
        public long FileSizeInBytes { get; private set; }
        /// <summary>
        /// 文件原始名
        /// </summary>       
        public string FileSHA256Hash { get; private set; }
        /// <summary>
        /// 文件备份地址
        /// </summary>
        public Uri BackUrl { get; private set; }
        /// <summary>
        /// 文件远程地址，供外部访问者访问的路径
        public Uri RemoteUrl { get; private set; }

        public static UploadedItem Create(DateTime createTime, long fileName, long fileSizeInBytes, string fileSHA256Hash, Uri backUrl, Uri remoteUrl)
        {
            UploadedItem item = new UploadedItem()
            {
                CreateTime = createTime,
                FileName = fileName,
                FileSizeInBytes = fileSizeInBytes,
                FileSHA256Hash = fileSHA256Hash,
                BackUrl = backUrl,
                RemoteUrl = remoteUrl
            };
            return item;        
        }
    }
}
