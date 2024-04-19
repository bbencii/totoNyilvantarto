using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
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
using System.Collections;

namespace totoNyilvantarto
{
    //git init
    /// <summary>
    /// A kulonbozo gombnyomasra es szovegbevitelre torteno funkciokat megvalosito metodusok osztalya
    /// </summary>
    public partial class MainWindow : Window
    {
        double hazaiSzorzo = 0; double dontetlenSzorzo = 0; double vendegSzorzo = 0;
        double hazaiSzazalek = 0; double dontetlenSzazalek = 0; double vendegSzazalek = 0;
        int megjatszottHasabok = 0;

        List<Tippek> meccsekList = new List<Tippek>();
        List<visszaolvasottTippek> visszaolvasottMeccsek = new List<visszaolvasottTippek>();
        public MainWindow()
        {
            InitializeComponent();
        }
        // odds esely kalkulator funkciok
        private void szazalekSzamitas_button_Click(object sender, RoutedEventArgs e)
        {
            hazaiSzazalek = Math.Round(1 / hazaiSzorzo * 100, 2);
            dontetlenSzazalek = Math.Round(1 / dontetlenSzorzo * 100, 2);
            vendegSzazalek = Math.Round(1 / vendegSzorzo * 100, 2);
            hazaiEsely_label.Content = $"{Convert.ToString(hazaiSzazalek)}%";
            dontetlenEsely_label.Content = $"{Convert.ToString(dontetlenSzazalek)}%";
            vendegEsely_label.Content = $"{Convert.ToString(vendegSzazalek)}%";
        }

        private void hazaiOdds_TextChanged(object sender, TextChangedEventArgs e)
        {
            hazaiSzorzo = double.Parse(hazaiOdds.Text);
        }

        private void dontetlenOdds_TextChanged(object sender, TextChangedEventArgs e)
        {
            dontetlenSzorzo = double.Parse(dontetlenOdds.Text);
        }

        private void vendegOdds_TextChanged(object sender, TextChangedEventArgs e)
        {
            vendegSzorzo = double.Parse(vendegOdds.Text);
        }

        // odds esely kalkulator funkciok vege

        // toto adatbazis funkciok itt kezdodnek
        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {
            if (sender is RadioButton radioButton && radioButton.IsChecked == true)
            {
                switch (radioButton.Name)
                {
                    case "hasabSzam_is_one":
                        hasab1_comboBox.IsEnabled = true; hasab2_comboBox.IsEnabled = false; 
                        hasab3_comboBox.IsEnabled = false; hasab4_comboBox.IsEnabled = false; 
                        hasab5_comboBox.IsEnabled = false; megjatszottHasabok = int.Parse((string)radioButton.Content);
                        meccsNeve_box.IsEnabled = true;
                        break;
                    case "hasabSzam_is_two":
                        hasab1_comboBox.IsEnabled = true; hasab2_comboBox.IsEnabled = true; 
                        hasab3_comboBox.IsEnabled = false; hasab4_comboBox.IsEnabled = false; 
                        hasab5_comboBox.IsEnabled = false; megjatszottHasabok = int.Parse((string)radioButton.Content);
                        meccsNeve_box.IsEnabled = true;
                        break;
                    case "hasabSzam_is_three":
                        hasab1_comboBox.IsEnabled = true; hasab2_comboBox.IsEnabled = true; 
                        hasab3_comboBox.IsEnabled = true; hasab4_comboBox.IsEnabled = false; 
                        hasab5_comboBox.IsEnabled = false; megjatszottHasabok = int.Parse((string)radioButton.Content);
                        meccsNeve_box.IsEnabled = true;
                        break;
                    case "hasabSzam_is_four":
                        hasab1_comboBox.IsEnabled = true; hasab2_comboBox.IsEnabled = true; 
                        hasab3_comboBox.IsEnabled = true; hasab4_comboBox.IsEnabled = true; 
                        hasab5_comboBox.IsEnabled = false; megjatszottHasabok = int.Parse((string)radioButton.Content);
                        meccsNeve_box.IsEnabled = true;
                        break;
                    case "hasabSzam_is_five":
                        hasab1_comboBox.IsEnabled = true; hasab2_comboBox.IsEnabled = true; 
                        hasab3_comboBox.IsEnabled = true; hasab4_comboBox.IsEnabled = true; 
                        hasab5_comboBox.IsEnabled = true; megjatszottHasabok = int.Parse((string)radioButton.Content);
                        meccsNeve_box.IsEnabled = true;
                        break;
                    default:
                        break;
                }
            }
        }

        private void meccsEsTippHozzaadasa_Click(object sender, RoutedEventArgs e)
        {
            // checkelni az ures inputot es a radio buttonokat biztos ami biztos (vegul az elozo lepesben megoldottam igy itt nemkell :))
            if (meccsNeve_box.Text.Length == 0)
            {
                MessageBox.Show("Kérlek add meg a meccs nevét!", "Hiba");
            }
            else
            {
                Tippek t = new Tippek();
                t.meccsNeve = meccsNeve_box.Text; t.hasab1 = hasab1_comboBox.Text; 
                t.hasab2 = hasab2_comboBox.Text; t.hasab3 = hasab3_comboBox.Text; 
                t.hasab4 = hasab4_comboBox.Text;t.hasab5 = hasab5_comboBox.Text;
                       
                meccsekList.Add(t);
                meccsekListBox.Items.Add($"{meccsNeve_box.Text} — {hasab1_comboBox.Text}  {hasab2_comboBox.Text}  {hasab3_comboBox.Text}  {hasab4_comboBox.Text}  {hasab5_comboBox.Text} ");
                    
                // minden input torlese hogy hozza lehessen adni a kovetkezo meccset
                meccsNeve_box.Clear(); hasab1_comboBox.SelectedItem = null;
                hasab2_comboBox.SelectedItem = null; hasab3_comboBox.SelectedItem = null;    
                hasab4_comboBox.SelectedItem = null; hasab5_comboBox.SelectedItem = null;
            }
        }

        private void kiiratasButton_Click(object sender, RoutedEventArgs e)
        {
            //tippek mentese kesobb olvashato txt-fileba
            using (StreamWriter sw = new StreamWriter("tippek_olvashato.txt"))
            {
                foreach (var sor in meccsekList)
                {
                    sw.Write($"{sor.meccsNeve}\t{sor.hasab1}\t{sor.hasab2}\t{sor.hasab3}\t{sor.hasab4}\t{sor.hasab5}\n");

                }
            }
            MessageBox.Show("A fájlbaírás megtörtént!", "Fájlbaírás");

            /*using (StreamWriter sw = new StreamWriter("tippek.txt"))
            {
                // ez csak esztetikailag fontos ha bele akarok nezni, amugy nem
                sw.Write("Meccs\t\t\t\t\t\t\tTippek");
                sw.Write("\n————————————————————————————————————————————————————————————————————————————————————————————————————");
                foreach (var sor in meccsekList)
                {
                    sw.Write($"\n{sor.meccsNeve}\t\t\t\t{sor.hasab1}  {sor.hasab2}  {sor.hasab3}  {sor.hasab4}  {sor.hasab5}");
                    sw.Write("\n————————————————————————————————————————————————————————————————————————————————————————————————————");
                }
            }*/
        }
        
        // toto adatbazis funkciok vege

        // talalatkereso/ellenorzo itt kezdodik
        private void button2_Click(object sender, RoutedEventArgs e)
        {
            // visszaolvasom az elozo fuggvenyben kiirt file-t..
            using (StreamReader sr = new StreamReader("tippek_olvashato.txt"))
            {
                while (!sr.EndOfStream)
                {
                    visszaolvasottTippek v = new visszaolvasottTippek();
                    string[] s = sr.ReadLine().Split('\t');
                    v.vmeccsNev = s[0];
                    v.vhasab1 = s[1];
                    v.vhasab2 = s[2];
                    v.vhasab3 = s[3];
                    v.vhasab4 = s[4];
                    v.vhasab5 = s[5];
                    visszaolvasottMeccsek.Add(v);
                }
            }

            foreach (var sor in visszaolvasottMeccsek)
            {
                sajatTippek_listBox.Items.Add($"{sor.vmeccsNev} — {sor.vhasab1} {sor.vhasab2} {sor.vhasab3} {sor.vhasab4} {sor.vhasab5}");
            }
            button2.IsEnabled = false;
        }

        private void kereses_Button_Click(object sender, RoutedEventArgs e)
        {
            //Debug.WriteLine("gomb megnyomva..");
            string nyertesTippsor = nyertesTippek_box.Text;
            string[] ellenorizendo_hasabok = { "vhasab1", "vhasab2", "vhasab3", "vhasab4", "vhasab5" };
            int hasab1DB = 0, hasab2DB = 0, hasab3DB = 0, hasab4DB = 0, hasab5DB = 0;

            if (nyertesTippek_box.Text.Length == 0)
            {
                MessageBox.Show("Kérlek add meg a forduló nyertes tippsorát!", "Hiba");
            }
            else
            {
                // eloszor vegighasitok a tombon, hogy a switchnek jok legyenek a hasab parameterek
                for (int i = 0; i < ellenorizendo_hasabok.Length; i++)
                {
                    //most meg a visszatoltott meccslistan
                    for (int j = 0; j < visszaolvasottMeccsek.Count; j++)
                    {
                        string jelenlegiOszlop = ellenorizendo_hasabok[i];

                        // es akkor itt vegigszaladok az egyezosegeken, a vegen az output labelbe kiirom
                        // itt meg a szinezeseket is megcsinalom, a reszletezo kiirashoz
                        switch (jelenlegiOszlop)
                        {
                            case "vhasab1":
                                Run run = new Run(visszaolvasottMeccsek[j].vhasab1);
                                if (visszaolvasottMeccsek[j].vhasab1 == nyertesTippsor[j].ToString())
                                {
                                    hasab1DB++; run.Foreground = Brushes.Green; run.FontWeight = FontWeights.Bold;
                                }    
                                else
                                {
                                    run.Foreground = Brushes.Red; run.FontWeight = FontWeights.Bold;
                                }
                                elsoTippek.Inlines.Add(run);
                                break;
                            case "vhasab2":
                                Run run2 = new Run(visszaolvasottMeccsek[j].vhasab2);
                                if (visszaolvasottMeccsek[j].vhasab2 == nyertesTippsor[j].ToString())
                                {
                                    hasab2DB++; run2.Foreground = Brushes.Green; run2.FontWeight = FontWeights.Bold;
                                }
                                else
                                {
                                    run2.Foreground = Brushes.Red; run2.FontWeight = FontWeights.Bold;
                                }
                                masodikTippek.Inlines.Add(run2);
                                break;
                            case "vhasab3":
                                Run run3 = new Run(visszaolvasottMeccsek[j].vhasab3);
                                if (visszaolvasottMeccsek[j].vhasab3 == nyertesTippsor[j].ToString())
                                {
                                    hasab3DB++; run3.Foreground = Brushes.Green; run3.FontWeight = FontWeights.Bold;
                                }
                                else
                                {
                                    run3.Foreground = Brushes.Red; run3.FontWeight = FontWeights.Bold;
                                }
                                harmadikTippek.Inlines.Add(run3);
                                break;
                            case "vhasab4":
                                Run run4 = new Run(visszaolvasottMeccsek[j].vhasab4);
                                if (visszaolvasottMeccsek[j].vhasab4 == nyertesTippsor[j].ToString())
                                {
                                    hasab4DB++; run4.Foreground = Brushes.Green; run4.FontWeight = FontWeights.Bold;
                                }
                                else
                                {
                                    run4.Foreground = Brushes.Red; run4.FontWeight = FontWeights.Bold;
                                }
                                negyedikTippek.Inlines.Add(run4);
                                break;
                            case "vhasab5":
                                Run run5 = new Run(visszaolvasottMeccsek[j].vhasab5);
                                if (visszaolvasottMeccsek[j].vhasab5 == nyertesTippsor[j].ToString())
                                {
                                    hasab5DB++; run5.Foreground = Brushes.Green; run5.FontWeight = FontWeights.Bold;
                                }
                                else
                                {
                                    run5.Foreground = Brushes.Red; run5.FontWeight = FontWeights.Bold;
                                }
                                otodikTippek.Inlines.Add(run5);
                                break;
                            default:
                                //Debug.WriteLine("ha ezt latom vmi rohadt nagy baj van..");
                                break;
                        }
                    }
                }
            }

            //output szovegek a labelben
            if (hasab1DB >= 10) { elsoT_Copy.Foreground = Brushes.Green; elsoT_Copy.FontWeight = FontWeights.Bold; elsoT_Copy.Content = hasab1DB.ToString() + " találat"; }
            else { elsoT_Copy.Foreground = Brushes.Red; elsoT_Copy.FontWeight = FontWeights.Bold; elsoT_Copy.Content = hasab1DB.ToString() + " találat"; }

            if (hasab2DB >= 10) { masodikT_Copy.Foreground = Brushes.Green; masodikT_Copy.FontWeight = FontWeights.Bold; masodikT_Copy.Content = hasab2DB.ToString() + " találat"; }
            else { masodikT_Copy.Foreground = Brushes.Red; masodikT_Copy.FontWeight = FontWeights.Bold; masodikT_Copy.Content = hasab2DB.ToString() + " találat"; }

            if (hasab3DB >= 10) { harmadikT_Copy.Foreground = Brushes.Green; harmadikT_Copy.FontWeight = FontWeights.Bold; harmadikT_Copy.Content = hasab3DB.ToString() + " találat"; }
            else { harmadikT_Copy.Foreground = Brushes.Red; harmadikT_Copy.FontWeight = FontWeights.Bold; harmadikT_Copy.Content = hasab3DB.ToString() + " találat"; }

            if (hasab4DB >= 10) { negyedikT_Copy.Foreground = Brushes.Green; negyedikT_Copy.FontWeight = FontWeights.Bold; negyedikT_Copy.Content = hasab4DB.ToString() + " találat"; }
            else { negyedikT_Copy.Foreground = Brushes.Red; negyedikT_Copy.FontWeight = FontWeights.Bold; negyedikT_Copy.Content = hasab4DB.ToString() + " találat"; }

            if (hasab5DB >= 10) { otodikT_Copy.Foreground = Brushes.Green; otodikT_Copy.FontWeight = FontWeights.Bold; otodikT_Copy.Content = hasab5DB.ToString() + " találat"; }
            else { otodikT_Copy.Foreground = Brushes.Red; otodikT_Copy.FontWeight = FontWeights.Bold; otodikT_Copy.Content = hasab5DB.ToString() + " találat"; }
        }
        // talalatkereso vege itt van, meg ugy mindennek
    }
}
