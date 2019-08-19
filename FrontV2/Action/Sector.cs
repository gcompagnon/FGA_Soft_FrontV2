using System;

namespace FrontV2.Action
{
    class Sector
    {
        private int _id;
        private String _libelle;

        public int Id
        {
            get
            {
                return _id;
            }
            set
            {
                _id = value;
            }
        }

        public String Libelle
        {
            get
            {
                return _libelle;
            }
            set
            {
                _libelle = value;
            }
        }

        public Sector()
        {
        }

        public Sector(int id, String libelle)
        {
            _id = id;
            _libelle = libelle;
        }
    }
}