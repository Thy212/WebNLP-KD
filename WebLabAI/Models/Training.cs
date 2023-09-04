using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace WebLabAI.Models
{
    public class Training
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? ShortDescription { get; set; }
        public string? Description { get; set; }

        [DataType(DataType.Date, ErrorMessage = "Date is required")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime ToDate { get; set; } // đến ngày or ngày diễn ra
        public bool IsCourse { get; set; }
        public string? ThumbnailUrl { get; set; }
        public ApplicationUser? ApplicationUser { get; set; }
        public string? ApplicationUserId { get; set; }
        public string? Slug { get; set; }
        public int Price { get; set; }
        public string? RegisterUrl { get; set; }
        
        public bool VideoRecord { get; set; }
        public bool Resources { get; set; }
        public bool Assignment { get; set; }
        public bool Certificate { get; set; }
        public bool Online { get; set; }
        public string? Teacher { get; set; }
    }
}
