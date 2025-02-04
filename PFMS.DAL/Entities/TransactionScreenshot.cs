﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PFMS.Utils.Interfaces;

namespace PFMS.DAL.Entities
{
    public class TransactionScreenshot: IIdentifiable
    {
        [Key]
        [Column("screenshotId")]
        public Guid Id { get; set; }

        [Column("fileName")]
        public string FileName { get; set; }

        [Column("fileExtension")]
        public string FileExtension { get; set; }

        [Column("fileSizeInBytes")]
        public long FileSizeInBytes { get; set; }

        [Column("filePath")]
        public string FilePath { get; set; }

        [ForeignKey(nameof(Transaction))]
        [Column("transactionId")]
        public Guid TransactionId { get; set; }
        
        #region Navigation Properties
        public Transaction? Transaction { get; set; }
        #endregion
    }
}
