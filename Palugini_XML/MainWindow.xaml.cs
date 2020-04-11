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
        CancellationTokenSource ct;
        public MainWindow()
        {
            InitializeComponent();
        }


        private void btn_aggiungi_Click(object sender, RoutedEventArgs e)
        {
            ct = new CancellationTokenSource();
            btn_aggiungi.IsEnabled = false;
            btn_stop.IsEnabled = true;
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
            Thread.Sleep(1000);

            foreach (var item in xmlstudente)
            {
                XElement xmlLastName = item.Element("cognome");
                XElement xmlFirstName = item.Element("nome");
                XElement xmlPresenze = item.Element("presenze");
                Studente s = new Studente();
                s.Cognome = xmlLastName.Value;
                s.Nome = xmlFirstName.Value;
                s.Presenze = Convert.ToInt32(xmlPresenze.Value);

                Dispatcher.Invoke(() => lst_ListaAlunni.Items.Add(s));

                if (ct.Token.IsCancellationRequested)
                {
                    break;
                }
                Thread.Sleep(1000);                
            }

            Dispatcher.Invoke(() =>
            {
                btn_aggiungi.IsEnabled = true;
                btn_stop.IsEnabled = false;
                ct = null;
            });
        }


        private void btn_stop_Click(object sender, RoutedEventArgs e)
        {
            ct.Cancel();
        }

        private void lst_ListaAlunni_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            lbl_studente.IsEnabled = true;
            lbl_presenze.IsEnabled = true;
            btn_modifica.IsEnabled = true;
            txt_modifica.IsEnabled = true;

            Studente s = (Studente)lst_ListaAlunni.SelectedItem;
            if (s != null)
            {
                lbl_studente.Content = s.ToString();
                txt_modifica.Text = s.Presenze.ToString();
            }
        }

        private void btn_modifica_Click(object sender, RoutedEventArgs e)
        {
            Studente s = (Studente)lst_ListaAlunni.SelectedItem;
            int valore = Convert.ToInt32(txt_modifica.Text);
            if (s.Presenze != valore)
            {
                s.Presenze = valore;
                MessageBox.Show("Operazione eseguita con successo, OK");
            }
            Task.Factory.StartNew(Scrivi);

            lbl_studente.IsEnabled = false;
            lbl_presenze.IsEnabled = false;
            btn_modifica.IsEnabled = false;
            txt_modifica.IsEnabled = false;
        }


        private void Scrivi()
        {
            string path = @"ListaAlunni.xml";

            XElement xmlstudenti = new XElement("studenti");

            foreach (Studente studente in lst_ListaAlunni.Items)
            {
                XElement xmlstudente = new XElement("studente");
                XElement xmlcognome = new XElement("cognome");
                XElement xmlnome = new XElement("nome");
                XElement xmlpresenze = new XElement("presenze");

                xmlstudente.Add(xmlcognome);
                xmlstudente.Add(xmlnome);
                xmlstudente.Add(xmlpresenze);
                xmlstudenti.Add(xmlstudente);
            }
            xmlstudenti.Save(path);
        }
    }
}
