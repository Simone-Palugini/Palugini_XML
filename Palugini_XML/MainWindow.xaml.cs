using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml.Linq;

namespace Palugini_XML
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        bool token = false;

        private void btn_aggiungi_Click(object sender, RoutedEventArgs e)
        {
            token = true;
            lst_ListaAlunni.Items.Clear();
            Task.Factory.StartNew(()=>CaricaDati());
        }
        
        private void CaricaDati()
        {
            Studente studenti = new Studente();
            string path = @"ListaAlunni.xml";
            XDocument xmlDoc = XDocument.Load(path);
            XElement xmlstudenti = xmlDoc.Element("studenti");
            var xmlstudente = xmlstudenti.Elements("studente");

            foreach (var item in xmlstudente)
            {
                XElement xmlLastName = item.Element("cognome");
                XElement xmlFirstName = item.Element("nome");
                XElement xmlPresenze = item.Element("presenze");
                Studente s = new Studente();
                s.Cognome = xmlLastName.Value;
                s.Nome = xmlFirstName.Value;
                s.Presenze = Convert.ToInt32(xmlPresenze.Value);
                studenti = s;
                Dispatcher.Invoke(() => lst_ListaAlunni.Items.Add(studenti));
                Thread.Sleep(800);
                if (!token)
                    break;
            }                   
        }

        private void btn_stop_Click(object sender, RoutedEventArgs e)
        {
            token = false;
        }
    }
}
