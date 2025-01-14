﻿using System.ComponentModel.DataAnnotations;

namespace BismillahGraphic.DataCore
{
    public class InstitutionVM
    {
        public int InstitutionID { get; set; }
        [Required]
        public string InstitutionName { get; set; }
        [Required]
        public string Address { get; set; }
        [Required]
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Website { get; set; }
        public byte[] InstitutionLogo { get; set; }
    }

    public class HomeVM
    {
        public string Address { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
    }
}
