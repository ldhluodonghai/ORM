using FileService.Domain.Eneities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileService.Domain
{
    public interface IFSRepository
    {
        /// <summary>
        /// 查找文件上传文件相同大小的散列值文件记录
        /// </summary>
        /// <param name="fileSize"></param>
        /// <param name="fileSHA256Hash"></param>
        /// <returns></returns>
        Task<UploadedItem?> FindFileOrSame(long fileSize,string fileSHA256Hash);
    }
}
