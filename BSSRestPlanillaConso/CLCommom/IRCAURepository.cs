
namespace CLCommom
{
    public interface IRCAURepository
    {
        string ExisteUSR(string user, string pass);
        RCAU DatosUSR(string user, string pass);
    }
}
