namespace BismillahGraphic.DataCore
{
    public partial class PageLinkAssign
    {
        public int LinkAssignID { get; set; }
        public int RegistrationID { get; set; }
        public int LinkID { get; set; }

        public virtual PageLink PageLink { get; set; }
        public virtual Registration Registration { get; set; }
    }
}
