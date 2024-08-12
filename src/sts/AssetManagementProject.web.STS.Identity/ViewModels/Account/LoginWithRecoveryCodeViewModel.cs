using System.ComponentModel.DataAnnotations;

namespace AssetManagementProject.web.STS.Identity.ViewModels.Account
{
    public class LoginWithRecoveryCodeViewModel
    {
        [Required]
        [DataType(DataType.Text)]
        public string RecoveryCode { get; set; }

        public string ReturnUrl { get; set; }
    }
}








