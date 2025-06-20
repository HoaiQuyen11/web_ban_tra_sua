﻿using System.ComponentModel.DataAnnotations;

namespace ShopManager.Areas.Admin.Models
{
    public class CategoryAdmin
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "*")]
        [MaxLength(100, ErrorMessage = "Tối đa 100 kí tự")]
        public string Title { get; set; }

        public DateTime CreateAt { get; set; }
        public DateTime UpdateAt { get; set; }
    }
}
