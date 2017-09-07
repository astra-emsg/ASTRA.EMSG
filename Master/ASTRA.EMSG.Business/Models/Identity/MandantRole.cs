using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASTRA.EMSG.Business.Models.Identity
{
    [Table("AspMandantRole")]
    public class MandantRole
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string MandantName { get; set; }
        [Required]
        public string RoleName { get; set; }

        [Required]
        public string UserId { get; set; }
        [ForeignKey("UserId")]
        public virtual ApplicationUser User { get; set; }
    }
}
