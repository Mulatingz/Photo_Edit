using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrahimTalb_Projet_Info
{
    public class Pixel
    {
        /// <summary>
        /// Cette classe permet de créer un pixel composé de ces bytes rouge ,vert et bleu
        /// </summary>

        //Attributs
        byte rouge;
        byte vert;
        byte bleu;

        // Constructeur
        public Pixel(byte[] rgb)
        {
            this.rouge = rgb[0];
            this.vert = rgb[1];
            this.bleu = rgb[2];
        }

        // Propriétés
        public byte Rouge
        {
            get { return rouge; }
            set { rouge = value; }
        }
        public byte Vert
        {
            get { return vert; }
            set { vert = value; }
        }
        public byte Bleu
        {
            get { return bleu; }
            set { bleu = value; }
        }


        // Méthodes
        public string toString()
        {
            return rouge + ";" + vert + ";" + bleu;
        }

    }
}

