using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PFMS.DAL.Entities
{
    public class OneTimePassword
    {
        [Key]
        [Column("otpId")]
        public Guid OtpId { get; set; }

        [Column("otp")]
        public string Otp { get; set; }

        [Column("expires")]
        public DateTime Expires { get; set; }

        [ForeignKey("User")]
        [Column("userId")]
        public Guid UserId { get; set; }

        [Column("uniqueDeviceId")]
        public Guid UniqueDeviceId { get; set; }

        [Column("isVerified")]
        public bool IsVerified { get; set; }

        #region Navigation Properties
        public User? User { get; set; }
        #endregion
    }
}
