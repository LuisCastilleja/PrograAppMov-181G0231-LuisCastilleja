using AppMovilComprasJwt.Views;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Xamarin.Essentials;

namespace AppMovilComprasJwt.Services
{
    public class UserService
    {
        private DateTime fechaExpiracion;
        public ClaimsPrincipal Claims { get; set; }

        //Verificar que no ha caducado el token.
        public bool Autenticado
        {
            get
            {
                var token = SecureStorage.GetAsync("Token").Result;
                return token != null && DateTime.UtcNow <= fechaExpiracion;
            }
        }

        public async void IniciarSesion(string jwt)
        {
            //Leer el token con Hanlder para sacar las claims y mandar la info al SecurityStorage
            var jwtHanlder = new JwtSecurityTokenHandler();
            var token = jwtHanlder.ReadJwtToken(jwt);
            //Checar la audiencia y verificar si es para mi cuando yo no genero el token.
            await SecureStorage.SetAsync("Token", jwt);

            var claims = token.Claims;
            Claims = new ClaimsPrincipal(new ClaimsIdentity(claims));

            fechaExpiracion = token.ValidTo;

        }
        public void CerrarSesion()
        {
            //Borrar el Token y borrar las cosas del SecurityStorage
            SecureStorage.RemoveAll();
            //Para hacer que el token ya no sea valido
            fechaExpiracion = DateTime.MinValue;
            //Limpiar las claims
            Claims = null;
            //Mandar a redirigir
            Redirigir();
        }
        public void Redirigir()
        {
            //Redirigir dependiendo del rol
            if (Autenticado)
            {
                //Si esta autenticado mandamos a la vista de compras
                App.Current.MainPage = new ComprasView();
            }
            else
            {
                //Si no al login, o si se vencio su token.
                App.Current.MainPage = new LoginView();
            }
        }

    }
}
