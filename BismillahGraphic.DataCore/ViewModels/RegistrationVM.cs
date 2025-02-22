﻿namespace BismillahGraphic.DataCore
{

    public class AdminBasic
    {
        public int RegistrationID { get; set; }
        public string Name { get; set; }
        public string UserName { get; set; }
        public string Type { get; set; }
        public byte[] Image { get; set; }
    }
    public class AdminInfo : AdminBasic
    {
        public string FatherName { get; set; }
        public string Designation { get; set; }
        public string DateofBirth { get; set; }
        public string NationalID { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
    }
}
