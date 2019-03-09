using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
namespace BCGSA
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class Settings : ContentPage
	{
		public Settings ()
		{
			InitializeComponent ();
        }

        private void Switch_Toggled(object sender, ToggledEventArgs e)
        {

        }
    }
}