namespace Sistema.Web.ViewModels;
using System.ComponentModel.DataAnnotations;

public class LoginViewModel
{
    [Display(Name = "Usuario")]
    [Required(ErrorMessage = "El usuario o contraseña es obligatorio")]
    public string User { get; set; }
    [Display(Name = "Contraseña")]
    [Required(ErrorMessage = "El usuario o contraseña es obligatorio")]
    public string Pass { get; set; }
    public string ErrorMessage { get; internal set; }
}