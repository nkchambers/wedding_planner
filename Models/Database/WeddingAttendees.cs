using System;
using System.ComponentModel.DataAnnotations;

namespace wedding_planner.Models
{
    public class WeddingAttendees
    {
        [Key]
        public int WeddingAttendeesId { get; set; }
        
        [Required]
        public int UserId { get; set; }
        public User AttendedBy { get; set; }

        [Required]
        public int WeddingId { get; set;}
        public Wedding WeddingAttended {get; set;}

        
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;
    }
}