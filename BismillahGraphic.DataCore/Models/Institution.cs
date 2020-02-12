using System;

namespace BismillahGraphic.DataCore
{
    public partial class Institution
    {
        public int InstitutionID { get; set; }
        public string InstitutionName { get; set; }
        public string Dialog_Title { get; set; }
        public string Established { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string LocalArea { get; set; }
        public string PostalCode { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Website { get; set; }
        public byte[] InstitutionLogo { get; set; }
        public DateTime? Date { get; set; }
    }
}
