using System.ComponentModel.DataAnnotations;

namespace MVCpersonalsite.Models
{
    public class ContactViewModel
    {
        //We can use DataAnnotations to add validation to our model.
        //This is useful when we have required fields or need cretain types of info.

        [Required(ErrorMessage = "* Required")]
        public string Name { get; set; } = null!;

        [Required(ErrorMessage = "* Required")]
        public string Company { get; set; } = null!;

        [Required(ErrorMessage = "* Required")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; } = null!;

        [Required(ErrorMessage = "* Required")]
        public string Subject { get; set; } = null!;

        [Required(ErrorMessage = "* Required")]
        [DataType(DataType.MultilineText)]//Gives us a larger textbox (textarea)
        public string Message { get; set; } = null!;

    }
}

