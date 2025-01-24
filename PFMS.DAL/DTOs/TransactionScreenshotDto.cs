using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PFMS.Utils.Interfaces;

namespace PFMS.DAL.DTOs
{
    public class TransactionScreenshotDto: IIdentifiable
    {
        public Guid Id { get; set; }
        public string FileName { get; set; }
        public string FileExtension { get; set; }
        public long FileSizeInBtes { get; set; }
        public string FilePath { get; set; }
        public Guid TransactionId { get; set; }
    }
}
