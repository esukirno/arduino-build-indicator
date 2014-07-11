using System;

namespace BuildIndicator.AlertDialler
{
    public class Member
    {
        public Member(string userName, string phoneNumber)
        {
            if (string.IsNullOrEmpty( userName)) throw new ArgumentNullException("userName");
            if (string.IsNullOrEmpty(userName)) throw new ArgumentNullException("phoneNumber");

            UserName = userName;
            PhoneNumber = phoneNumber;
        }

        public string UserName { get; private set; }
        public string PhoneNumber { get; private set; }
    }
}