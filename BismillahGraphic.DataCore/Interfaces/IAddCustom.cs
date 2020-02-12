namespace BismillahGraphic.DataCore
{
    public interface IAddCustom<in TObject> where TObject : class
    {
        void AddCustom(TObject model);
    }
}
