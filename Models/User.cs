using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

public class User : IdentityUser
{
    public int Id { get; set; } 

    [Required(ErrorMessage = "Имя обязательно для заполнения.")]
    [StringLength(50, ErrorMessage = "Имя не должно превышать 50 символов.")]
    public string FirstName { get; set; }

    [Required(ErrorMessage = "Фамилия обязательна для заполнения.")]
    [StringLength(50, ErrorMessage = "Фамилия не должна превышать 50 символов.")]
    public string LastName { get; set; }

    [Required(ErrorMessage = "Email обязателен для заполнения.")]
    [EmailAddress(ErrorMessage = "Неверный формат Email.")]
    public string Email { get; set; }

    [Required(ErrorMessage = "Номер телефона обязателен для заполнения.")]
    [StringLength(15, ErrorMessage = "Номер телефона не должен превышать 15 символов.")]
    public string PhoneNumber { get; set; }

    public ICollection<Book>? Books { get; set; }
}
