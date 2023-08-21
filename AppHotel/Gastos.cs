using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Java.Util;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using static Android.Icu.Text.Transliterator;

namespace AppHotel
{
    [Activity(Label = "Gastos")]
    public class Gastos : Activity
    {
        readonly CultureInfo culture = new CultureInfo("pt-BR");
        public string Descricao { get; set; }
        public double Valor { get; set; }
        public string Funcionario{ get; set; }

        readonly HttpClient _httpClient = new HttpClient();

        EditText edtGasto, edtValor;
        ListView listView;
        Button btnSalvar;

        TextView tvUser, tvPosition;
        ImageView imgUser;

        readonly List<string> listGastos = new List<string>();
        readonly string data = DateTime.Now.ToString("yyyy-MM-dd");

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your application here
            SetContentView(Resource.Layout.Gastos);

            imgUser    = FindViewById<ImageView>(Resource.Id.imgUser);
            tvUser     = FindViewById<TextView>(Resource.Id.tvUser);
            tvPosition = FindViewById<TextView>(Resource.Id.tvPosition);
            edtGasto   = FindViewById<EditText>(Resource.Id.edtGasto);
            edtValor   = FindViewById<EditText>(Resource.Id.edtValor);
            listView   = FindViewById<ListView>(Resource.Id.listView);
            btnSalvar  = FindViewById<Button>(Resource.Id.btnSalvar);

            imgUser.SetImageResource(Resource.Drawable.users);
            tvUser.Text = Intent.GetStringExtra("name");
            tvPosition.Text = Intent.GetStringExtra("position");

            btnSalvar.Click += BtnSalvar_Click;

            Listar();
        }

        private void BtnSalvar_Click(object sender, EventArgs e)
        {
            RegistrarGasto(edtGasto.Text, edtValor.Text, tvUser.Text);
        }

        private async void Listar()
        {
            listGastos.Clear();

            ArrayAdapter<string> adapterVazio = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleListItem1, listGastos);
            listView.Adapter = adapterVazio;

            List<Gastos> lista = await ObterGastosData(data);

            for (int i = 0; i < lista.Count; i++)
            {
                listGastos.Add(lista[i].Funcionario + "  -  " + lista[i].Descricao+ ": " + lista[i].Valor.ToString("C2", culture));
            }

            ArrayAdapter<string> adapter = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleListItem1, listGastos);

            listView.Adapter = adapter;
        }

        public async Task<List<Gastos>> ObterGastosData(string data)
        {
            // Constrói a URL da rota para a API
            string apiUrl = "http://192.168.1.6:8000/obter-gastos?data=" + data;

            // Envia a requisição
            HttpResponseMessage httpResponse = await _httpClient.GetAsync(apiUrl);

            List<Gastos> gastos = await DeserializeJsonContentAsyncArray<List<Gastos>>(httpResponse);

            return gastos;
        }

        public async Task<List<Gastos>> DeserializeJsonContentAsyncArray<Movimentacao>(HttpResponseMessage response)
        {
            string jsonContent = await response.Content.ReadAsStringAsync();

            List<Gastos> gastos= JsonConvert.DeserializeObject<List<Gastos>>(jsonContent);

            return gastos;
        }

        public async Task<T> DeserializeJsonContentAsync<T>(HttpResponseMessage response)
        {
            string jsonContent = await response.Content.ReadAsStringAsync();

            T resp = JsonConvert.DeserializeObject<T>(jsonContent);

            return resp;
        }

        public async void RegistrarGasto (string descricao, string valor, string funcionario)
        {
            // Constrói a URL da rota da API
            string apiUrl = "http://192.168.1.6:8000/inserir-gasto";

            //
            var dados = new { descricao, valor, funcionario };

            // Envia a requisição POST
            HttpResponseMessage httpResponse = await _httpClient.PostAsJsonAsync(apiUrl, dados);

            ApiResponse response = await DeserializeJsonContentAsync<ApiResponse>(httpResponse);

            if (response.AffectedRows < 1)
            {
                Toast.MakeText(Application.Context, "Não foi possível registrar a movimentação!", ToastLength.Long).Show();
            }
            else
            {
                Toast.MakeText(Application.Context, "Movimentação registrada com sucesso!", ToastLength.Long).Show();
                Listar();

                edtGasto.Text = "";
                edtValor.Text = "";

            }
        }
    }
}