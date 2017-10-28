using JetBrains.Annotations;
using System;

namespace pw.lena.Core.Data.Models
{
    public class Identity
    {
        private bool sessionHashExpired;

        public event EventHandler SessionExpired;

        public string Login { get; set; }

        public string SessionHash { get; set; }

        public bool SessionHashExpired
        {
            get
            {
                return sessionHashExpired;
            }

            set
            {
                if (sessionHashExpired != value)
                {
                    sessionHashExpired = value;

                    if (sessionHashExpired)
                    {
                        OnSessionExpired();
                    }
                }
            }
        }

        [CanBeNull]
        public DeviceModel deviceModel { get; set; }

        public bool IsAuthenticated()
        {
            return !(string.IsNullOrEmpty(Login) || string.IsNullOrEmpty(SessionHash));
        }

        private void OnSessionExpired()
        {
            SessionExpired?.Invoke(this, System.EventArgs.Empty);
        }
    }
}
