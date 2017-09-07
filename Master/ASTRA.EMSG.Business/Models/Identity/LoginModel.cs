using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace ASTRA.EMSG.Business.Models.Identity
{
    public class LoginModel
    {
        [Required(ErrorMessage = "„E-Mail“ darf nicht leer sein.")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required(ErrorMessage = "„Passwort“ darf nicht leer sein.")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [ScaffoldColumn(false)]
        [HiddenInput]
        public string ReturnUrl { get; set; }
    }
}
