namespace BismillahGraphic.DataCore
{
    public interface IInstitutionRepository : IRepository<Institution>
    {
        void UpdateCustom(InstitutionVM model);
        InstitutionVM FindCustom();
        HomeVM HomeInfo();
    }
}
