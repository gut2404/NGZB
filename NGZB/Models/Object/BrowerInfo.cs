namespace NGZB.Models.Object
{
    public class BrowerInfo
    {
        public string Type { get; set; }
        public string Browser { get; set; }
        public string Version { get; set; }
        public bool VBScript { get; set; }
        public bool Cookies { get; set; }
        public bool ActiveXControls { get; set; }
        public bool AOL { get; set; }
        public string UserHostAddress { get; set; }
        public string UserHostName { get; set; }
        public string DnsSafeHost { get; set; }
        public int Port { get; set; }
    }
}