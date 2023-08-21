using Android.App;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using AndroidX.AppCompat.App;
using Google.Android.Material.FloatingActionButton;
using Google.Android.Material.Snackbar;
using System;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Net.Http;
using Android.Content;

namespace AppHotel
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme.NoActionBar", MainLauncher = true)]
    public class MainActivity : AppCompatActivity
    {
        ImageView imgLogo, imgUser, imgPassword;
        EditText  edtUser, edtPassword;
        Button    btnLogin;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);

            SetContentView(Resource.Layout.activity_main);

            // Relacionar os elementos da ACT
            imgLogo     = FindViewById<ImageView>(Resource.Id.imgLogo);
            imgUser     = FindViewById<ImageView>(Resource.Id.imgUser);
            imgPassword = FindViewById<ImageView>(Resource.Id.imgPassword);
            edtUser     = FindViewById<EditText> (Resource.Id.edtUser);
            edtPassword = FindViewById<EditText> (Resource.Id.edtPassword);
            btnLogin    = FindViewById<Button>   (Resource.Id.btnLogin);

            // Define as imagens da tela login
            imgLogo.SetImageResource    (Resource.Drawable.logo);
            imgUser.SetImageResource    (Resource.Drawable.usuarios);
            imgPassword.SetImageResource(Resource.Drawable.senha);

            // Valida as informações dos campos e realiza o login
            btnLogin.Click += BtnLogin_Click;
        }

        /// <summary>
        /// Realiza as validações necessárias para entrar na aplicação
        /// Chama a tela do Menu Principal.
        /// </summary>
        private async void BtnLogin_Click(object sender, EventArgs e)
        {
            // Desabilita o botão de login
            btnLogin.Enabled = false;

            edtUser.Text     = edtUser.Text.ToString().Trim();
            edtPassword.Text = edtPassword.Text.ToString().Trim();

            // Verifica se possui valor no campo de usuário
            if (edtUser.Text == string.Empty)
            {
                Toast.MakeText(Application.Context, "Usuário deve ser informado!", ToastLength.Long).Show();

                edtUser.RequestFocus();

                // Reativa o botão de login
                btnLogin.Enabled = true;

                return;
            }

            // Verifica se possui valor no campo de senha
            if (edtPassword.Text == string.Empty)
            {
                Toast.MakeText(Application.Context, "Senha deve ser informada!", ToastLength.Long).Show();

                edtPassword.RequestFocus();

                // Reativa o botão de login
                btnLogin.Enabled = true;

                return;
            }

            // Instancia um LoginService
            Authentication auth = new Authentication();

            // Realiza a requisição de login e obtém o retorno
            LoginResponse loginResponse = await auth.AuthenticateUser(edtUser.Text, edtPassword.Text);

            // Verifica se há algum na resposta da requisição
            if (loginResponse.Erro == null)
            {
                var menu = new Intent(this, typeof(MenuPrincipal));

                menu.PutExtra("name", loginResponse.Nome);
                menu.PutExtra("position", loginResponse.Cargo);

                // Chama o layout do menu principal da aplicação
                StartActivity(menu);

                // Limpa os campos da tela de login
                Limpar();
            }
            else
            {
                // Informa o erro ocorrido para o usuário
                Toast.MakeText(Application.Context, loginResponse.Erro, ToastLength.Long).Show();
            }

            // Reativa o botão de login
            btnLogin.Enabled = true;
        }

        /// <summary>
        /// Método responsável por limpar os campos da tela de login
        /// </summary>
        private void Limpar()
        {
            edtUser.Text     = string.Empty;
            edtPassword.Text = string.Empty;

            edtUser.RequestFocus();
        }

    }
}
