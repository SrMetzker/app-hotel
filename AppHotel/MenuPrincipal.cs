using Android.App;
using Android.Content;
using Android.OS;
using Android.Widget;
using System.Globalization;
using Android.Graphics;
using System;

namespace AppHotel
{
    [Activity(Label = "MenuPrincipal")]
    public class MenuPrincipal : Activity
    {
        readonly CultureInfo culture = new CultureInfo("pt-BR");

        double totalEntrada, totalSaida;

        ImageView imgUser, imgMov;
        TextView tvUser, tvPosition, tvInput, tvOutput, tvBalance;
        ImageButton btnGastos, btnMoviment, btnCheckIn, btnReservas;
        string hoje = DateTime.Today.ToString("yyyy-MM-dd");

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your application here
            SetContentView(Resource.Layout.MenuPrincipal);

            imgUser    = FindViewById<ImageView>(Resource.Id.imgUser);
            tvUser     = FindViewById<TextView> (Resource.Id.tvUser);
            tvPosition = FindViewById<TextView> (Resource.Id.tvPosition);
            imgMov     = FindViewById<ImageView>(Resource.Id.imgMov);
            tvInput    = FindViewById<TextView> (Resource.Id.tvInput);
            tvOutput   = FindViewById<TextView> (Resource.Id.tvOutput);
            tvBalance  = FindViewById<TextView> (Resource.Id.tvBalance);

            btnGastos   = FindViewById<ImageButton>(Resource.Id.imgBtnGastos);
            btnMoviment = FindViewById<ImageButton>(Resource.Id.imgBtnMoviment);
            btnReservas = FindViewById<ImageButton>(Resource.Id.imgBtnReservas);
            btnCheckIn  = FindViewById<ImageButton>(Resource.Id.imgBtnCheckIn);

            tvInput.SetTextColor(Color.DarkGreen);
            tvOutput.SetTextColor(Color.DarkRed);
            tvBalance.SetTextColor(Color.DarkGreen);

            imgUser.SetImageResource(Resource.Drawable.users);
            imgMov.SetImageResource (Resource.Drawable.Movimentacoes);

            btnGastos.SetImageResource  (Resource.Drawable.Gastos);
            btnMoviment.SetImageResource(Resource.Drawable.Movimentacao);
            btnReservas.SetImageResource(Resource.Drawable.Reservas);
            btnCheckIn.SetImageResource (Resource.Drawable.CheckIn);

            btnMoviment.Click += BtnMoviment_Click;
            btnGastos.Click += BtnGastos_Click;

            tvUser.Text     = Intent.GetStringExtra("name");
            tvPosition.Text = Intent.GetStringExtra("position");

            TotalizarBalanco();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <exception cref=""></exception>
        private void BtnGastos_Click(object sender, EventArgs e)
        {
            var gastos = new Intent(this, typeof(Gastos));

            gastos.PutExtra("name", tvUser.Text);
            gastos.PutExtra("position", tvPosition.Text);

            StartActivity(gastos);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <exception cref=""></exception>
        private void BtnMoviment_Click(object sender, EventArgs e)
        {
            var movimentacoes = new Intent(this, typeof(Movimentacoes));

            StartActivity(movimentacoes);
        }

        /// <summary>
        /// 
        /// </summary>
        private async void TotalizarBalanco()
        {
            Movimentacao Entrada = new Movimentacao();
            Movimentacao Saida = new Movimentacao();

            Entrada = await Entrada.TotalizarEntradas(hoje);
            totalEntrada = Entrada.Valor;
            tvInput.Text = "Entradas: " + totalEntrada.ToString("C2", culture);

            Saida = await Saida.TotalizarSaidas(hoje);
            totalSaida = Saida.Valor;
            tvOutput.Text = "Saídas: " + totalSaida.ToString("C2", culture);

            double saldo = totalEntrada - totalSaida;

            // Define a cor do texto com base no valor do saldo
            tvBalance.SetTextColor(saldo < 0 ? Color.DarkRed : Color.DarkGreen);

            tvBalance.Text = "Saldo: " + saldo.ToString("C2", culture);
        }
    }
}