using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PROG7311_ICEFinal.Models
{
    public class Booking
    {
        [Key]
        public int BookingId { get; set; }

        [Required]
        [Display(Name = "Customer")]
        public int CustomerId { get; set; }

        [ForeignKey("CustomerId")]
        public virtual Customer? Customer { get; set; }

        [Required]
        [Display(Name = "Service Type")]
        public string ServiceType { get; set; } = string.Empty;

        [Required]
        [DataType(DataType.DateTime)]
        [Display(Name = "Booking Date")]
        public DateTime BookingDate { get; set; }

        [Display(Name = "Assigned Technician")]
        public int? TechnicianId { get; set; }

        [ForeignKey("TechnicianId")]
        public virtual Employee? Technician { get; set; }

        [Required]
        [Display(Name = "Status")]
        public string Status { get; set; } = "Pending"; // Pending, In Progress, Completed, Cancelled

        [Display(Name = "Issue Description")]
        public string Description { get; set; } = string.Empty;

        [DataType(DataType.DateTime)]
        [Display(Name = "Assigned Date")]
        public DateTime? AssignedDate { get; set; }

        [DataType(DataType.DateTime)]
        [Display(Name = "Completion Date")]
        public DateTime? CompletionDate { get; set; }
    }
}