
namespace CLCommom
{
    public class RFCTLKEY
    {
        public string KCATEG { get; set; }
        public string KLLAVE { get; set; }
    }
    public interface IRFCTLKEYRepository
    {
        string Add(RFCTLKEY datos);
    }
}
