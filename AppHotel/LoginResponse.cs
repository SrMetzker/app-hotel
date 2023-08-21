using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AppHotel
{
    public class LoginResponse
    {
        public string Authorized { get; set; }
        public string Erro { get; set; }
        public string Nome { get; set; }
        public string Usuario { get; set; }
        public string Senha { get; set; }
        public string Cargo { get; set; }
    }
}