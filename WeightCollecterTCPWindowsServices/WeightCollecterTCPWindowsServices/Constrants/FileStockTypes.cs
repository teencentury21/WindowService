using Inventec.FIS.Common;
namespace Inventec.FIS.Model.Domiain.Constrants
{
    public class FileStockTypes : Descriptor<string>
    {
        private static readonly FileStockTypes _TCP = new FileStockTypes("TCP");
        public static FileStockTypes TCP
        {
            get { return _TCP; }
        }
        
        private static readonly FileStockTypes _ModbusTCP = new FileStockTypes("ModbusTCP");
        public static FileStockTypes ModbusTCP
        {
            get { return _ModbusTCP; }
        }

        public FileStockTypes()
        {

        }
        public FileStockTypes(string value) : base(value)
        {

        }
    }

}
