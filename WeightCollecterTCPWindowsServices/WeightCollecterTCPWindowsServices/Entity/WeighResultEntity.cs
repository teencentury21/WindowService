namespace Inventec.FIS.Model.Domiain.Entity
{
    public class WeighResultEntity
    {
        public WeighResultEntity()
        {
            Weight = 0;
            Error = "";
            Overload = "N";
        }

        /// <summary>
        /// 
        /// </summary>
        public double Weight { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Error { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Overload { get; set; }

        public string RawData { get; set; }
        public string SN { get; set; }
        public double GrossWeight { get; set; }
        public double NetWeight { get; set; }
        public int Length { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public string Weigher { get; set; }
    }
}
