using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ProjBank.Models
{
    public class Wishlist
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public virtual int AccountId { get; set; }
        public virtual Account Account { get; set; }
        [DataType(DataType.Date)]
        [Display(Name = "Date Added")]
        [ReadOnly(true)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime DateAdded { get; set; }
        [Required]
        [DataType(DataType.Currency)]
        [Range(0, int.MaxValue, ErrorMessage = "Cost cannot be a negative value")]
        public decimal Cost { get; set; }
        [Required]
        public string Description { get; set; }
        [Url]
        public string Link { get; set; }
        public bool Purchased { get; set; }
    }
}