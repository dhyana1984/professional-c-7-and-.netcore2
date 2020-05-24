using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Security.Principal;

namespace SecuritySample
{
    public static class PrincipalSample
    {
        public static void DisplaySample()
        {
            WindowsIdentity identity = ShowIdentityInformation();

            WindowsPrincipal principal = ShowPrincipal(identity);

            ShowClaims(principal.Claims);
        }

        //Identity包含用户身份验证信息
        //WindowsIdentity实现了IDentity，包含IdentityType，IsAuthenticated和Name3个属性，便于所有派生标识类实现它们
        static WindowsIdentity ShowIdentityInformation()
        {
            WindowsIdentity identity = WindowsIdentity.GetCurrent();
            if (identity == null)
            {
                Console.WriteLine("not a Windows Identity");
                return null;
            }

            identity.AddClaim(new Claim("Age", "25"));


            Console.WriteLine($"IdentityType: {identity}");
            Console.WriteLine($"Name: {identity.Name}");
            Console.WriteLine($"Authenticated: {identity.IsAuthenticated}");
            Console.WriteLine($"Authentication Type: {identity.AuthenticationType}");
            Console.WriteLine($"Anonymous? {identity.IsAnonymous}");
            Console.WriteLine($"Access Token: {identity.AccessToken.DangerousGetHandle()}");
            Console.WriteLine();
            return identity;
        }

        //Principal包含用户标识和用户所属的角色对象
        //Principal实现了IPrincipalI接口，提供了IsInRole和Identity属性
        static WindowsPrincipal ShowPrincipal(WindowsIdentity identity)
        {
            Console.WriteLine("Show principal information");
            WindowsPrincipal principal = new WindowsPrincipal(identity);
            if (principal == null)
            {
                Console.WriteLine("not a Windows Principal");
                return null;
            }
            //验证用户是否属于内置角色User和Administrator
            //WindowsBuiltInRole是角色字符串的枚举
            Console.WriteLine($"Users? {principal.IsInRole(WindowsBuiltInRole.User)}");
            Console.WriteLine($"Administrators? {principal.IsInRole(WindowsBuiltInRole.Administrator)}");
            Console.WriteLine();
            return principal;
        }

        public static void ShowClaims(IEnumerable<Claim> claims)
        {
            Console.WriteLine("Claims");
            foreach (var claim in claims)
            {
                Console.WriteLine($"Subject: {claim.Subject}");
                Console.WriteLine($"Issuer: {claim.Issuer}");
                Console.WriteLine($"Type: {claim.Type}");
                Console.WriteLine($"Value type: {claim.ValueType}");
                Console.WriteLine($"Value: {claim.Value}");
                foreach (var prop in claim.Properties)
                {
                    Console.WriteLine($"\tProperty: {prop.Key} {prop.Value}");
                }
                Console.WriteLine();
            }
        }
    }
}

