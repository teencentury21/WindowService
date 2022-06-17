using Inventec.FIS.Common;
namespace Inventec.FIS.Model.Domiain.Constrants
{
    public class FileStockCategories : Descriptor<string>
    {
        private static readonly FileStockTypes _WeighFile = new FileStockTypes("WeighFile");
        public static FileStockTypes WeighFile
        {
            get { return _WeighFile; }
        }
        private static readonly FileStockTypes _Weigh = new FileStockTypes("Weigh");
        public static FileStockTypes Weigh
        {
            get { return _Weigh; }
        }
        

        public FileStockCategories()
        {

        }
        public FileStockCategories(string value) : base(value)
        {

        }
    }

}
