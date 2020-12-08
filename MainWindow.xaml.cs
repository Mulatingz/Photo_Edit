using System;
using System.Collections.Generic;
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
using System.Drawing;

namespace BrahimTalb_Projet_Info
{
    /// <summary>
    /// Logique d'interaction pour MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        string cheminE = "";
        string cheminS = "";
        int fonction = 0;
        int Compteur = 0;
        public MainWindow()
        {
            InitializeComponent();
            CheminE.Text = "C:/Users/Admin/Desktop/"; // On initialise les champs d'entrée et de sortie
            CheminS.Text = "C:/Users/Admin/Desktop/";
            cheminS = CheminS.Text + "sortie(X).bmp";
            cheminE = CheminE.Text;


        }

        /// <summary>
        /// Cette fonction permet de choisir quel option on va appliquer à l'image
        /// </summary>
        /// <param name="fonction"></param>
        /// <param name="image"></param>
        private void Apply_Click(object sender, RoutedEventArgs e)
        {
            Erreur.Visibility = Visibility.Hidden;

            cheminE = CheminE.Text;
            Affichage_ImageE.Source = new BitmapImage(new Uri(cheminE, UriKind.Absolute)); // On Affiche l'image passée en entrée
            cheminS = CheminS.Text +"sortie(X).bmp";
            MyImage image = new MyImage(cheminE);

            int fonction = modification.SelectedIndex; // On regarde quel Index de la combobox est selectionné
            if (0 < fonction && fonction <= 14)
            {
                if (fonction == 1)
                {
                    image.Gris();
                }
                if (fonction == 2)
                {
                    image.Noir_Blanc();
                }
                if (fonction == 3)
                {
                    image.Rotation_90();
                }
                if (fonction == 4)
                {
                    image.Rotation_180();
                }
                if (fonction == 5)
                {
                    image.Rotation_270();
                }
                if (fonction == 6)
                {
                    image.Miroir();
                }
                if (fonction == 7)
                {
                    image.Detection_De_Contour();
                }
                if (fonction == 8)
                {
                    image.Renforcement_des_Bords();
                }
                if (fonction == 9)
                {
                    image.Flou();
                }
                if (fonction == 10)
                {
                    image.Repoussage();
                }
                if (fonction == 11)
                {
                    // On fait apparaitre les champs permettant de modifier le coefficient de la fonction
                    Label_Coef_1.Content = "Coefficient d'agrandissement";
                    Label_Coef_1.Visibility = Visibility.Visible;
                    Coefficient1.Visibility = Visibility.Visible;
                    Valider.Visibility = Visibility.Visible;
                }
                if (fonction == 12)
                {
                    Label_Coef_1.Content = "Coefficient de Retrecissement";
                    Label_Coef_1.Visibility = Visibility.Visible;
                    Coefficient1.Visibility = Visibility.Visible;
                    Valider.Visibility = Visibility.Visible;
                }
                if (fonction == 13)
                {
                    Label_Coef_1.Content = "Coefficient 1";
                    Label_Coef_1.Visibility = Visibility.Visible;
                    Coefficient1.Visibility = Visibility.Visible;
                    Valider.Visibility = Visibility.Visible;
                }
                if (fonction == 14)
                {
                    image.Histogramme();
                }
            }
            else
            {
                Erreur.Content = " Veuillez saisir un type de modification.";
                Erreur.Visibility = Visibility.Visible;
            }
            if(0 < fonction && fonction <= 14 && fonction != 11 && fonction != 12 && fonction != 13)
            {
                // on affiche l'image en sortie
                string a = cheminS.Replace("(X)", "(" + Convert.ToString(Compteur) + ")"); // Obligatoire de créer un nouveau fichier a chaque fois car on ne peut pas fermer l'image que l'on vient 
                image.EcritureImage(a);
                Affichage_ImageS.Source = new BitmapImage(new Uri(a, UriKind.Absolute));
                Compteur++;
            }
        }

        private void Valider_Click(object sender, RoutedEventArgs e)
        {
            MyImage image = new MyImage(cheminE);
            string emplacement = cheminS.Replace("(X)", "(" + Convert.ToString(Compteur) + ")");
            if (fonction == 11)
            {
                int foisA = Convert.ToInt32(Coefficient1.Text);
                image.Aggrandissement(foisA, emplacement);
            }
            if (fonction == 12)
            {
                int foisR = Convert.ToInt32(Coefficient1.Text);
                image.Retrecissement(foisR, emplacement);
            }
            if (fonction == 13)
            {
                int longueur = Convert.ToInt32(Coefficient1.Text);
                image.Fractale(longueur,emplacement);
            }
            //image.EcritureImage(emplacement);
            Affichage_ImageS.Source = new BitmapImage(new Uri(emplacement, UriKind.Absolute));
            Compteur++;
            Label_Coef_1.Visibility = Visibility.Hidden;
            Coefficient1.Visibility = Visibility.Hidden;
            Valider.Visibility = Visibility.Hidden;
        }

    }
}


