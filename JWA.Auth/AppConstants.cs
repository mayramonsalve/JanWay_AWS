using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JWA.Auth
{
    public class AppConstants
    {
        public const string InviteEmailTemplate= "<html>< head ></ head >< body >< img src='' width='500' height='+600+'><a href = 'www.w3schools.com' > Visit website ></a><h1>Welcome to JanWay</h1></br><p>Lorem ipsum dolor sirt amet consectetur adipiscing elit laoreet, dapibus nunc bibendum scelerisque litora erat mollis egestas quisque justo curabitur.</p></br><button href = '{0}' > START HERE</button></body></html>";
        public const string InviteEmailSubject= "Invite to collaborate with JanWay - {0}";
        public const string ForgetPasswordEmailSubject= "Please Reset Password";
        public const string ForgetPasswordEmailBody= "<html>< head ></ head >< body >< img src='' width='500' height='600'><a href = 'https://www.w3schools.com' > Visit website ></a><h1>Did you forget your password?</h1></br><p>Don't worry, click on the link bellow to reset your password and set a new one.</p></br><div id='token' hidden>{0}</div><button> RECOVER PASSWORD</button></body></html>";
    }
}
