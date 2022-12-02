namespace CARRITO_D.Helpers
{
    public class MisMetodos
    {
        public static string getUrl(string root, string nombre, string nombreDefecto)
        {
            string resu = string.Empty;

            resu = string.Concat(root, string.IsNullOrEmpty(nombre)?nombreDefecto:nombre);

            return resu;
        }
    }
}
