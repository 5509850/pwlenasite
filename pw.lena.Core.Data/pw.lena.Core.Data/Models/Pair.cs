namespace pw.lena.Core.Data.Models
{
    public class Pair
    {
        public bool isCodeAExpired { get; set; }
        public int CodeA { get;  set; }
        public int CodeB { get; set; }
        public int TimeOutValidCodeA { get; set; }//{ //return timeOutValidCodeSecund; } }
        public string ErrorMessage
        {
            get; set;
        }
    }
}
