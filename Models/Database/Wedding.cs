using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;



namespace wedding_planner.Models
{
    public class Wedding
    {
        [Key]
        public int WeddingId { get; set; }


        [Display(Name = "Wedder One")]
        [Required(ErrorMessage = "Please enter a name for wedder one.")]
        [RegularExpression("^[A-Za-z ]+$", ErrorMessage = "Name may only contain letters and spaces")]
        public string WedderOne { get; set; }


        [Display(Name = "Wedder Two")]
        [Required(ErrorMessage = "Please enter a name for wedder two.")]
        [RegularExpression("^[A-Za-z ]+$", ErrorMessage = "Name may only contain letters and spaces")]
        public string WedderTwo { get; set; }


        [Display(Name = "Date")]
        [Required(ErrorMessage = "Please enter a date for the wedding.")]
        public DateTime Date { get; set; }


        [Display(Name = "Wedding Address")]
        [Required(ErrorMessage = "Please enter an address for the wedding.")]
        public string WeddingAddress { get; set; }


        //Foreign Key for the User who created any given wedding 
        public int UserId { get; set; }
        

        //Navigation property for that actual User
        public User CreatedBy { get; set; }


        public List<WeddingAttendees> WeddingAttendees { get; set; } = new List<WeddingAttendees>();


        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;

    }
}