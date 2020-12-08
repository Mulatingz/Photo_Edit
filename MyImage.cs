using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace BrahimTalb_Projet_Info
{
    public class MyImage
    {/// <summary>
     /// Description des attributs de la classe MyImage (cette classe permet de lire et d'écrire une image sous format bmp)
     /// Le tableau de bit myfile correspond au tableau que l'on reçois après la lecture d'image Il contient le header et le header indo ainsi que les pixels qui constituent l'image
     /// On séquence ce tableau alors en plusieurs variables pour pouvoir les modifiers et les adapter aux différentes modifications que l'on oppérera sur les l'image . (ex: noir et blanc,miroir,...)
     /// Les tableaux taille_fich, correspond à la taille de l'image, Il est composé des cases à du tableau myfile.
     /// Les tableaux header, headerinfo permettent de sauvegarder les données concernenant le descriptif de l'image (taille, largeur, hauteur, lecture ,...)
     /// hauteur et largeur, nous donnent la taille de la matrice de pixel à créer pour modifier l'image
     /// La matrice de pixels Image regroupe tout les pixels composant l'image. Cf classe pixel.
     /// </summary>
        // Attributs
        public byte[] myfile;
        byte[] taillefich;
        public byte[] header;
        public byte[] headerInfo;
        int hauteur;
        int largeur;
        public Pixel[,] Image;


        // Constructeurs
        public MyImage(string file) // (string file, bool afficheimage)
        {
            myfile = File.ReadAllBytes(file);

            // on recupère la taille du fichier
            #region
            taillefich = new byte[4];
            for (int i = 2; i < 6; i++)
            { taillefich[i - 2] = myfile[i]; }
            Console.WriteLine(myfile.Length);
            #endregion

            //On récupère le Header
            #region
            header = new byte[14];
            for (int i = 0; i < 14; i++)
            {
                header[i] = myfile[i];
            }
            #endregion


            // On récupère le HeaderInfo
            #region
            headerInfo = new byte[40];
            for (int i = 14; i < 54; i++)
            {
                headerInfo[i - 14] = myfile[i];
            }
            #endregion

            // largeur de l'image (en pixel) ==> nbr de colonne de la matrice
            #region 
            //int entier = myfile[2];
            byte[] Largeur = new byte[4];
            for (int i = 18; i < 22; i++)
            { Largeur[i - 18] = myfile[i]; }
            largeur = Conversion_Byte_to_Int(Largeur);
            Console.WriteLine(largeur);
            #endregion

            // hauteur de l'image ==> nbr de ligne de la matrice
            #region
            byte[] Hauteur = new byte[4];
            for (int i = 22; i < 26; i++)
            { Hauteur[i - 22] = myfile[i]; }
            hauteur = Conversion_Byte_to_Int(Hauteur);
            Console.WriteLine(hauteur);
            // Hauteur conversion en int
            #endregion

            // création d'une matrice image
            byte[] tableau_intermédiaire = new byte[3];
            int compteurligne = 0;
            int compteurcolonne = 0;
            Image = new Pixel[hauteur, largeur];
            for (int i = 54; i < myfile.Length - 2; i = i + 3)
            {
                tableau_intermédiaire[0] = myfile[i];
                tableau_intermédiaire[1] = myfile[i + 1];
                tableau_intermédiaire[2] = myfile[i + 2];
                Image[compteurcolonne, compteurligne] = new Pixel(tableau_intermédiaire); // on prend un bit rouge, un bit vert et un bit bleu pour construire un pixel

                compteurligne++;
                if (compteurligne == Image.GetLength(1))
                {
                    compteurligne = 0;
                    compteurcolonne++;
                }

            }
        }

        // Propriétés
        public int Hauteur
        {
            get { return hauteur; }
            set { hauteur = value; }
        }
        public int Largeur
        {
            get { return this.largeur; }
            set { largeur = value; }
        }

        /// <summary>
        /// Methodes pour afficher l'image 
        /// Pour créer l'image on injecte dans un tableau de byte le header et le header info, puis on modifie la taille, la largeur et la longueur de l'image
        /// Puis on vide la matrice de pixel dans le tableau de byte tout en les séparant les bites de chaque pixel
        /// </summary>
        public void EcritureImage(string chemin)
        {
            byte[] file;
            file = new byte[Image.GetLength(0) * Image.GetLength(1) * 3 + 54];
            // on rempli le header
            for (int i = 0; i < 14; i++)
            {
                file[i] = header[i];
            }
            // on rempli le header info
            for (int i = 14; i < 54; i++)
            {
                file[i] = headerInfo[i - 14];
            }
            // on modifie la taille du fichier (en cas de retrecissement ou agrandissement)
            for (int i = 2; i < 6; i++)
            {
                file[i] = Conversion_Int_to_Byte(file.Length, 4)[i - 2];
            }
            // on modifie la largeur de ce dernier en cas de modification de la taille de l'image ou de demi tour
            for (int i = 18; i < 22; i++)
            {
                file[i] = Conversion_Int_to_Byte(Image.GetLength(1), 4)[i - 18];
            }
            // on modifie la hauteur de ce dernier en cas de modification de la taille de l'image ou de demi tour
            for (int i = 22; i < 26; i++)
            {
                file[i] = Conversion_Int_to_Byte(Image.GetLength(0), 4)[i - 22];
            }

            int cpt = 54;
            // on transverse la matrice de pixel dans le tableau
            for (int i = 0; i < Image.GetLength(0); i++)
            {
                for (int j = 0; j < Image.GetLength(1); j++)
                {
                    file[cpt] = Image[i, j].Rouge;
                    cpt++;
                    file[cpt] = Image[i, j].Vert;
                    cpt++;
                    file[cpt] = Image[i, j].Bleu;
                    cpt++;
                }
            }
            File.WriteAllBytes(chemin, file) ;
        }

        /// <summary>
        /// Cette fonction convertit des bytes en entier 
        /// </summary>Le processus ne peut pas accéder au fichier 'C:\Users\Admin\Desktop\coco.bmp', car il est en cours d'utilisation par un autre processus.'
        /// <param name="tab">prend en paramètre un tableau de byte</param>
        /// <returns> retourne la valeur du tableau de byte sous forme de int </returns>
        public int Conversion_Byte_to_Int(byte[] tab)
        {
            double Conversion = 0;
            for (int i = 0; i < tab.Length; i++)
            {
                Conversion += tab[i] * Math.Pow(16, i * 2);
            }
            return Convert.ToInt32(Conversion);
        }
        /// <summary>
        /// Permet de convertir un entier en tableau de bit dont on aura donnée la taille au préalable
        /// Cette fontion est utilisé par exemple lorsqu'on veut changer la taille de l'image
        /// </summary>
        /// <param name="entier"> entier que l'on souhaite transformé en bites </param>
        /// <param name="nbr_de_bit"> taille du tableau (correspond à la taille que la donnée occupe dans le header ou le headeinfo)</param>
        /// <returns>tableau de bites</returns>
        public byte[] Conversion_Int_to_Byte(int entier, int nbr_de_bit)
        {
            byte[] tabinverse = new byte[nbr_de_bit];
            byte[] Conversion = new byte[nbr_de_bit];

            for (int i = 0; i < nbr_de_bit; i++)
            {
                tabinverse[i] = (byte)Math.Truncate(entier / Math.Pow(16, Convert.ToDouble((nbr_de_bit - 1 - i) * 2)));
                entier = entier % Convert.ToInt32(Math.Pow(16, Convert.ToDouble((nbr_de_bit - 1 - i) * 2)));
            }
            for (int i = 0; i < nbr_de_bit; i++)
            {
                Conversion[i] = tabinverse[nbr_de_bit - 1 - i];
            }
            return Conversion;
        }

        // Methodes pour modifier l'image 

        /// <summary>
        /// Rotation 90°
        /// Fait tourner une image à 90°  en jouant avec la lecture et l'écriture de la matrice
        /// </summary>
        public void Rotation_90()
        {
            Pixel[,] new_Image = new Pixel[Image.GetLength(1), Image.GetLength(0)];
            for (int i = 0; i < Image.GetLength(1); i++)
            {
                for (int j = 0; j < Image.GetLength(0); j++)
                {
                    new_Image[i, j] = Image[j, i];
                }
            }
            Image = new_Image;

        }
        /// <summary>
        /// Rotation à 180°
        /// Fait tourner une image à 180° (Initialement nous souhaitions utilisée deux fois la focntion à 90° mais nous n'avons pas réussi
        /// </summary>
        public void Rotation_180()
        {
            for (int i = 0; i < Image.GetLength(0) / 2; i++)
            {
                for (int j = 0; j < Image.GetLength(1); j++)
                {
                    Pixel save = Image[i, j];
                    Image[i, j] = Image[Image.GetLength(0) - i - 1, j];
                    Image[Image.GetLength(0) - i - 1, j] = save;
                }
            }
        }
        /// <summary>
        /// Fait tourner une image à 270° en utilisant les fonction 180° et 90° 
        /// </summary>
        public void Rotation_270()
        {
            Rotation_180();
            Rotation_90();
        }

        /// <summary>
        /// Nuance de gris:
        /// On passe l'image en nuances de gris, pour cela dans chaque pixels on mets les bits vert, bleu et rouge au meme niveau d'intenisté (pour cela on calcule leur moyenne)
        /// </summary>
        public void Gris()
        {
            int moyenne;

            for (int i = 0; i < Image.GetLength(0); i++)
            {
                for (int j = 0; j < Image.GetLength(1); j++)
                {
                    moyenne = Convert.ToInt32((Image[i, j].Rouge + Image[i, j].Vert + Image[i, j].Bleu) / 3);

                    Image[i, j].Rouge = (byte)moyenne;
                    Image[i, j].Vert = (byte)moyenne;
                    Image[i, j].Bleu = (byte)moyenne;
                }
            }
            Console.WriteLine("oui");
        }
        /// <summary>
        /// Noir et blanc: On passe l'image en noir et blanc pour ça on calcule la moyenne de chaque pixel. si cette moyenne est superieur à 128 alors le pixel est blanc sinon il est noir
        /// </summary>
        public void Noir_Blanc()
        {
            int moyenne;
            for (int i = 0; i < Image.GetLength(0); i++)
            {
                for (int j = 0; j < Image.GetLength(1); j++)
                {
                    moyenne = Convert.ToInt32((Image[i, j].Rouge + Image[i, j].Vert + Image[i, j].Bleu) / 3);
                    if (moyenne < 128)
                    {
                        Image[i, j].Rouge = 0;
                        Image[i, j].Vert = 0;
                        Image[i, j].Bleu = 0;
                    }
                    else
                    {
                        Image[i, j].Rouge = 255;
                        Image[i, j].Vert = 255;
                        Image[i, j].Bleu = 255;
                    }
                }
            }

        }
        /// <summary>
        /// Miroir
        /// on échange déplaces les pixels dans la matrice de façon à avoir un effet miroir. C'est à dire que la colonne la plus à gauche dans la matrice est échangé avec la plus à droite et ainsi de suite.
        /// </summary>
        public void Miroir()
        {
            for (int i = 0; i < Image.GetLength(0); i++)
            {
                for (int j = 0; j < Image.GetLength(1) / 2; j++)
                {
                    Pixel save = Image[i, j];
                    Image[i, j] = Image[i, Image.GetLength(1) - j - 1];
                    Image[i, Image.GetLength(1) - j - 1] = save;
                }
            }
        }


        /// <summary>
        /// Detection de Contour:
        /// Multiplication de la matrice image avec la matrice de convolution puis traitement des éléments qui débordent et on les clonent dans une matrice de même taille
        /// </summary>
        public void Detection_De_Contour()
        {
            int[,] detec = { { 0, 1, 0 }, { 1, -4, 1 }, { 0, 1, 0 } };
            Pixel[,] Image_2 = new Pixel[Hauteur + 2, Largeur + 2];

            for (int a = 1; a < Hauteur + 1; a++)
            {
                for (int b = 1; b < Largeur + 1; b++)
                {
                    Image_2[a, b] = Image[a - 1, b - 1];
                }
            }
            for (int z = 1; z < Image_2.GetLength(0) - 1; z++)
            {
                Image_2[z, 0] = Image_2[z, 1];
                Image_2[z, Image_2.GetLength(1) - 1] = Image_2[z, Image_2.GetLength(1) - 2];
            }
            for (int z = 0; z < Image_2.GetLength(1); z++)
            {
                Image_2[0, z] = Image_2[1, z];
                Image_2[Image_2.GetLength(0) - 1, z] = Image_2[Image_2.GetLength(0) - 2, z];
            }
            int rouge = 0;
            int vert = 0;
            int bleu = 0;

            for (int i = 1; i < Image.GetLength(0) + 1; i++)
            {
                for (int j = 1; j < Image.GetLength(1) + 1; j++)
                {
                    rouge = 0;
                    vert = 0;
                    bleu = 0;
                    int div = 0;
                    for (int k = -1; k < 2; k++)
                    {
                        for (int l = -1; l < 2; l++)
                        {
                            div += detec[l + 1, k + 1];
                            rouge += Image_2[i + k, j + l].Rouge * detec[l + 1, k + 1];
                            vert += Image_2[i + k, j + l].Vert * detec[l + 1, k + 1];
                            bleu += Image_2[i + k, j + l].Bleu * detec[l + 1, k + 1];
                        }
                    }
                    if (div > 1)
                    {
                        rouge = rouge / div;
                        vert = vert / div;
                        bleu = bleu / div;
                    }
                    if (rouge > 255)
                    {
                        rouge = 255;
                    }
                    if (rouge < 0)
                    {
                        rouge = 0;
                    }
                    if (vert > 255)
                    {
                        vert = 255;
                    }
                    if (vert < 0)
                    {
                        vert = 0;
                    }
                    if (bleu > 255)
                    {
                        bleu = 255;
                    }
                    if (bleu < 0)
                    {
                        bleu = 0;
                    }

                    byte[] pix = { (byte)rouge, (byte)vert, (byte)bleu };
                    Image[i - 1, j - 1] = new Pixel(pix);
                }
            }

        }
        /// <summary>
        /// Renforcement_des_Bords:
        /// Multiplication de la matrice image avec la matrice de convolution puis traitement des éléments qui débordent et on les clonent dans une matrice de même taille
        /// </summary>
        public void Renforcement_des_Bords()
        {
            int[,] renforcement = { { 0, 0, 0 }, { -1, 1, 0 }, { 0, 0, 0 } };
            Pixel[,] Image_2 = new Pixel[Hauteur + 2, Largeur + 2];
            for (int a = 1; a < Hauteur + 1; a++)
            {
                for (int b = 1; b < Largeur + 1; b++)
                {
                    Image_2[a, b] = Image[a - 1, b - 1];
                }
            }
            for (int z = 1; z < Image_2.GetLength(0) - 1; z++)
            {
                Image_2[z, 0] = Image_2[z, 1];
                Image_2[z, Image_2.GetLength(1) - 1] = Image_2[z, Image_2.GetLength(1) - 2];
            }
            for (int z = 0; z < Image_2.GetLength(1); z++)
            {
                Image_2[0, z] = Image_2[1, z];
                Image_2[Image_2.GetLength(0) - 1, z] = Image_2[Image_2.GetLength(0) - 2, z];
            }
            int rouge = 0;
            int vert = 0;
            int bleu = 0;
            for (int i = 1; i < Image.GetLength(0) + 1; i++)
            {
                for (int j = 1; j < Image.GetLength(1) + 1; j++)
                {
                    rouge = 0;
                    vert = 0;
                    bleu = 0;
                    int div = 0;
                    for (int k = -1; k < 2; k++)
                    {
                        for (int l = -1; l < 2; l++)
                        {
                            div += renforcement[l + 1, k + 1];
                            rouge += Image_2[i + k, j + l].Rouge * renforcement[l + 1, k + 1];
                            vert += Image_2[i + k, j + l].Vert * renforcement[l + 1, k + 1];
                            bleu += Image_2[i + k, j + l].Bleu * renforcement[l + 1, k + 1];
                        }
                    }
                    if (div > 1)
                    {
                        rouge = rouge / div;
                        vert = vert / div;
                        bleu = bleu / div;
                    }
                    if (rouge > 255)
                    {
                        rouge = 255;
                    }
                    if (rouge < 0)
                    {
                        rouge = 0;
                    }
                    if (vert > 255)
                    {
                        vert = 255;
                    }
                    if (vert < 0)
                    {
                        vert = 0;
                    }
                    if (bleu > 255)
                    {
                        bleu = 255;
                    }
                    if (bleu < 0)
                    {
                        bleu = 0;
                    }
                    byte[] nouveaupixel = { (byte)rouge, (byte)vert, (byte)bleu };
                    Image[i - 1, j - 1] = new Pixel(nouveaupixel);
                }
            }

        }
        /// <summary>
        /// Flou:
        /// Multiplication de la matrice image avec la matrice de convolution puis traitement des éléments qui débordent et on les clonent dans une matrice de même taille
        /// </summary>
        public void Flou()
        {
            int[,] flou = { { 1, 1, 1 }, { 1, 1, 1 }, { 1, 1, 1 } };
            Pixel[,] Image_2 = new Pixel[Hauteur + 2, Largeur + 2];

            for (int a = 1; a < Hauteur + 1; a++)
            {
                for (int b = 1; b < Largeur + 1; b++)
                {
                    Image_2[a, b] = Image[a - 1, b - 1];
                }
            }
            for (int z = 1; z < Image_2.GetLength(0) - 1; z++)
            {
                Image_2[z, 0] = Image_2[z, 1];
                Image_2[z, Image_2.GetLength(1) - 1] = Image_2[z, Image_2.GetLength(1) - 2];
            }
            for (int z = 0; z < Image_2.GetLength(1); z++)
            {
                Image_2[0, z] = Image_2[1, z];
                Image_2[Image_2.GetLength(0) - 1, z] = Image_2[Image_2.GetLength(0) - 2, z];
            }
            int rouge = 0;
            int vert = 0;
            int bleu = 0;

            for (int i = 1; i < Image.GetLength(0) + 1; i++)
            {
                for (int j = 1; j < Image.GetLength(1) + 1; j++)
                {
                    rouge = 0;
                    vert = 0;
                    bleu = 0;
                    int div = 0;
                    for (int k = -1; k < 2; k++)
                    {
                        for (int l = -1; l < 2; l++)
                        {
                            div += flou[l + 1, k + 1];
                            rouge += Image_2[i + k, j + l].Rouge * flou[l + 1, k + 1];
                            vert += Image_2[i + k, j + l].Vert * flou[l + 1, k + 1];
                            bleu += Image_2[i + k, j + l].Bleu * flou[l + 1, k + 1];
                        }
                    }

                    if (div > 1)
                    {
                        rouge = rouge / div;
                        vert = vert / div;
                        bleu = bleu / div;
                    }
                    if (rouge > 255)
                    {
                        rouge = 255;
                    }
                    if (rouge < 0)
                    {
                        rouge = 0;
                    }
                    if (vert > 255)
                    {
                        vert = 255;
                    }
                    if (vert < 0)
                    {
                        vert = 0;
                    }
                    if (bleu > 255)
                    {
                        bleu = 255;
                    }
                    if (bleu < 0)
                    {
                        bleu = 0;
                    }

                    byte[] nouveaupixel = { (byte)rouge, (byte)vert, (byte)bleu };
                    Image[i - 1, j - 1] = new Pixel(nouveaupixel);
                }
            }

        }
        /// <summary>
        /// Repoussage:
        /// Multiplication de la matrice image avec la matrice de convolution puis traitement des éléments qui débordent et on les clonent dans une matrice de même taille
        /// </summary>
        public void Repoussage()
        {
            int[,] repoussage = { { -2, -1, 0 }, { -1, 1, 1 }, { 0, 1, 2 } };
            Pixel[,] Image_2 = new Pixel[Hauteur + 2, Largeur + 2];

            for (int a = 1; a < Hauteur + 1; a++)
            {
                for (int b = 1; b < Largeur + 1; b++)
                {
                    Image_2[a, b] = Image[a - 1, b - 1];
                }
            }
            for (int z = 1; z < Image_2.GetLength(0) - 1; z++)
            {
                Image_2[z, 0] = Image_2[z, 1];
                Image_2[z, Image_2.GetLength(1) - 1] = Image_2[z, Image_2.GetLength(1) - 2];
            }
            for (int z = 0; z < Image_2.GetLength(1); z++)
            {
                Image_2[0, z] = Image_2[1, z];
                Image_2[Image_2.GetLength(0) - 1, z] = Image_2[Image_2.GetLength(0) - 2, z];
            }
            int rouge = 0;
            int vert = 0;
            int bleu = 0;

            for (int i = 1; i < Image.GetLength(0) + 1; i++)
            {
                for (int j = 1; j < Image.GetLength(1) + 1; j++)
                {
                    rouge = 0;
                    vert = 0;
                    bleu = 0;
                    int div = 0;
                    for (int k = -1; k < 2; k++)
                    {
                        for (int l = -1; l < 2; l++)
                        {
                            div += repoussage[l + 1, k + 1];
                            rouge += Image_2[i + k, j + l].Rouge * repoussage[l + 1, k + 1];
                            vert += Image_2[i + k, j + l].Vert * repoussage[l + 1, k + 1];
                            bleu += Image_2[i + k, j + l].Bleu * repoussage[l + 1, k + 1];
                        }
                    }

                    if (div > 1)
                    {
                        rouge = rouge / div;
                        vert = vert / div;
                        bleu = bleu / div;
                    }
                    if (rouge > 255)
                    {
                        rouge = 255;
                    }
                    if (rouge < 0)
                    {
                        rouge = 0;
                    }
                    if (vert > 255)
                    {
                        vert = 255;
                    }
                    if (vert < 0)
                    {
                        vert = 0;
                    }
                    if (bleu > 255)
                    {
                        bleu = 255;
                    }
                    if (bleu < 0)
                    {
                        bleu = 0;
                    }

                    byte[] nouveaupixel = { Convert.ToByte(rouge), Convert.ToByte(vert), Convert.ToByte(bleu) };
                    Image[i - 1, j - 1] = new Pixel(nouveaupixel);
                }
            }

        }

        /// <summary>
        /// Fonction permettant de retrecir une image suivant un facteur entré par l'utilisateur.
        /// Le principe de base est de faire la moyenne d'une matrice puis de rassembler les pixels en un. 
        /// </summary>
        /// <param name="fois">le coefficient de réduction que l'on prend en paramètre</param>
        public void Retrecissement(int fois,string emplacement)
        {
            Hauteur /= fois;
            Largeur /= fois;
            Pixel[,] Retrecie = new Pixel[Hauteur, Largeur];
            byte[] zero = { 0, 0, 0 };
            for (int i = 0; i < Retrecie.GetLength(0); i++)
            {
                for (int j = 0; j < Retrecie.GetLength(1); j++)
                {
                    Retrecie[i, j] = new Pixel(zero);
                }
            }
            int r = 0;
            int v = 0;
            int b = 0;
            for (int i = 0; i < Hauteur; i++)
            {
                for (int j = 0; j < Largeur; j++)
                {
                    r = 0;
                    v = 0;
                    b = 0;
                    for (int k = 0; k < fois; k++)
                    {
                        r += Image[i * fois, j * fois].Rouge;
                        v += Image[i * fois, j * fois].Vert;
                        b += Image[i * fois, j * fois].Bleu;
                    }
                    r /= (fois);
                    v /= (fois);
                    b /= (fois);
                    Retrecie[i, j].Rouge = Convert.ToByte(r);
                    Retrecie[i, j].Vert = Convert.ToByte(v);
                    Retrecie[i, j].Bleu = Convert.ToByte(b);

                }
            }
            Image = new Pixel[Hauteur, Largeur];
            Image = Retrecie;
            EcritureImage(emplacement);
        }

        /// <summary>
        /// On va créer une matrice de pixel de taille plus grande qui sera multipliée par un facteur entré par l'utilisateur. 
        /// </summary>
        /// <param name="fois"></param>
        public void Aggrandissement(int fois, string emplacement)
        {
            Hauteur *= fois;
            Largeur *= fois;
            Pixel[,] agrandie = new Pixel[Hauteur, Largeur];
            for (int i = 0; i < Hauteur; i += fois)
            {
                for (int j = 0; j < Largeur; j += fois)
                {
                    agrandie[i, j] = Image[i / fois, j / fois];
                    for (int k = 0; k < fois; k++)
                    {
                        agrandie[i, j + k] = agrandie[i, j];
                    }
                }
            }

            for (int i = 0; i < Hauteur; i++)
            {
                for (int j = 0; j < Largeur; j++)
                {
                    if (agrandie[i, j] == null)
                    {
                        agrandie[i, j] = agrandie[i - 1, j];
                    }
                    else
                        break;
                }
            }

            Image = new Pixel[Hauteur, Largeur];
            for (int i = 0; i < Image.GetLength(0); i++)
            {
                for (int j = 0; j < Image.GetLength(1); j++)
                {
                    Image[i, j] = agrandie[i, j];
                }
            }

            EcritureImage(emplacement);
        }

        public class Complex
        {
            public double Real;
            public double Imaginary;

            public Complex(double Real, double Imaginary)
            {
                this.Real = Real;
                this.Imaginary = Imaginary;
            }

            public void Square()
            {
                double temp = (Real * Real) - (Imaginary * Imaginary);
                Imaginary = 2.0 * Real * Imaginary;
                Real = temp;
            }

            public double Magnitude()
            {
                return Math.Sqrt((Real * Real) + (Imaginary * Imaginary));
            }

            public void Add(Complex c)
            {
                Real += c.Real;
                Imaginary += c.Imaginary;
            }


        }

        public void Proprietes(Pixel[,] image)
        {
            int temp = Conversion_Byte_to_Int(taillefich);
            temp -= Image.GetLength(0) * image.GetLength(1);
            Largeur = image.GetLength(0);
            Hauteur = image.GetLength(1);
            temp += Largeur * Hauteur * 9;
            Image = image;
        }

        public void Fractale(int longueur, string emplacement)
        {
            //Console.WriteLine(" Veuillez rentrer la longueur voulue (supérieure a 100 pour avoir une image claire) : ");
            //int longueur = Convert.ToInt32(Console.ReadLine());
            Image = new Pixel[longueur, longueur];
            Proprietes(Image);

            for (int x = 0; x < Largeur; x++)
            {
                for (int y = 0; y < Hauteur; y++)
                {
                    double a = (double)(x - (Largeur / 2)) / (double)(Largeur / 4);
                    double b = (double)(y - (Hauteur / 2)) / (double)(Hauteur / 4);

                    Complex c = new Complex(a, b);
                    Complex z = new Complex(0, 0);

                    int cmpt = 0;
                    while (cmpt < 100)
                    {
                        cmpt++;
                        z.Square();
                        z.Add(c);
                        if (z.Magnitude() > 2) break;

                    }

                    byte[] mat = { 0, 0, 0 };
                    byte[] mat2 = { 255, 0, 255 };


                    if (cmpt < 100) Image[x, y] = new Pixel(mat);
                    else Image[x, y] = new Pixel(mat2);
                }
            }
            EcritureImage(emplacement);
        }

        public void Histogramme()
        {
            int[] NbCouleursr = new int[256];
            int[] NbCouleursv = new int[256];
            int[] NbCouleursb = new int[256];
            for (int i = 0; i < Image.GetLength(0); i++)
            {
                for (int j = 0; j < Image.GetLength(1); j++)
                {
                    NbCouleursr[Convert.ToInt32(Image[i, j].Rouge)]++;
                    NbCouleursv[Convert.ToInt32(Image[i, j].Vert)]++;
                    NbCouleursb[Convert.ToInt32(Image[i, j].Bleu)]++;
                }
            }
            int maxR = NbCouleursr.Max();
            int maxV = NbCouleursv.Max();
            int maxB = NbCouleursb.Max();
            Image = new Pixel[255 * 3, 255 * 4];
            byte[] mat = { 0, 0, 0 };
            for (int i = 0; i < Image.GetLength(0); i++)
            {
                for (int j = 0; j < Image.GetLength(1); j++)
                {
                    Image[i, j] = new Pixel(mat);
                }
            }
            for (int i = 0; i < Image.GetLength(0) - 1; i += 3)
            {
                for (int j = 0; j < 3; j++)
                {
                    Console.WriteLine(NbCouleursr[i / 3] + " " + i / 3);
                    int yR = 1000 * NbCouleursr[i / 3] / maxR;
                    int yV = 1000 * NbCouleursv[i / 3] / maxV;
                    int yB = 1000 * NbCouleursb[i / 3] / maxB;
                    for (int k = 0; k < yR; k++)
                    {
                        Image[i + j, k].Rouge = 255;
                    }
                    for (int k = 0; k < yV; k++)
                    {
                        Image[i + j, k].Vert = 255;
                    }
                    for (int k = 0; k < yB; k++)
                    {
                        Image[i + j, k].Bleu = 255;
                    }
                }
            }
        }
    }
}
