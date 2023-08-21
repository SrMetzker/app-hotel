using Android.App;
using Android.Graphics;
using Android.OS;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace AppHotel
{
    [Activity(Label = "Movimentacoes")]
    public class Movimentacoes : Activity
    {
        readonly CultureInfo culture = new CultureInfo("pt-BR");

        double totalEntrada, totalSaida;

        TextView tvInput, tvOutput, tvBalance;

        DatePicker date;
        ListView list;

        string selectedDateString;
        DateTime selectedDateTime;

        List<string> listMov = new List<string>();

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your application here
            SetContentView(Resource.Layout.Movimentacoes);

            tvInput   = FindViewById<TextView>(Resource.Id.tvInput);
            tvOutput  = FindViewById<TextView>(Resource.Id.tvOutput);
            tvBalance = FindViewById<TextView>(Resource.Id.tvBalance);

            tvInput.SetTextColor(Color.DarkGreen);
            tvOutput.SetTextColor(Color.DarkRed);
            tvBalance.SetTextColor(Color.DarkGreen);

            date = FindViewById<DatePicker>(Resource.Id.datePicker);
            list = FindViewById<ListView>(Resource.Id.listView);

            date.DateChanged += Date_DateChanged;

            string data = String.IsNullOrEmpty(selectedDateString) ? DateTime.Today.ToString("yyyy-MM-dd") : selectedDateString;

            BuscarMovimentacoes(data);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <exception cref="System.NotImplementedException"></exception>
        private void Date_DateChanged(object sender, DatePicker.DateChangedEventArgs e)
        {
            selectedDateString = e.Year + "-" + 0 + (e.MonthOfYear + 1) + "-" + e.DayOfMonth;

            Toast.MakeText(Application.Context, selectedDateString, ToastLength.Long).Show();

            selectedDateTime = Convert.ToDateTime(selectedDateString);

            BuscarMovimentacoes(selectedDateString);
        }

        private async void BuscarMovimentacoes(string data)
        {
            Movimentacao mov     = new Movimentacao();
            Movimentacao Entrada = new Movimentacao();
            Movimentacao Saida   = new Movimentacao();

            listMov.Clear();

            List<Movimentacao> lista = await mov.ObterMovimentacoesData(data);

            for (int i = 0; i < lista.Count; i++)
            {
                listMov.Add(lista[i].Tipo + " - " + lista[i].Movimento + ": " + lista[i].Valor);
            }

            ArrayAdapter<string> adapter = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleListItem1, listMov);

            list.Adapter = adapter;

            Entrada = await Entrada.TotalizarEntradas(data);
            totalEntrada = Entrada.Valor;
            tvInput.Text = "Entradas: " + totalEntrada.ToString("C2", culture);

            Saida = await Saida.TotalizarSaidas(data);
            totalSaida = Saida.Valor;
            tvOutput.Text = "Saídas: " + totalSaida.ToString("C2", culture);

            double saldo = totalEntrada - totalSaida;

            // Define a cor do texto com base no valor do saldo
            tvBalance.SetTextColor(saldo < 0 ? Color.DarkRed : Color.DarkGreen);

            tvBalance.Text = "Saldo: " + saldo.ToString("C2", culture);
        }
    }
}