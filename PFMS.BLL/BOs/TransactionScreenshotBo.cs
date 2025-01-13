using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PFMS.BLL.BOs
{
    public class TransactionScreenshotBo
    {
        public Guid ScreenshotId { get; set; }
        public string FileName { get; set; }
        public string FileExtension { get; set; }
        public long FileSizeInBytes { get; set; }
        public string FilePath { get; set; }
        public Guid TransactionId { get; set; }
    }
}
