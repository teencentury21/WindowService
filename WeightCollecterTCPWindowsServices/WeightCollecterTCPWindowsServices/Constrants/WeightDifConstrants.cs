using Inventec.FIS.Common;
namespace Inventec.FIS.Model.Domiain.Constrants
{
    public class WeightDifConstrants: Descriptor<string>
    {
        private static readonly WeightDifConstrants _KeyErrorMessage = new WeightDifConstrants("Weight Collect Process Got Issue.");
        public static WeightDifConstrants KeyErrorMessage
        {
            get { return _KeyErrorMessage; }
        }

        private static readonly WeightDifConstrants _TcpClient = new WeightDifConstrants("TcpClient_");
        public static WeightDifConstrants TcpClient
        {
            get { return _TcpClient; }
        }
        
        private static readonly WeightDifConstrants _ModbusTCPClient = new WeightDifConstrants("ModbusTCPClient_");
        public static WeightDifConstrants ModbusTCPClient
        {
            get { return _ModbusTCPClient; }
        }

        private static readonly WeightDifConstrants _Exception = new WeightDifConstrants("Exception_");
        public static WeightDifConstrants Exception
        {
            get { return _Exception; }
        }

        private static readonly WeightDifConstrants _PASS = new WeightDifConstrants("PASS");
        public static WeightDifConstrants PASS
        {
            get { return _PASS; }
        }

        private static readonly WeightDifConstrants _FAIL = new WeightDifConstrants("FAIL");
        public static WeightDifConstrants FAIL
        {
            get { return _FAIL; }
        }

        private static readonly WeightDifConstrants _Monitor = new WeightDifConstrants("MONITOR");
        public static WeightDifConstrants Monitor
        {
            get { return _Monitor; }
        }

        private static readonly WeightDifConstrants _IptWeightService = new WeightDifConstrants("IPT Weight Service");
        public static WeightDifConstrants IptWeightService
        {
            get { return _IptWeightService; }
        }

        public WeightDifConstrants()
        {

        }
        protected WeightDifConstrants(string value) : base(value)
        {

        }
    }

}
