using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace wedding_planner.Models
{
    public class User
    {
        [Key]
        public int UserId { get; set; }


        [Display(Name = "First Name")]
        [Required(ErrorMessage = "Please enter a first name.")]
        [RegularExpression("^[A-Za-z ]+$", ErrorMessage = "First Name may only contain letters and spaces")]
        public string FirstName { get; set; }


        [Display(Name = "Last Name")]
        [Required(ErrorMessage = "Please enter a last name.")]
        [RegularExpression("^[A-Za-z ]+$", ErrorMessage = "Last Name may only contain letters and spaces")]
        public string LastName { get; set; }


        [Required(ErrorMessage = "Please enter an email address.")]
        [DataType(DataType.Text)]
        [EmailAddress(ErrorMessage = "Please enter a valid email address")]
        public string Email {get; set;}


        [Required(ErrorMessage = "You must enter a password.")]
        [DataType(DataType.Password)]
        [MinLength(8, ErrorMessage = "Password must be at least 8 characters in length.")]
        public string Password { get; set; }



        public List<Wedding> WeddingsCreated { get; set; }



        [Display(Name = "Confirm Password")]
        [NotMapped]
        [Compare("Password", ErrorMessage = "Passwords must match.")]
        [DataType(DataType.Password)]
        public string ConfirmPW { get; set; }

        
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;
    }
}